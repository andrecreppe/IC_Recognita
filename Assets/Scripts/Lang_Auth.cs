using UnityEngine;
using UnityEngine.UI;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class Lang_Auth : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    public Image locked;
    public Text errorTxt, reloadTxt, stateTxt, descTxt, btnSnap;

    //---------------- PRIVATE METHODS -------------------

    private void Start()
    {
        //Setup
        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey);

        UpdateButton();
    }

    private void UpdateButton()
    {
        string buttxt = "";

        switch (count)
        {
            case 1:
                buttxt = "Desbloquear!";
                break;
            case 2:
                buttxt = "Try to unlock!";
                break;
            case 3:
                buttxt = "Unverschlossen!";
                break;
        }

        btnSnap.text = buttxt;
    }

    private void UpdateState(bool unlocked)
    {
        string state = "";

        if(unlocked)
        {
            switch (count)
            {
                case 1:
                    state = "Desbloqueado!";
                    break;
                case 2:
                    state = "Unlocked!";
                    break;
                case 3:
                    state = "Unverschlossen!";
                    break;
            }
        }
        else
        {
            switch (count)
            {
                case 1:
                    state = "Bloqueado!";
                    break;
                case 2:
                    state = "Locked!";
                    break;
                case 3:
                    state = "Schlossen!";
                    break;
            }

        }

        stateTxt.text = state;
    }

    private void UpdateDescription(bool unlocked)
    {
        string description = "";
        Color32 textColor;

        if (unlocked)
        {
            textColor = new Color32(207, 109, 0, 255);

            switch (count)
            {
                case 1:
                    description = "A face não bate" +
                    	"\ncom a registrada" +
                    	"\nno dispositivo " +
                    	"\n;-;";
                    break;
                case 2:
                    description = "The face doesn't" +
                    	"\nmatch with the" +
                    	"\nresistered one" +
                    	"\n;-;";
                    break;
                case 3:
                    description = "";
                    break;
            }
        }
        else
        {
            textColor = new Color32(0, 207, 80, 255);

            switch (count)
            {
                case 1:
                    description = "A face bateu com " +
                    	"\na registrada no " +
                    	"\ndispositivo!" +
                    	"\n:)";
                    break;
                case 2:
                    description = "";
                    break;
                case 3:
                    description = "";
                    break;
            }

        }

        descTxt.text = description;
        descTxt.color = textColor;
    }

    //---------------- PUBLIC METHODS --------------------

    public void Result(bool unlocked)
    {
        locked.gameObject.SetActive(!unlocked);

        UpdateState(unlocked);
        UpdateDescription(unlocked);
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
