using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageProcessing : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int descriptor_in_use;
    private Texture2D gray1, gray2;

    public float[] tresholds; //Thresholds -> Determined by the matlab code
    public RawImage block;

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

    private float Comparison()
    {
        return 0.0f; //in percentage
    }

    private void ShowResults(float res)
    {
        //menu? popup?
    }

    //---------------- IMAGE DESCRIPTORS --------------------
    //Novo arquivo!

    private void LBP(/*????*/)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {

            }
        }
    }



    //---------------- PUBLIC METHODS --------------------

    //public float Recognition(Texture2D img1, Texture2D img2
    public void Recognition(Texture2D texture1, Texture2D texture2)
    {
        gray1 = ConvertToGrayscale(texture1);
        gray2 = ConvertToGrayscale(texture2);

        gray1 = CutImage(gray1);
        gray2 = CutImage(gray2);

        block.texture = gray1;

        //------------------------------------------

        //LBP(); //Use different methods also

        //float resp = Comparison();

        //ShowResults(resp);
    }
}
