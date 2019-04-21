using UnityEngine;
using UnityEngine.UI;

public class CameraLanguage : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    public Text dataTxt, resultsTxt, errorTxt;

    //---------------- PRIVATE METHODS -------------------

    private void Start()
    {
        //Setup
        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey);

        UpdateResults();
    }

    private void UpdateResults()
    {
        string txt = "error";

        switch(count)
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

    private string ResultLang(bool equal)
    {
        if(equal)
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

    public void Result(double percent, bool equal)
    {
        Color32 cor;
        string txt;

        //Define the color
        if(equal)
            cor = new Color32(55, 145, 45, 255);
        else
            cor = new Color32(145, 80, 45, 255);

        //Get the result text
        txt = ResultLang(equal);

        //Format the percentage
        txt += "\n\n(" + percent + "% " + PercentageLang(equal) + ")";

        //Set the result
        dataTxt.color = cor;
        dataTxt.text = txt;

    }

    public void CameraError(int op)
    {
        string msg = "";

        if (op == 1) //Not available
        {
            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "ERRO!\n\nNenhuma\ncâmera\ndetectada!\n:(";
                    break;
                case 2: //En
                    msg = "ERROR!\n\nNo camera\ndetected!\n:(";
                    break;
                case 3: //De
                    msg = "FEHLER!\n\nKeine kamera\nerkannt!\n:(";
                    break;
            }
        }
        else if (op == 2) //Not found
        {
            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "ERRO!\n\nCâmera não\ndisponível\n:(";
                    break;
                case 2: //En
                    msg = "ERROR!\n\nCamera not\navailable\n:(";
                    break;
                case 3: //De
                    msg = "FEHLER!\n\nKamera nicht\nerhältlich\n:(";
                    break;
            }
        }

        errorTxt.text = msg;
    }
}
