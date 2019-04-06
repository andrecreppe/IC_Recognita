using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
    //---------------- VARIABLES -----------------

    private string langKey;

    //---------------- PRIVATE METHODS -----------------

    public void Start()
    {
        langKey = "lang";

        if (!PlayerPrefs.HasKey(langKey)) //First time?
            PlayerPrefs.SetInt(langKey, 1); //Default = português
        PlayerPrefs.Save();
    }

    //---------------- PUBLIC METHODS -----------------

    public void LoadInfo()
    {
        SceneManager.LoadScene("Info");
    }

    public void LoadCamera()
    {
        SceneManager.LoadScene("Camera");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
