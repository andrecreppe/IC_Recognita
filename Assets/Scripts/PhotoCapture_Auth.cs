using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture_Auth : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private Texture2D pic1, pic2, pic3;

    public Text snapText;
    public Image confirmationDialog;

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
                if (PlayerPrefs.HasKey("features0"))
                    confirmationDialog.gameObject.SetActive(true);
                else
                    Overwrite();

                break;
        }

        langauthreg.UpdateSnapText(count);
        count++;
    }

    public void Overwrite()
    {
        imgprocauth.SavePerson(pic1, pic2, pic3);

        appcontrl.LoadAuthenticator();
    }

    public void CancelOverwrite()
    {
        appcontrl.LoadAuthRegister(); //is eficient?
    }

    public void DoSnapAuthenticator()
    {
        pic1 = camcon.GetCamImage();

        imgprocauth.TryToUnlock(pic1);

        pic1 = null;
    }
}
