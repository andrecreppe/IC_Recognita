using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture_Auth : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private Texture2D pic1, pic2, pic3;
    public Text snapText;

    private AppController appcontrl;
    private CameraController camcon;
    private ImgProcessing_Auth imgprocauth;
    private Lang_AuthReg langauthreg;

    //---------------- PRIVATE METHODS ----------------

    private void Awake()
    {
        appcontrl = FindObjectOfType<AppController>();
        camcon = FindObjectOfType<CameraController>();
        imgprocauth = FindObjectOfType<ImgProcessing_Auth>();
        langauthreg = FindObjectOfType<Lang_AuthReg>();
    }

    private void Start()
    {
        count = 0;
    }

    //---------------- PUBLIC METHODS -----------------

    /* Record one frame */
    /* After beeing taken, send to analysis */
    public void DoSnapRecord()
    {
        switch (count)
        {
            case 0:
                pic1 = camcon.GetCamImage();

                break;
            case 1:
                pic2 = camcon.GetCamImage();

                break;
            case 2:
                pic3 = camcon.GetCamImage();

                break;
            default:
                imgprocauth.SavePerson(pic1, pic2, pic3);

                pic1 = null;
                pic2 = null;
                pic3 = null;

                appcontrl.LoadAuthenticator();

                break;
        }

        langauthreg.UpdateSnapText(count);
        count++;
    }

    public void DoSnapAuthenticator()
    {
        pic1 = camcon.GetCamImage();

        imgprocauth.TryToUnlock(pic1);

        pic1 = null;
    }
}
