using UnityEngine;
using UnityEngine.SceneManagement;
#if PLATFORM_ANDROID
    using UnityEngine.Android;
#endif

public class AppController : MonoBehaviour
{
    //---------------- PRIVATE METHODS -----------------

    private void Awake()
    {
        //Detect system language -> first log
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
        }
    }

    private void Start()
    {
        //Allow camera for android devices
        #if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
                Permission.RequestUserPermission(Permission.Camera);
        #endif
    }

    //-------------- PUBLIC METHODS (Scene Loaders) ---------------

    //General loaders
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadConfigs()
    {
        SceneManager.LoadScene("Configs");
    }

    public void LoadAbout()
    {
        SceneManager.LoadScene("About");
    }

    //Authenticator loaders
    public void LoadAuthenticator()
    {
        SceneManager.LoadScene("Authenticator");
    }

    public void LoadAuthRegister()
    {
        SceneManager.LoadScene("AuthRegister");
    }

    public void LoadAuthTutorial()
    {
        SceneManager.LoadScene("AuthTutorial");
    }

    //Comparator Loaders
    public void LoadComparison()
    {
        SceneManager.LoadScene("Comparison");
    }

    public void LoadCompTutorial()
    {
        SceneManager.LoadScene("CompTutorial");
    }
}