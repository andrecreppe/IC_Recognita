using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoLanguage : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    public Text info, developers;
    public Button langButton;
    public Sprite[] flags;

    //------------------ PRIVATE METHODS --------------------

    private void Start()
    {
        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey) - 1;
        UpdateLanguage();
    }

    //------------------ PUBLIC METHODS --------------------

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

                developers.text = "Aluno Bolsista:\n" +
                    "\tAndré Z. Creppe\n\n" +
                    "Professor Orientador:\n" +
                    "\tProf. Dr. Nilceu Marana";

                break;

            //English
            case 2:
                info.text = "Scientific Initiation Project \n" +
                "designed to test the \n" +
                "effectiveness of a \n" +
                "biometric system utilizing \n" +
                "elements of the face and \n" +
                "ocular region.";

                developers.text = "Initiation Student:\n" +
                    "\tAndré Z. Creppe\n\n" +
                    "Advisor Teacher:\n" +
                    "\tProf. Dr. Nilceu Marana";

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

                developers.text = "Wissenchaftlichte Student:\n" +
                    "\tAndré Z. Creppe\n\n" +
                    "Ratgeber Lehrer:\n" +
                    "\tProf. Dr. Nilceu Marana";

                break;
        }

        //Change the flag icon
        langButton.image.sprite = flags[count - 1];
    }
}
