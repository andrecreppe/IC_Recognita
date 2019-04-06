using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private string langKey;
    private int count;
    private bool camAvailable;
    private WebCamTexture backCamera, frontCamera, activeCamera;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;
    public Text errorMsg;
    public Button snapButton;

    //---------------- PRIVATE METHODS --------------------

    private void Start()
    {
        langKey = "lang";
        count = 0;

        IsFirstTime();

        //Setup
        defaultBackground = background.texture;

        WebCamDevice[] devices = WebCamTexture.devices;

        errorMsg.text = "";

        //Camera Detection
        if(devices.Length == 0)
        {
            CameraError(1);
            
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
            activeCamera = frontCamera;
        else
            activeCamera = backCamera;


        //Set up the camera
        TestCamera();
    }

    private void Update()
    {
        //Camera exists?
        if(!camAvailable)
            return;

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

    private void TestCamera()
    {
        if(activeCamera == null)
        {
            background.texture = defaultBackground;

            camAvailable = false;

            CameraError(2);

            snapButton.gameObject.SetActive(false);
        }
        else
        {
            activeCamera.Play();

            background.texture = activeCamera;

            camAvailable = true;

            CameraError(0);

            snapButton.gameObject.SetActive(true);
        }
    }

    private void CameraError(int op)
    {
        string msg = "";

        if (op == 1) //Not available
        {
            switch(PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "ERRO!\n\nNenhuma\ncâmera\ndetectada!\n:(";
                    break;
                case 2: //En
                    msg = "ERROR!\n\nNo camera\ndetected!\n:(";
                    break;
                case 3: //De
                    msg = "FEHLER!\n\nKeine kamera\nerkannt!\n:(";
                    break;
            }
        }
        else if(op == 2) //Not found
        {
            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "ERRO!\n\nCâmera não\ndisponível\n:(";
                    break;
                case 2: //En
                    msg = "ERROR!\n\nCamera not\navailable\n:(";
                    break;
                case 3: //De
                    msg = "FEHLER!\n\nKamera nicht\nerhältlich\n:(";
                    break;
            }
        }

        errorMsg.text = msg;
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

    private void SaveToDevice(Texture2D snap)
    {
        File.WriteAllBytes(
            Application.dataPath //Location
            + "/capture" + ++count + ".png", //Image Name
            snap.EncodeToPNG() //File type
        );

        Debug.Log("Image saved at: " + Application.dataPath);
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
            TestCamera();
        }
        //Backcamera -> Frontcamera
        else
        {
            activeCamera = frontCamera;
            TestCamera();
        }
    }

    public Texture2D GetCamImage()
    {
        activeCamera.Pause();

        Texture2D snap = new Texture2D(activeCamera.width, activeCamera.height);
        snap.SetPixels(activeCamera.GetPixels());
        snap.Apply();

        activeCamera.Play();

        //SaveToDevice(snap);

        return snap;
    }

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
}
