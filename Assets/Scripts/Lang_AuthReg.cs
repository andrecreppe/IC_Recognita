using UnityEngine;
using UnityEngine.UI;
#if PLATFORM_ANDROID
    using UnityEngine.Android;
#endif

public class Lang_AuthReg : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    public Text errorTxt, reloadTxt, snapText, msgText, descText, warnText, yesText, noText;
    public Button snapButton;

    //---------------- PRIVATE METHODS -------------------

    private void Start()
    {
        //Setup
        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey);

        UpdateSnapText(-1);
        UpdateMessageText();
    }

    //---------------- PUBLIC METHODS --------------------

    public void UpdateSnapText(int numimg)
    {
        string msg = "";

        numimg++;

        if (numimg < 3)
        {
            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "Registrar (" + numimg + "/3)";

                    break;
                case 2: //En
                    msg = "Register (" + numimg + "/3)";

                    break;
                case 3: //De
                    msg = "Unterlagen (" + numimg + "/3)";

                    break;
            }
        }
        else
        {
            ColorBlock colors = snapButton.colors;
            colors.normalColor = new Color32(0, 255, 255, 255);
            colors.highlightedColor = new Color32(0, 255, 255, 255);
            colors.pressedColor = new Color32(160, 255, 255, 255);

            snapButton.colors = colors;

            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "Salvar! (3/3)";

                    break;
                case 2: //En
                    msg = "Save! (3/3)";

                    break;
                case 3: //De
                    msg = "Retten! (3/3)";

                    break;
            }
        }

        snapText.text = msg;
    }

    public void UpdateMessageText()
    {
        string msg = "";

        if(!PlayerPrefs.HasKey("features0"))
        {
            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "Primeiro registro";

                    break;
                case 2: //En
                    msg = "First record";

                    break;
                case 3: //De
                    msg = "Erste verzeichnis";

                    break;
            }
        }
        else
        {
            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "Novo registro";

                    break;
                case 2: //En
                    msg = "New record";

                    break;
                case 3: //De
                    msg = "Neu verzeichnis";

                    break;
            }
        }

        msgText.text = msg;
    }

    public void UpdateConfirmationText()
    {
        string description = "", warning = "", yes = "", no = "";

        switch (PlayerPrefs.GetInt(langKey))
        {
            case 1: //Pt
                description = "Sobrescrever os" +
                	"\ndados salvos";
                warning = "Você quer realmente" +
                	"\nsubstituir os dados" +
                	"\nsalvos com novos?";
                yes = "Sim";
                no = "Não";

                break;
            case 2: //En
                description = "Overwrite saved" +
                	"\ndata";
                warning = "Do you realy want" +
                	"\nto replace the " +
                	"\nactual features with" +
                	"\nnew ones?";
                yes = "Yes";
                no = "No";

                break;
            case 3: //De
                description = "Überschreiben Sie" +
                	"\ngespeicherte Daten:";
                warning = "Möchten Sie wirklich" +
                	"\ndie gespeicherten" +
                	"\nDaten durch neue" +
                	"\nersetzen?";
                yes = "Ja";
                no = "Nein";

                break;
        }

        descText.text = description;
        warnText.text = warning;
        yesText.text = yes;
        noText.text = no;
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
