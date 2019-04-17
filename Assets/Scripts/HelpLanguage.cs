using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpLanguage : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    public Text howTo, steps;
    public Button langButton;
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

    public void UpdateLanguage() //Change according to flag order
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
                howTo.text = "Como utilizar o app:";

                steps.text = "1) Alinhe seu rosto com a\n\tborda vermelha\n\n" +
                	"2) Aperte o botão \"Snap!\";\n\n" +
                	"2.1) Caso a imagem fique\n" +
                	"\truim, clique no canto \n" +
                	"\tinferor e visualize-a para\n" +
                	"\tpoder deletar;\n\n" +
                	"3) Após capturar as 2\n" +
                	"\timagens, clique em \"Go\"\n" +
                	"\te espere pelo resultado!";

                break;

            //English
            case 2:
                howTo.text = "How to use the app:";

                steps.text = "1) Align your face with the \n" +
                	"\tred border;\n\n" +
                	"2) Press the button \"Snap!\";\n\n" +
                	"2.1) If the image get bad,\n" +
                	"\tclick in the lower corner\n" +
                	"\tto visualize it and you\n" +
                	"\tcan delete it;\n\n" +
                	"3) After capturing both\n" +
                	"\timages, press \"Go\" and\n" +
                	"\twait for the result!";

                break;

            //Deutsch
            case 3:
                howTo.text = "Verwenden Sie die App:";

                steps.text = "1) Richten Sie das Gesicht\n" +
                	"\tmit dem rotem Rand;\n\n" +
                	"2) Drücken Sie die Taste\n" +
                	"\t\"Snap!\";\n" +
                	"2.1) Wenn das Bild schleckt\n" +
                	"\tbleiben, klicken Sie unten,\n" +
                	"\tum zu sehen und kann\n" +
                	"\tgelöscht werden;\n\n" +
                	"3) Nachdem 2 Fotos gemaht,\n" +
                	"\tdrücken Sie die Taste\n" +
                	"\t\"Go!\" und warten für das\n" +
                	"\tErgebnis!";

                break;
        }

        //Change the flag icon
        langButton.image.sprite = flags[count - 1];
    }
}
