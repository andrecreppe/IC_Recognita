using UnityEngine;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour
{
    //---------------- PRIVATE METHODS -----------------

    private void Awake()
    {
        //Detect system language -> first time
        string langKey = "lang";

        if (PlayerPrefs.GetInt(langKey) > 3 || PlayerPrefs.GetInt(langKey) < 1)
        {
            if (Application.systemLanguage == SystemLanguage.Portuguese)
                PlayerPrefs.SetInt(langKey, 1);
            else if (Application.systemLanguage == SystemLanguage.German)
                PlayerPrefs.SetInt(langKey, 3);
            else
                PlayerPrefs.SetInt(langKey, 2); //Default = english

            PlayerPrefs.Save();

            Debug.Log("default system lang = " + PlayerPrefs.GetInt(langKey));
        }

        //Is the first time -> load tutorial
        string begginKey = "first";

        if (PlayerPrefs.GetInt(begginKey) != 1)
        {
            //Set flag to complete
            PlayerPrefs.SetInt(begginKey, 1);
            PlayerPrefs.Save();

            //Reload scene
            SceneManager.LoadScene("Tutorial");
        }
    }

    //-------------- PUBLIC METHODS (Scene Loaders) ---------------

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
