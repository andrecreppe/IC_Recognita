using System;
using UnityEngine;
using UnityEngine.UI;
#if PLATFORM_ANDROID
    using UnityEngine.Android;
#endif

public class Lang_Comp : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    public Text dataTxt, resultsTxt, errorTxt, reloadTxt;

    //---------------- PRIVATE METHODS -------------------

    private void Start()
    {
        //Setup
        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey);

        UpdateResults();
    }

    /* Update the results 'introduction' panel language */
    private void UpdateResults()
    {
        string txt = "error";

        switch (count)
        {
            case 1: //Portuguese
                txt = "RESULTADOS:\n\nAs pessoas são";
                break;
            case 2: //English
                txt = "RESULTS:\n\nThe persons are";
                break;
            case 3: //Deutsch
                txt = "ERGENBNISSE:\n\nDie Personen sind";
                break;
        }

        resultsTxt.text = txt;
    }

    /* Update the language at 'result' at result panel */
    private string ResultLang(bool equal)
    {
        if (equal)
        {
            switch (count)
            {
                case 1: //Portuguese
                    return "iguais!";
                case 2: //English
                    return "the same!";
                case 3: //Deutsch
                    return "identisch!";
            }
        }
        else
        {
            switch (count)
            {
                case 1: //Portuguese
                    return "diferentes!";
                case 2: //English
                    return "different!";
                case 3: //Deutsch
                    return "verschieden!";
            }
        }

        return "ERROR!";
    }

    /* Update the language at 'percentage' in the panel */
    private string PercentageLang(bool equal)
    {
        if (equal)
        {
            switch (count)
            {
                case 1: //Portuguese
                    return "smiliar!";
                case 2: //English
                    return "similar!";
                case 3: //Deutsch
                    return "ähnlich!";
            }
        }
        else
        {
            switch (count)
            {
                case 1: //Portuguese
                    return "diferentes!";
                case 2: //English
                    return "different!";
                case 3: //Deutsch
                    return "verschieden!";
            }
        }

        return "ERROR!";
    }

    //---------------- PUBLIC METHODS --------------------

    /* Format the result panel */
    public void Result(double percent, bool equal)
    {
        Color32 cor;
        string txt;

        //Define the color
        if (equal)
            cor = new Color32(55, 145, 45, 255);
        else
            cor = new Color32(145, 80, 45, 255);

        //Get the result text
        txt = ResultLang(equal);

        //Format the percentage
        txt += "\n\n(Pdist = " + Math.Round(percent, 5) + ")";

        //Set the result
        dataTxt.color = cor;
        dataTxt.text = txt;

    }

    /* Set the language of the error */
    public void CameraError(int op)
    {
        string msg = "", msg2 = "";

        if (op == 1) //Not available
        {
            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "ERRO!\n\nNenhuma\ncâmera\ndetectada!\n:(";
                    msg2 = "Recarregar";
                    break;
                case 2: //En
                    msg = "ERROR!\n\nNo camera\ndetected!\n:(";
                    msg2 = "Reload";
                    break;
                case 3: //De
                    msg = "FEHLER!\n\nKeine kamera\nerkannt!\n:(";
                    msg2 = "Aufladen";
                    break;
            }
        }
        else if (op == 2) //Not found
        {
            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "ERRO!\n\nCâmera não\ndisponível\n:(";
                    msg2 = "Reccaregar";
                    break;
                case 2: //En
                    msg = "ERROR!\n\nCamera not\navailable\n:(";
                    msg2 = "Reload";
                    break;
                case 3: //De
                    msg = "FEHLER!\n\nKamera nicht\nerhältlich\n:(";
                    msg2 = "Aufladen";
                    break;
            }
        }

        //Camera denied
        #if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            {
                switch (PlayerPrefs.GetInt(langKey))
                {
                    case 1: //Pt
                        msg = "ERRO!\n\nCâmera não\nautorizada\n:(";
                        msg2 = "Reccaregar";
                        break;
                    case 2: //En
                        msg = "ERROR!\n\nCamera not\nauthorized\n:(";
                        msg2 = "Reload";
                        break;
                    case 3: //De
                        msg = "FEHLER!\n\nKamera nicht\nautorisiert\n:(";
                        msg2 = "Aufladen";
                        break;
                }
            }
        #endif

        errorTxt.text = msg;
        reloadTxt.text = msg2;
    }
}
