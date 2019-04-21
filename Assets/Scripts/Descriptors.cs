using System;
using UnityEngine;

public class Descriptors : MonoBehaviour
{
    //---------------- VARIABLES --------------------

    public readonly double LBP_treshold = 36.4;

    //---------------- PREPARATION METHODS --------------------

    private Texture2D ConvertToGrayscale(Texture2D graph) //Gray the image
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

    private Texture2D CutImage(Texture2D raw) //Resize for make the LBP
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

    //---------------- LBP PREQUIRED METHODS --------------------

    private int[,] MatMultiplyer3x3(int[,] mat1, int[,] mat2) //Multiply 3x3 Matrix
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
    private int[,] CompareToElement3x3(int[,] mat, int elem) //Compare a 3x3 matrix to a certain element
    {
        int[,] neww = new int[3, 3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (mat[i, j] > elem)
                    neww[i, j] = 1;
                else
                    neww[i, j] = 0;
            }
        }

        return neww;
    }

    private int SumMat(int[,] mat, int lin, int col) //Add all numbers from one matrix
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

    private double Pdist(int[] hist1, int[] hist2) //Calculate the distance of 2 vectors
    {
        double sum = 0;

        for(int i=0; i<hist1.Length; i++)
        {
            sum += Mathf.Sqrt(Mathf.Pow(hist1[i] - hist2[i], 2)); //Euclidian Distance
        }

        sum /= hist1.Length;

        return sum;
    }

    //---------------- LBP METHODS --------------------

    private int[] ExtractLBPFeatures(Texture2D pic) //Extract a vector of features from a image
    {
        //PHASE 1 -> Conversion to Integers
        Color[] uni = pic.GetPixels();
        int[,] mat = new int[pic.height, pic.width];
        int count = 0;

        for (int i = 0; i < pic.height; i++)
        {
            for (int j = 0; j < pic.width; j++)
            {
                mat[i, j] = Mathf.RoundToInt(uni[count++].r * 255);
            }
        }

        ////////////////////////////////////////////
        //PHASE 2 -> Matrix Manipulation
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
            }
        }

        //pic_new => lbp transformed image

        ////////////////////////////////////////////
        //PHASE 3 -> Features Generation
        int[] features = new int[256];
        Array.Clear(features, 0, features.Length);

        for (int i = 0; i < pic.height; i++)
        {
            for (int j = 0; j < pic.width; j++)
            {
                features[pic_new[i,j]] += 1;
            }
        }

        return features;
    }


    //---------------- PUBLIC METHODS --------------------

    public double CompareImages(Texture2D img1, Texture2D img2) //Compare two images using LBP
    {
        Texture2D gray1, gray2;
        int[] features1, features2;
        double resp;

        gray1 = ConvertToGrayscale(CutImage(img1));
        features1 = ExtractLBPFeatures(gray1);

        gray2 = ConvertToGrayscale(CutImage(img2));
        features2 = ExtractLBPFeatures(gray2);

        resp = Pdist(features1, features2);

        return resp;
    }
}
