using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture_Auth : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private Texture2D image;
    public Text snapText;

    private CameraController camcon;
    private ImageProcessing imgprocess;

    //---------------- PRIVATE METHODS ----------------

    private void Awake()
    {
        camcon = FindObjectOfType<CameraController>();
        imgprocess = FindObjectOfType<ImageProcessing>();
    }

    //---------------- PUBLIC METHODS -----------------

    /* Record one frame */
    /* After beeing taken, send to analysis */
    public void DoSnap()
    {
        image = camcon.GetCamImage();

        Descriptors desc = FindObjectOfType<Descriptors>();
        int[] teste = desc.ExtractLBPFeatures(image);



        Debug.Log("teste.length = " + teste[1]);
        //imgprocess.Recognition(pic1, pic2);

        image = null;
    }

    public void Teste() //FAZER ISSO PRA SALVAR
    {
        int count = 0;
        string featuresKey = "features";

        for(int i=0; i<256; i++)
        {
            PlayerPrefs.SetInt(featuresKey, count);
        }
    }
}
