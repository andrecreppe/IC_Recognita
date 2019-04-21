using System;
using System.Collections;
using System.Collections.Generic;
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

        //Set the Result Display active
        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();


        if (resp <= treshold) //Equal images
        {
            //Color green
            //HEX = 37912D
        }
        else //Different Images
        {
            //Color red
            //HEX = 91502D
        }

        //Change the text
        //sent to camera languae -> ok or not ok

        //Calculate the percentage
        //result.text += "\n(" + perc  + "%" + getsimilar() + ")";
    }

    private void SaveResult()
    {
        //See below
        if(saveData)
        {

        }
    }

    //---------------- PUBLIC METHODS --------------------

    public void Recognition(Texture2D img1, Texture2D img2)
    {
        Descriptors dec = FindObjectOfType<Descriptors>();
        double resp; 
        
        resp = dec.CompareImages(img1, img2);

        ShowResults(resp, dec.LBP_treshold);
    }

    public void CloseResults()
    {
        CameraController camcon = FindObjectOfType<CameraController>();

        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();
    }
}

/*
public void SaveGame()
{
    // 1
    Save save = CreateSaveGameObject();

    // 2
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
    bf.Serialize(file, save);
    file.Close();

    // 3
    hits = 0;
    shots = 0;
    shotsText.text = "Shots: " + shots;
    hitsText.text = "Hits: " + hits;

    ClearRobots();
    ClearBullets();
    Debug.Log("Game Saved");
}      

public void LoadGame()
{ 
    // 1
    if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
    {
    ClearBullets();
    ClearRobots();
    RefreshRobots();

    // 2
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
    Save save = (Save)bf.Deserialize(file);
    file.Close();

    // 3
    for (int i = 0; i < save.livingTargetPositions.Count; i++)
    {
     int position = save.livingTargetPositions[i];
     Target target = targets[position].GetComponent<Target>();
     target.ActivateRobot((RobotTypes)save.livingTargetsTypes[i]);
     target.GetComponent<Target>().ResetDeathTimer();
    }

    // 4
    shotsText.text = "Shots: " + save.shots;
    hitsText.text = "Hits: " + save.hits;
    shots = save.shots;
    hits = save.hits;

    Debug.Log("Game Loaded");

    Unpause();
    }
    else
    {
    Debug.Log("No game saved!");
    }
}      
*/
