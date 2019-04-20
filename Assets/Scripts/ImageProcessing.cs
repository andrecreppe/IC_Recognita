using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageProcessing : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int descriptor_in_use;
    private Texture2D gray1, gray2;

    public float[] tresholds; //Thresholds -> Determined by the matlab code
    public RawImage block, block2;

    //---------------- PRIVATE METHODS --------------------

    private void Start()
    {
        //Setup
        descriptor_in_use = 1; //Default descriptor -> LBP
    }

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
        int x = Mathf.RoundToInt((float) calc);
        calc = (raw.height / 2) - (raw.height * h / 2);
        int y = Mathf.RoundToInt((float) calc);

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

    private float Comparison(int[,] pic1, int[,] pic2, int lin, int col)
    {
        int dist;
        int[,] final = new int[lin, col];

        for(int i=0; i<lin; i++)
        {
            for(int j=0; j<col; j++)
            {
                final[i,j] = Mathf.Abs(pic2[i, j] - pic1[i, j]);
            }
        }

        dist = SumMat(final, lin, col);
        //treshold comparison

        dist = Mathf.RoundToInt(dist / (lin * col));

        return dist;
    }

    private void ShowResults(float res)
    {
        //menu? popup?
    }

    //---------------- IMAGE DESCRIPTORS --------------------
    //Novo arquivo!

    private int[,] MatMultiplyer3x3(int[,] mat1, int[,] mat2)
    {
        int[,] c = new int[3,3];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    c[i, j] = mat1[i, k] * mat2[k, j];
                }
            }
        }

        return c;
    }

    private int[,] CompareToElement3x3(int[,] mat, int elem)
    {
        int[,] neww = new int[3,3];

        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                if (mat[i, j] > elem)
                    neww[i, j] = 1;
                else
                    neww[i, j] = 0;
            }
        }

        return neww;
    }

    private int SumMat(int[,] mat, int lin, int col)
    {
        int sum = 0;
        for(int i=0; i<lin; i++)
        {
            for(int j=0; j<col; j++)
            {
                sum += mat[i, j];
            }
        }

        return sum;
    }

    private int[,] ExtractLBPFeatures(Texture2D pic)
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
                mini_area[0,0] = mat[i - 1, j - 1];
                mini_area[0,1] = mat[i - 1, j];
                mini_area[0,2] = mat[i - 1, j + 1];
                mini_area[1,0] = mat[i, j - 1];
                mini_area[1,1] = mat[i, j];
                mini_area[1,2] = mat[i, j + 1];
                mini_area[2,0] = mat[i + 1, j - 1];
                mini_area[2,1] = mat[i + 1, j];
                mini_area[2,2] = mat[i + 1, j + 1];

                pic_new[i, j] = SumMat(MatMultiplyer3x3(
                    CompareToElement3x3(
                        mini_area, mini_area[1, 1]
                    ), pesos
                ), 3, 3
                );
            }
        }

        return pic_new;
    }


    //---------------- PUBLIC METHODS --------------------

    //public float Recognition(Texture2D img1, Texture2D img2
    public void Recognition(Texture2D texture1, Texture2D texture2)
    {
        Descriptors dec = FindObjectOfType<Descriptors>();

        Debug.Log("Before: " + Time.realtimeSinceStartup);

        gray1 = ConvertToGrayscale(texture1);
        gray2 = ConvertToGrayscale(texture2);

        gray1 = CutImage(gray1);
        gray2 = CutImage(gray2);

        block.texture = gray1;
        block2.texture = gray2;

        float resp = Comparison(
            ExtractLBPFeatures(gray1), ExtractLBPFeatures(gray2),
            gray1.height, gray1.width
        );

        Debug.Log("resp = " + resp);
        Debug.Log("After: " + Time.realtimeSinceStartup);

        //ShowResults(resp);
    }
}
