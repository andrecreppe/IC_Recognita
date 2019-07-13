using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android; //Android for camera verification

public class Lang_Comp : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    private Descriptors desc;

    public Text dataTxt, resultsTxt, errorTxt, reloadTxt, returnTxt;

    //---------------- PRIVATE METHODS -------------------

    private void Awake()
    {
        desc = FindObjectOfType<Descriptors>();
    }

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

    //Return the language to show below the (X) button
    private void ReturnLang()
    {
        string txt = "";

        switch(count)
        {
            case 1:
                txt = "Voltar";
                break;
            case 2:
                txt = "Return";
                break;
            case 3:
                txt = "Zurückkommen";
                break;
        }

        returnTxt.text = txt;
    }

    private string DescriptorName()
    {
        int in_use = desc.GetDescriptorInUse();

        switch(in_use)
        {
            case 1:
                return "Cityblock";
            case 2:
                return "Euclidian";
            case 3:
                return "Cossine";
        }

        return "ERROR IN NAME";
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

        //Set the descriptor info
        txt += "\n<i>[" + DescriptorName() + "]</i>";

        /*-------*/
            //BETA FUNCTION!!!!!
            if (desc.GetDescriptorInUse() < 3)
                txt += "\n<i>BETA RESULT</i>";
        /*-------*/

        //Set the result
        dataTxt.color = cor;
        dataTxt.text = txt;

        //Return button language
        ReturnLang();
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

        //Camera denied - ANDROID VERSION
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

        errorTxt.text = msg;
        reloadTxt.text = msg2;
    }
}
