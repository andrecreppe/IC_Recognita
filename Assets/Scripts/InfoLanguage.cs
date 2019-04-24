using UnityEngine;
using UnityEngine.UI;

public class InfoLanguage : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;
    private bool emailAndre, emailNilceu;

    public Text info, andre, nilceu;
    public Button langButton;
    public Sprite[] flags;

    //------------------ PRIVATE METHODS --------------------

    private void Start()
    {
        //Setup
        emailAndre = false;
        emailNilceu = false;

        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey) - 1;
        UpdateLanguage();
    }

    /* Change the 'André' content */
    private void AndreText()
    {
        if (emailAndre)
        {
            switch (count) //Show contacts
            {
                case 1: //Portugues
                    andre.text = "Contato André:\n" +
                        "\tandre.creppe@uol.com.br";
                    break;
                case 2: //English
                    andre.text = "André's contact:\n" +
                        "\tandre.creppe@uol.com.br";
                    break;
                case 3: //Deutsch
                    andre.text = "André Kontakt:\n" +
                        "\tandre.creppe@uol.com.br";
                    break;
            }
        }
        else
        {
            switch (count) //Show Persons
            {
                case 1: //Portugues
                    andre.text = "Aluno Bolsista:\n" +
                        "\tAndré Z. Creppe";
                    break;
                case 2: //English
                    andre.text = "Initiation Student:\n" +
                        "\tAndré Z. Creppe";
                    break;
                case 3: //Deutsch
                    andre.text = "Wissenchaftlichte Student:\n" +
                        "\tAndré Z. Creppe";
                    break;
            }
        }
    }

    /* Change the 'Nilceu' content */
    private void NilceuText()
    {
        if (emailNilceu) //Show Contact
        {
            switch (count)
            {
                case 1: //Portugues
                    nilceu.text = "Contato Nilceu:\n" +
                        "\tnilceu@fc.unesp.br";
                    break;
                case 2: //English
                    nilceu.text = "Nilceu's contact:\n" +
                        "\tnilceu@fc.unesp.br";
                    break;
                case 3: //Deutsch
                    nilceu.text = "Nilceu Kontakt:\n" +
                        "\tnilceu@fc.unesp.br";
                    break;
            }
        }
        else
        {
            switch (count) //Show Persons
            {
                case 1: //Portugues
                    nilceu.text = "Professor Orientador:\n" +
                        "\tProf. Dr. Nilceu Marana";
                    break;
                case 2: //English
                    nilceu.text = "Advisor Teacher:\n" +
                        "\tProf. Dr. Nilceu Marana";
                    break;
                case 3: //Deutsch
                    nilceu.text = "Ratgeber Lehrer:\n" +
                        "\tProf. Dr. Nilceu Marana";
                    break;
            }
        }
    }

    //------------------ PUBLIC METHODS --------------------

    /* Change the language according to the flag order */
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
                info.text = "Projeto de Iniciação\n" +
                "Científica voltado para\n" +
                "testar a efetividade de\n" +
                "um sistema biometrico\n" +
                "utilizando elementos da \n" +
                "face e da região ocular.";

                AndreText();
                NilceuText();

                break;

            //English
            case 2:
                info.text = "Scientific Initiation Project \n" +
                "designed to test the \n" +
                "effectiveness of a \n" +
                "biometric system utilizing \n" +
                "elements of the face and \n" +
                "ocular region.";

                AndreText();
                NilceuText();

                break;

            //Deutsch
            case 3:
                info.text = "Wissenschaftliches \n" +
                "Initiationsprojekt, \n" +
                "das die Wirksamkeit eines \n" +
                "biometrischen Systems mit \n" +
                "Elementen des Gesichts \n" +
                "und des Augengebiets \n" +
                "testen soll.";

                AndreText();
                NilceuText();

                break;
        }

        //Change the flag icon
        langButton.image.sprite = flags[count - 1];
    }

    /* Flip 'Andre' content state */
    public void ContactAndre()
    {
        emailAndre = !emailAndre;

        AndreText();
    }

    /* Flip 'Nilceu' content state */
    public void ContactNilceu()
    {
        emailNilceu = !emailNilceu;

        NilceuText();
    }
}
