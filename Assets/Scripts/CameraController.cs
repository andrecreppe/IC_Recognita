using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private bool camAvailable, isBackCamera;
    private WebCamTexture backCamera, frontCamera, activeCamera;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;
    public Text errorMsg;
    public Button snapButton, reloadButton;

    private Lang_Comp camlang;

    //---------------- PRIVATE METHODS --------------------

    private void Awake()
    {
        camlang = FindObjectOfType<Lang_Comp>();

        camlang.CameraError(2);
        reloadButton.gameObject.SetActive(true);
        reloadButton.transform.SetAsLastSibling();
    }

    private void Start()
    {
        //Setup Enviroment
        defaultBackground = background.texture;

        WebCamDevice[] devices = WebCamTexture.devices;

        //No camera detection
        if(devices.Length == 0)
        {
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

    //Test if camera is working (start error or not)
    private void TestCamera()
    {
        if(activeCamera == null)
        {
            background.texture = defaultBackground;

            camAvailable = false;

            camlang.CameraError(2);

            snapButton.gameObject.SetActive(false);
            reloadButton.gameObject.SetActive(true);
        }
        else
        {
            activeCamera.Play();

            background.texture = activeCamera;

            camAvailable = true;

            camlang.CameraError(0);

            snapButton.gameObject.SetActive(true);
            reloadButton.gameObject.SetActive(false);
        }
    }

    //---------------- PUBLIC METHODS --------------------

    /* Get the camAvailable state */
    public bool GetCamAvailable()
    {
        return this.camAvailable;
    }

    /* Get what camera is active now */
    public int GetWichCamera() 
    {
        int ret = 0;

        if(camAvailable) //Has working cameras
        {
            if (activeCamera == backCamera)
                ret = 1; //back camera
            else
                ret = 2; //front camera
        }

        return ret;
    }

    /* Swap Cameras */
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

    /* Save the current image frame */
    public Texture2D GetCamImage()
    {
        activeCamera.Pause();

        //Invert if is backcamera
        Texture2D snap = new Texture2D(activeCamera.width, activeCamera.height);
        snap.SetPixels(activeCamera.GetPixels());
        snap.Apply();

        if (isBackCamera) //Image Upside down (backCamera)
        {
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

    /* Invert camera status (pause or running) */
    public void ChangeCameraStatus() 
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

    /* Reload the initial setup */
    public void ReloadCameras()
    {
        Start();
    }
}
