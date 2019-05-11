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

    public Text errorTxt, reloadTxt, snapText;

    //---------------- PRIVATE METHODS -------------------

    private void Start()
    {
        //Setup
        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey);
    }

    //---------------- PUBLIC METHODS --------------------

    public void UpdateSnapText(int count)
    {
        string msg = "";

        count++;

        if (count < 3)
        {
            switch (PlayerPrefs.GetInt(langKey))
            {
                case 1: //Pt
                    msg = "Registrar (" + count + "/3)";

                    break;
                case 2: //En
                    msg = "Register (" + count + "/3)";

                    break;
                case 3: //De
                    msg = "Unterlagen (" + count + "/3)";

                    break;
            }
        }
        else
        {
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
