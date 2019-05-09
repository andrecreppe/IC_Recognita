using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture_Comp : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private bool pic1_show, pic2_show, pic1_isBack, pic2_isBack;
    private Texture2D pic1, pic2;

    public GameObject but1, but2;
    public Button snapButton, returnButton, trashButton;
    public Text snapText;
    public RawImage preview;

    private CameraController camcon;
    private ImageProcessing imgprocess;

    //---------------- PRIVATE METHODS ----------------

    private void Awake()
    {
        camcon = FindObjectOfType<CameraController>();
        imgprocess = FindObjectOfType<ImageProcessing>();
    }

    private void Start()
    {
        //Setup Enviroment
        but1.SetActive(false); pic1_show = false;
        but2.SetActive(false); pic2_show = false;

        //Render some objects last - to appear on top
        returnButton.gameObject.SetActive(false);
            returnButton.transform.SetAsLastSibling(); 
        trashButton.gameObject.SetActive(false);
            trashButton.transform.SetAsLastSibling();

        preview.gameObject.SetActive(false);
        preview.transform.localScale = new Vector3(
            -preview.transform.localScale.x,
            preview.transform.localScale.y,
            preview.transform.localScale.z
        );
    }

    /* Change the button text according to quantity of images taken */
    private void UpdateText()
    {
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

    /* Show the selected image */
    private void TogglePreview(Texture2D tx)
    {
        Quaternion rot;

        preview.texture = tx;
        preview.gameObject.SetActive(!preview.gameObject.activeSelf);

        returnButton.gameObject.SetActive(!returnButton.gameObject.activeSelf);
        trashButton.gameObject.SetActive(!trashButton.gameObject.activeSelf);

        //Reset Rotation
        rot = Quaternion.Euler(0, 0, 0);
        preview.gameObject.transform.rotation = rot;

        //Fix mobile rotation
        if ((tx == pic1) && pic1_isBack)
        {
            rot = Quaternion.Euler(0, 0, 180);
            preview.gameObject.transform.rotation = rot;
        }
        else if ((tx == pic2) && pic2_isBack)
        {
            rot = Quaternion.Euler(0, 0, 180);
            preview.gameObject.transform.rotation = rot;
        }
        else
        {
            rot = Quaternion.Euler(0, 0, -90);
            preview.gameObject.transform.rotation = rot;
        }
    }

    //---------------- PUBLIC METHODS -----------------

    /* Record the frame and sort-it (1 or 2) */
    /* After both beeing taken, send to analysis */
    public void DoSnap()
    {
        if (count == 0) //Not 1 - Not 2
        {
            pic1 = camcon.GetCamImage();

            if (camcon.GetWichCamera() == 1)
                pic1_isBack = true;
            else
                pic1_isBack = false;

            count++;

            UpdateText();
            but1.SetActive(true);
        }
        else if (count == 1 && pic1 == null) //Not 1 - Has 2
        {
            pic1 = camcon.GetCamImage();
            count++;

            if (camcon.GetWichCamera() == 1)
                pic1_isBack = true;
            else
                pic1_isBack = false;

            UpdateText();
            but1.SetActive(true);
        }
        else if(count == 1 && pic1 != null) //Has 1 - Not 2
        {
            pic2 = camcon.GetCamImage();
            count++;

            if (camcon.GetWichCamera() == 1)
                pic2_isBack = true;
            else
                pic2_isBack = false;

            UpdateText();
            but2.SetActive(true);
        }
        else //Has all -> Good to go!
        {
            imgprocess.Recognition(pic1, pic2);
        }
    }

    /* Show image 1 */
    public void RevealPic1()
    {
        camcon.ChangeCameraStatus(); //Stop camera processing

        TogglePreview(pic1);
        pic1_show = true;
    }

    /* Show image 2 */
    public void RevealPic2()
    {
        camcon.ChangeCameraStatus(); //Stop camera processing

        TogglePreview(pic2);
        pic2_show = true;
    }

    /* Delete the picture in the displayed preview */
    public void RemoveSelectedPicture()
    {
        count--;
        UpdateText();

        if (pic1_show)
        {
            pic1 = null;
            pic1_show = false;
            pic1_isBack = false;

            but1.gameObject.SetActive(false);
        }
        else
        {
            pic2 = null;
            pic2_show = false;
            pic2_isBack = false;

            but2.gameObject.SetActive(false);
        }

        ClosePreview();
    }

    /* Close the preview window */
    public void ClosePreview()
    {
        camcon.ChangeCameraStatus(); //Start again the camera

        //Preview = OFF
        TogglePreview(null);

        if (pic1_show)
            pic1_show = false;
        else if (pic2_show)
            pic2_show = false;
    }

    /* Clear both pictures */
    public void DeletePictures()
    {
        pic1_show = true;
        RemoveSelectedPicture();
        RemoveSelectedPicture();
    }
}
