using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private bool pic1_show, pic2_show;
    private Texture2D pic1, pic2;
    private CameraController camcon;
    private ImageProcessing imgprocess;

    public GameObject but1, but2;
    public Button snapButton, returnButton, trashButton;
    public Text snapText;
    public RawImage preview;    

    //---------------- PRIVATE METHODS ----------------

    private void Start()
    {
        //Setup Enviroment
        but1.SetActive(false); pic1_show = false;
        but2.SetActive(false); pic2_show = false;

        returnButton.gameObject.SetActive(false);
        returnButton.transform.SetAsLastSibling(); //rendered last - apear in top

        trashButton.gameObject.SetActive(false);
        trashButton.transform.SetAsLastSibling();

        //Fix mobile rotation (mac is "normal")

        preview.gameObject.SetActive(false);
        preview.transform.localScale = new Vector3( //Unflip
            -preview.transform.localScale.x,
            preview.transform.localScale.y,
            preview.transform.localScale.z
        );
        //Rotation wrong
        Quaternion rot = Quaternion.Euler(0, 0, -90);
        preview.gameObject.transform.rotation = rot;
    }

    private void UpdateText()
    {
        //Change the button status according to quantity of taken images
        ColorBlock colors = snapButton.colors;

        if (count == 2)
        {
            snapText.text = "Go! (" + count + "/2)";

            colors.normalColor = new Color32(73, 255, 82, 255);
            colors.highlightedColor = new Color32(73, 255, 82, 255);
            colors.pressedColor = new Color32(132, 255, 138, 255);

            snapButton.colors = colors;
        }
        else
        {
            snapText.text = "Snap! (" + count + "/2)";

            colors.normalColor = new Color32(252, 73, 77, 255);
            colors.highlightedColor = new Color32(252, 73, 77, 255);
            colors.pressedColor = new Color32(255, 129, 132, 255);

            snapButton.colors = colors;
        }
    }

    private void TogglePreview(Texture2D tx)
    {
        //Invert the status of the preview
        //Show the image recorded

        preview.texture = tx;
        preview.gameObject.SetActive(!preview.gameObject.activeSelf);

        returnButton.gameObject.SetActive(!returnButton.gameObject.activeSelf);
        trashButton.gameObject.SetActive(!trashButton.gameObject.activeSelf);
    }

    //---------------- PUBLIC METHODS -----------------

    public void DoSnap()
    {
        //Save Image or send them to analysis

        camcon = FindObjectOfType<CameraController>();
        imgprocess = FindObjectOfType<ImageProcessing>();

        if (count == 0) //Picture 1
        {
            pic1 = camcon.GetCamImage();

            count++;

            UpdateText();
            but1.SetActive(true);
        }
        else if (count == 1 && pic1 == null) //Picture 2
        {
            pic1 = camcon.GetCamImage();
            count++;

            UpdateText();
            but1.SetActive(true);
        }
        else if(count == 1 && pic1 != null)
        {
            pic2 = camcon.GetCamImage();
            count++;

            UpdateText();
            but2.SetActive(true);
        }
        else
        {
            imgprocess.Recognition(pic1, pic2);
        }
    }

    public void RevealPic1()
    {
        camcon = FindObjectOfType<CameraController>();
        camcon.ChangeCameraStatus(); //Stop camera processing

        //Preview = ON
        TogglePreview(pic1);
        pic1_show = true;
    }

    public void RevealPic2()
    {
        camcon = FindObjectOfType<CameraController>();
        camcon.ChangeCameraStatus(); //Stop camera processing

        //Preview = ON
        TogglePreview(pic2);
        pic2_show = true;
    }

    public void RemoveSelectedPicture()
    {
        count--;
        UpdateText();

        if (pic1_show)
        {
            pic1 = null;
            pic1_show = false;

            but1.gameObject.SetActive(false);
        }
        else
        {
            pic2 = null;
            pic2_show = false;

            but2.gameObject.SetActive(false);
        }

        ClosePreview();
    }

    public void ClosePreview()
    {
        camcon = FindObjectOfType<CameraController>();
        camcon.ChangeCameraStatus(); //Start again the camera

        //Preview = OFF
        TogglePreview(null);

        if (pic1_show)
            pic1_show = false;
        else
            pic2_show = false;
    }
}
