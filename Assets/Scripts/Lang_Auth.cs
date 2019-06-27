using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class Lang_Auth : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    private Descriptors desc;

    public Image locked;
    public Text errorTxt, reloadTxt, stateTxt, descTxt, btnSnap, returnTxt;

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

        UpdateButton();
        UpdateReturn();
    }

    private void DescriptorName()
    {
        int in_use = desc.GetDescriptorInUse();
        string txt = "";

        switch (in_use)
        {
            case 1:
                txt = "Cityblock";
                break;
            case 2:
                txt = "Euclidian";
                break;
            case 3:
                txt = "Cossine";
                break;
        }

        descTxt.text += "\n<i>[" + txt + "]</i>";
    }

    private void UpdateReturn()
    {
        string txt = "";

        switch (count)
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

        if (!unlocked)
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
                    description = "Das Gesicht stimmt" +
                    	"\nnicht mit dem " +
                    	"\nregistrierten " +
                    	"\nüberein" +
                    	"\n;-;";
                    break;
            }
        }
        else
        {
            textColor = new Color32(0, 207, 80, 255);

            switch (count)
            {
                case 1:
                    description = "A face bateu com" +
                    	"\na registrada no" +
                    	"\ndispositivo!" +
                    	"\n:)";
                    break;
                case 2:
                    description = "The face matched" +
                    	"\nwith the registered" +
                    	"\none" +
                    	"\n:)";
                    break;
                case 3:
                    description = "Das Gesicht" +
                    	"\npasste zu" +
                    	"\nden registrierten" +
                    	"\n:)";
                    break;
            }

        }

        descTxt.text = description;
        descTxt.color = textColor;

        DescriptorName();
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
