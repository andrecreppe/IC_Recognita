using System;
using UnityEngine;

public class Descriptors : MonoBehaviour
{
    //---------------- VARIABLES --------------------

    private readonly double COSSINE_treshold = 62.4; //already x1000
    private readonly double EUCLIDIAN_treshold = 0.0;
    private readonly double CITYBLOCK_treshold = 0.0;

    private int selected_comparator;
    private string compKey;

    //---------------- PREPARATION METHODS --------------------

    /* Cut the image to the red-circle format */
    private Texture2D CutImage(Texture2D raw)
    {
        double calc, w, h;

        //Constants -> Image Crop
        w = 0.20;
        h = 0.58;

        //Cutting constants
        calc = (raw.width / 2) - (raw.width * w / 2);
        int x = Mathf.RoundToInt((float)calc);
        calc = (raw.height / 2) - (raw.height * h / 2);
        int y = Mathf.RoundToInt((float)calc);

        calc = raw.width * w;
        int constW = Mathf.RoundToInt((float)calc);
        calc = raw.height * h;
        int constH = Mathf.RoundToInt((float)calc);

        //Bilding of the new image
        Color[] neww = raw.GetPixels(x, y, constW, constH);
        Texture2D resized = new Texture2D(constW, constH);

        resized.SetPixels(neww);
        resized.Apply();

        return resized;
    }

    /* Change the size of the image without losing qualiy */
    /* Reduce background noise */

    /*private Texture2D ResizeImage(Texture2D pic)
    {
        //PHASE 1 -> Conversion to Integers
        Color[] uni = pic.GetPixels();
        int[,] mat = new int[pic.height, pic.width];
        int count = 0;

        for (int i = 0; i < pic.height; i++)
        {
            for (int j = 0; j < pic.width; j++)
            {
                mat[i, j] = Mathf.RoundToInt(uni[count++].grayscale * 255);
            }
        }

        int med;
        int[,] novo = new int[height, width];

        for (int i = 0; i < height; i+=2)
        {
            for (int j = 0; j < width; j+=2)
            {
                med = img[i,j] + img[i+1,j] + img[i,j+1] + img[i+1,j+1];
                novo[i, j] = Mathf.RoundToInt(med / 4);
            }
        }

        return novo;
    }*/

    //---------------- MATRIX MANIPULATION METHODS --------------------

    /* Multiply two 3x3 Matrix */
    private int[,] MatMultiplyer3x3(int[,] mat1, int[,] mat2)
    {
        int[,] c = new int[3, 3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                c[i, j] = mat1[i, j] * mat2[i, j];
            }
        }

        return c;
    }

    /* Compare a 3x3 matrix to a certain element (binary comparison - 0 or 1) */
    private int[,] CompareToElement3x3(int[,] mat, int elem)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (mat[i, j] > elem)
                    mat[i, j] = 1;
                else
                    mat[i, j] = 0;
            }
        }

        return mat;
    }

    /* Add all elements from one matrix */
    private int SumMat(int[,] mat, int lin, int col)
    {
        int sum = 0;
        for (int i = 0; i < lin; i++)
        {
            for (int j = 0; j < col; j++)
            {
                sum += mat[i, j];
            }
        }

        return sum;
    }

    //---------------- DISTANCE CALCULATORS METHODS --------------------

    /* Calculate the Euclidian Distance between two histograms */
    /* Result = number */
    private double PdistEuclidian(int[] hist1, int[] hist2)
    {
        double euclidian = 0;

        for (int i = 0; i < hist1.Length; i++)
        {
            //Euclidian Distance
            euclidian += Mathf.Pow(hist1[i] - hist2[i], 2);
        }

        euclidian = Mathf.Sqrt((float)euclidian);

        return euclidian;
    }

    /* Calculate the Manhattan Distance between two histograms */
    /* Result = number */
    private double PdistCityBlock(int[] hist1, int[] hist2)
    {
        double manhattan = 0;

        for (int i = 0; i < hist1.Length; i++)
        {
            //Manhattan or CityBlock distance
            manhattan += Mathf.Abs(hist1[i] - hist2[i]);
        }

        return manhattan;
    }

    /* Calculate the Cosine Distance between two histograms (treated as vectors) */
    /* Result = |1 - number| */
    private double PdistCosine(int[] hist1, int[] hist2)
    {
        float sum_produto, sum_x, sum_y, resp;

        sum_produto = 0;
        sum_x = 0; sum_y = 0;

        for (int i = 0; i < hist1.Length; i++)
        {
            sum_produto += hist1[i] * hist2[i];
            sum_x += hist1[i] * hist1[i];
            sum_y += hist2[i] * hist2[i];
        }

        resp = sum_produto / (Mathf.Sqrt(sum_x) * Mathf.Sqrt(sum_y));

        return Mathf.Abs(1 - resp);
    }

    //---------------- LBP METHODS --------------------

    /* Extract a histogram[256] of features from a given image */
    public int[] ExtractLBPFeatures(Texture2D pic)
    {
        //PHASE 1 -> Conversion to Integers
        Color[] uni = pic.GetPixels();
        int[,] mat = new int[pic.height, pic.width];
        int count = 0;

        for (int i = 0; i < pic.height; i++)
        {
            for (int j = 0; j < pic.width; j++)
            {
                mat[i, j] = Mathf.RoundToInt(uni[count++].grayscale * 255);
            }
        }

        ////////////////////////////////////////////
        //PHASE 2 -> Matrix Manipulation
        int[] features = new int[256];
            Array.Clear(features, 0, features.Length);
        int[,] pic_new = new int[pic.height, pic.width];
        int[,] mini_area = new int[3, 3];
        int[,] pesos = {
            { 1, 2, 4 },
            { 128, 0, 8 },
            { 64, 32, 16 }
        };


        for (int i = 1; i < pic.height - 2; i++)
        {
            for (int j = 1; j < pic.width - 2; j++)
            {
                mini_area[0, 0] = mat[i - 1, j - 1];
                mini_area[0, 1] = mat[i - 1, j];
                mini_area[0, 2] = mat[i - 1, j + 1];
                mini_area[1, 0] = mat[i, j - 1];
                mini_area[1, 1] = mat[i, j];
                mini_area[1, 2] = mat[i, j + 1];
                mini_area[2, 0] = mat[i + 1, j - 1];
                mini_area[2, 1] = mat[i + 1, j];
                mini_area[2, 2] = mat[i + 1, j + 1];

                pic_new[i, j] = SumMat(
                    MatMultiplyer3x3(
                        CompareToElement3x3(mini_area, mini_area[1, 1]), pesos
                    ), 3, 3
                );

                ////////////////////////////////////////////
                //PHASE 3 -> Features sorting
                features[pic_new[i, j]] += 1;
            }
        }

        return features;
    }

    //---------------- PRIVATE METHODS --------------------

    private void Start()
    {
        compKey = "descriptor";

        if(!PlayerPrefs.HasKey(compKey)) //First time
        {
            PlayerPrefs.SetInt(compKey, 3); //Cossine = default
            PlayerPrefs.Save();

            selected_comparator = 3;
        }
        else
        {
            selected_comparator = PlayerPrefs.GetInt(compKey);
        }
    }

    //---------------- PUBLIC METHODS - Calculation --------------------

    /* Compare two images using the LBP method, with a given distance method */
    /* Best options: Cosine > Euclidian > Cityblock */
    public double CompareImages(Texture2D img1, Texture2D img2)
    {
        int[] features1, features2;
        double resp = 0;

        features1 = ExtractLBPFeatures(img1);
        features2 = ExtractLBPFeatures(img2);

        switch(selected_comparator)
        {
            case 1:
                resp = PdistCityBlock(features1, features2);
                break;
            case 2:
                resp = PdistEuclidian(features1, features2);
                break;
            case 3:
                resp = PdistCosine(features1, features2) * 10000;
                break;
        }

        return resp;
    }
    public double CompareImages(int[] features1, int[] features2)
    {
        double resp = -1;

        switch (selected_comparator)
        {
            case 1:
                resp = PdistCityBlock(features1, features2);
                break;
            case 2:
                resp = PdistEuclidian(features1, features2);
                break;
            case 3:
                resp = PdistCosine(features1, features2) * 10000;
                break;
        }

        return resp;
    }

    //---------------- PUBLIC METHODS - Info --------------------

    /* Get the treshold for the comparator in use */
    public double ActiveTreshold()
    {
        double tresh = 0;

        switch (selected_comparator)
        {
            case 1:
                tresh = CITYBLOCK_treshold;
                break;
            case 2:
                tresh = EUCLIDIAN_treshold;
                break;
            case 3:
                tresh = COSSINE_treshold;
                break;
        }

        return tresh;
    }

    /* Selected comparator numeric representent */
    public void SetDescriptorInUse(int desc)
    {
        selected_comparator = desc;
    }
    public int GetDescriptorInUse()
    {
        return selected_comparator;
    }

    /* Get the info about the selected descriptor */
    public string DescriptorInfo(int lang)
    {
        string resp = "- ";

        switch (GetDescriptorInUse())
        {
            case 3: //Cossine
                switch (lang)
                {
                    case 1:
                        resp += "Na configuração atual" +
                            "\n<b>(Cosseno)</b> ela representa" +
                            "\na distância vetorial entre" +
                            "\nseus pixeis.";

                        break;

                    //English
                    case 2:
                        resp += "The selected descriptor" +
                            "\n<b>(Cossine)</b> represents the " +
                            "\nvectorial distance between" +
                            "\nthe image pixels.";

                        break;

                    //Deutsch
                    case 3:
                        resp += "- Der ausgewählte Deskriptor " +
                            "\n<b>(Cossine)</b> stellt die vektoriell " +
                            "\nAbstand zwischen die \nBildpunkte.";

                        break;
                }
                break;

            case 2: //Euclidian
                switch (lang)
                {
                    case 1:
                        resp += "";

                        break;

                    //English
                    case 2:
                        resp += "";

                        break;

                    //Deutsch
                    case 3:
                        resp += "";

                        break;
                }
                break;

            case 1: //Cityblock
                switch (lang)
                {
                    case 1:
                        resp += "";

                        break;

                    //English
                    case 2:
                        resp += "";

                        break;

                    //Deutsch
                    case 3:
                        resp += "";

                        break;
                }
                break;
        }

        return resp;
    }
}
