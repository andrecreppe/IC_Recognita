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

    private void ShowResults(double resp, double treshold)
    {
        CameraController camcon = FindObjectOfType<CameraController>();
        CameraLanguage camlang = FindObjectOfType<CameraLanguage>();
        double perc;
        bool thesame;

        //Set the Result Display active
        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();

        thesame = resp <= treshold;

        if (thesame) //Equal images
        {
            perc = (2100 / resp) + 9;
            camlang.Result(Math.Round(perc, 2), true);
        }
        else //Different Images
        {
            resp -= treshold;
            perc = resp * 100 / (115-treshold);
            camlang.Result(Math.Round(perc, 2), false);
        }
        
        SaveResult(resp, perc, thesame);
    }

    private void SaveResult(double resp, double perc, bool equal) //Save the results in a log
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

    public void Recognition(Texture2D img1, Texture2D img2) //Use LBP to find the similarity between two images
    {
        Descriptors dec = FindObjectOfType<Descriptors>();
        double resp; 
        
        resp = dec.CompareImages(img1, img2);

        ShowResults(resp, dec.LBP_treshold);
    }

    public void CloseResults() //Close the results tab
    {
        CameraController camcon = FindObjectOfType<CameraController>();

        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();
    }
}