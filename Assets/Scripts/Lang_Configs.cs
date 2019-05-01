using UnityEngine;
using UnityEngine.UI;

public class Lang_Configs : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    public Text configs, lang, langName;
    public Button buttonFlag;
    public Sprite[] flags;

    //------------------ PRIVATE METHODS --------------------

    private void Start()
    {
        //Setup
        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey) - 1;

        UpdateLanguage();
    }

    //------------------ PUBLIC METHODS --------------------

    /* Change the language according to flag order */
    public void UpdateLanguage()
    {
        count++;
        if (count > 3)
            count = 1;

        PlayerPrefs.SetInt(langKey, count);
        PlayerPrefs.Save();

        switch (count)
        {
            //Português
            case 1:
                configs.text = "Configurações";

                lang.text = "Idioma:";
                langName.text = "Português";

                break;

            //English
            case 2:
                configs.text = "Settings";

                lang.text = "Language:";
                langName.text = "English";

                break;

            //Deutsch
            case 3:
                configs.text = "Einstellungen";

                lang.text = "Sprache:";
                langName.text = "Deutsch";

                break;
        }

        //Change the flag icon
        buttonFlag.image.sprite = flags[count - 1];
    }
}
