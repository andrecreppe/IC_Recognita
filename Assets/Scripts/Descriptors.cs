using System;
using UnityEngine;

public class Descriptors : MonoBehaviour
{
    //---------------- VARIABLES --------------------

    public readonly double COSSINE_treshold = 62.4; //already x100
    //public readonly double EUCLIDIAN_treshold = 0.0;
    //public readonly double CITYBLOCK_treshold = 0.0;

    //---------------- PREPARATION METHODS --------------------

    /* Convert a image to gray */
    private Texture2D ConvertToGrayscale(Texture2D graph)
    {
        Color32[] pixels = graph.GetPixels32();
        for (int x = 0; x < graph.width; x++)
        {
            for (int y = 0; y < graph.height; y++)
            {
                Color32 pixel = pixels[x + y * graph.width];
                int p = ((256 * 256 + pixel.r) * 256 + pixel.b) * 256 + pixel.g;
                int b = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int g = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int r = p % 256;
                float l = (0.2126f * r / 255f) + 0.7152f * (g / 255f) + 0.0722f * (b / 255f);
                Color c = new Color(l, l, l, 1);
                graph.SetPixel(x, y, c);
            }
        }
        graph.Apply(false);

        return graph;
    }

    /* Resize the image to make the LBP */
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
    private int[] ExtractLBPFeatures(Texture2D pic)
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
                //PHASE 3 -> Features Generation
                features[pic_new[i, j]] += 1;
            }
        }

        return features;
    }


    //---------------- PUBLIC METHODS --------------------

    /* Compare two images using the LBP method, with a given distance method */
    /* Best options: Cosine > Euclidian > Cityblock */
    public double CompareImages(Texture2D img1, Texture2D img2)
    {
        int[] features1, features2;
        double resp;

        features1 = ExtractLBPFeatures(img1);
        features2 = ExtractLBPFeatures(img2);

        //resp = PdistEuclidian(features1, features2);
        //resp = PdistCityBlock(features1, features2);
        resp = PdistCosine(features1, features2) * 100;

        return resp;
    }
}
