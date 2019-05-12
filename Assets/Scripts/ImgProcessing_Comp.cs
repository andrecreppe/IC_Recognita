using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class ImgProcessing_Comp : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    public Image result_menu;

    private CameraController camcon;
    private Lang_Comp camlang;
    private Descriptors dec;
    private PhotoCapture_Comp photcapcomp;

    //---------------- PRIVATE METHODS -------------------

    private void Awake()
    {
        camcon = FindObjectOfType<CameraController>();
        camlang = FindObjectOfType<Lang_Comp>();
        dec = FindObjectOfType<Descriptors>();
        photcapcomp = FindObjectOfType<PhotoCapture_Comp>();
    }

    /* Display the result panel with the comparison result */
    private void ShowResults(double resp, double treshold)
    {
        bool thesame;

        //Set the Result Display active
        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();

        thesame = resp <= treshold;

        camlang.Result(resp, thesame);
    }

    //---------------- PUBLIC METHODS --------------------

    /* Manage the recognition calling -> Execute the comparison order */
    public void Recognition(Texture2D img1, Texture2D img2)
    {
        double resp, tresh;

        resp = dec.CompareImages(img1, img2);
        tresh = dec.ActiveTreshold();

        photcapcomp.DeletePictures();

        ShowResults(resp, tresh);
    }

    /* Close the results tab */
    public void CloseResults()
    {
        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();
    }
}
