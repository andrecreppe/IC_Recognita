using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class ImageProcessing : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    public bool saveData;
    public Image result_menu;

    //---------------- PRIVATE METHODS -------------------

    /* Display the result panel with the comparison result */
    private void ShowResults(double resp, double treshold)
    {
        CameraController camcon = FindObjectOfType<CameraController>();
        CameraLanguage camlang = FindObjectOfType<CameraLanguage>();
        bool thesame;

        //Set the Result Display active
        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();

        thesame = resp <= treshold;

        if (thesame) //Equal images
            camlang.Result(resp, true);
        else //Different Images
            camlang.Result(resp, false);
        
        //SaveResult(resp, perc, thesame);
    }

    /* Save the results as a log */
    private void SaveResult(double resp, double perc, bool equal)
    {
        if(saveData)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs;
            string log, path;

            path = Application.persistentDataPath + "/results.log";
            log = "";

            //Check if a file exists
            if (File.Exists(path)) 
            {
                //Load file
                fs = File.Open(path, FileMode.Open);
                log = (string)bf.Deserialize(fs);
                fs.Close();
            }

            //Build the new log structure
            log += "Date: " + DateTime.Now;
            log += "\nPdist: " + resp;
            log += "\nEquality: " + equal;
            log += "\nPercentage: " + perc;
            log += "\n----------------------------\n";

            //Create the results
            fs = File.Create(path);
            bf.Serialize(fs, log);
            fs.Close();
        }
    }

    //---------------- PUBLIC METHODS --------------------

    /* Manage the recognition calling -> Execute the comparison order */
    public void Recognition(Texture2D img1, Texture2D img2)
    {
        Descriptors dec = FindObjectOfType<Descriptors>();
        PhotoCapture photcap = FindObjectOfType<PhotoCapture>();
        double resp; 
        
        resp = dec.CompareImages(img1, img2);

        photcap.DeletePictures();

        ShowResults(resp, dec.LBP_treshold);
    }

    /* Close the results tab */
    public void CloseResults()
    {
        CameraController camcon = FindObjectOfType<CameraController>();

        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();
    }
}