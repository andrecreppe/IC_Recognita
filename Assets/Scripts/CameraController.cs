using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private bool camAvailable, isBackCamera;
    private WebCamTexture backCamera, frontCamera, activeCamera;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;
    public Text errorMsg;
    public Button snapButton;

    //---------------- PRIVATE METHODS --------------------

    private void Start()
    {
        IsFirstTime();
        IsFirstTime();

        //Setup Enviroment
        defaultBackground = background.texture;

        WebCamDevice[] devices = WebCamTexture.devices;

        errorMsg.text = "";

        //Camera Detection
        if(devices.Length == 0)
        {
            CameraLanguage camlang = FindObjectOfType<CameraLanguage>();

            camlang.CameraError(1);
            
            camAvailable = false;
            return;
        }

        //Camera listing
        for(int i=0; i<devices.Length; i++)
        {
            //Frontal Camera
            if (devices[i].isFrontFacing)
            {
                frontCamera = new WebCamTexture(devices[i].name,
                    Screen.width, Screen.height);

            }
            //Back camera
            else
            {
                backCamera = new WebCamTexture(devices[i].name,
                    Screen.width, Screen.height);
            }
        }

        //Activation priority => frontcamera
        //Check if have backcamera
        if(frontCamera != null)
        {
            activeCamera = frontCamera;
            isBackCamera = false; //Called in the 'Save' script
        }
        else
        {
            activeCamera = backCamera;
            isBackCamera = true;
        }

        //Set up the camera
        TestCamera();
    }

    private void Update()
    {
        //Camera exists?
        if(!camAvailable)
            return;

        //Adjust camera for screen
        float ratio = (float)activeCamera.width / (float)activeCamera.height;
        fit.aspectRatio = ratio;

        float scaleY = activeCamera.videoVerticallyMirrored ? -1f : 1f;
        if(activeCamera == frontCamera)
            background.rectTransform.localScale = new Vector3(-1f, scaleY, 1f);
        else
            background.rectTransform.localScale = new Vector3(-1f, -scaleY, 1f);

        int orient = activeCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    private void TestCamera() //Test if camera is working (start error or not)
    {
        CameraLanguage camlang = FindObjectOfType<CameraLanguage>();

        if(activeCamera == null)
        {
            background.texture = defaultBackground;

            camAvailable = false;

            camlang.CameraError(2);

            snapButton.gameObject.SetActive(false);
        }
        else
        {
            activeCamera.Play();

            background.texture = activeCamera;

            camAvailable = true;

            camlang.CameraError(0);

            snapButton.gameObject.SetActive(true);
        }
    }

    private void IsFirstTime()
    {
        /*
         * This method is for after allowing access to the app
         * it restarts and initiate with all cameras loaded;
        */

        if(!PlayerPrefs.HasKey("first")) //is the first time
        {
            PlayerPrefs.SetInt("first", 1);
            PlayerPrefs.Save();

            //Reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //---------------- PUBLIC METHODS --------------------

    public bool GetCamAvailable()
    {
        return this.camAvailable;
    }

    public void ChangeCameras()
    {
        //Frontcamera -> Backcamera
        if (activeCamera == frontCamera) 
        {
            activeCamera = backCamera;
            isBackCamera = true;
            TestCamera();
        }
        //Backcamera -> Frontcamera
        else
        {
            activeCamera = frontCamera;
            isBackCamera = false;
            TestCamera();
        }
    }

    public Texture2D GetCamImage() //Save the current image frame
    {
        activeCamera.Pause();

        //Invert if is backcamera
        Texture2D snap = new Texture2D(activeCamera.width, activeCamera.height);
        snap.SetPixels(activeCamera.GetPixels());
        snap.Apply();

        //TEST IF THIS IS CORRECT!!!!

        if(isBackCamera) //Image Upside down (backCamera)
        {
            //Inverted because the image is inverted
            int yN = snap.width;
            int xN = snap.height;

            Texture2D fliped = new Texture2D(snap.width, snap.height);

            for (int i = 0; i < xN; i++)
            {
                for (int j = 0; j < yN; j++)
                {
                    fliped.SetPixel(j, xN - i - 1, snap.GetPixel(j, i));
                }
            }
            fliped.Apply();

            snap = fliped;
        }

        activeCamera.Play();

        return snap;
    }

    public void ChangeCameraStatus() //Invert camera status (pause or running)
    {
        if(camAvailable)
        {
            activeCamera.Pause();
            camAvailable = false;
        }
        else
        {
            activeCamera.Play();
            camAvailable = true;
        }
    }
}
