using UnityEngine;
using UnityEngine.UI;

public class Lang_Menu : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;

    public Text intro, auth, authBut, comp, compBut;

    //------------------ PRIVATE METHODS --------------------

    private void Start()
    {
        //Setup
        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey);

        UpdateLanguage();
    }

    /* Change the language according to flag order */
    private void UpdateLanguage()
    {
        switch (count)
        {
            //Português
            case 1:
                intro.text = "Selecione a funcionalidade" +
                	"\ndesejada:";

                authBut.text = "Autenticador";
                auth.text = "Simula um sistema biometrico" +
                	"\n(como desbloquear o celular)";

                compBut.text = "Comparador";
                comp.text = "Compara duas pessoas" +
                    "\n(reconhecimento de imagens)";

                break;

            //English
            case 2:
                intro.text = "Select the desired" +
                    "\nfunctionality:";

                authBut.text = "Authentifier";
                auth.text = "Simulate a biometric system" +
                    "\n(like unlocking the phone)";

                compBut.text = "Comparator";
                comp.text = "Compare two persons" +
                    "\n(image recognition)";

                break;

            //Deutsch
            case 3:
                intro.text = "Wählen Sie die gewünschte " +
                	"\nFunktion aus:";

                authBut.text = "Authentifizierer";
                auth.text = "Simulieren eines biometrischen Systems " +
                	"\n(z.B. Entsperren des Telefons)";

                compBut.text = "Vergleicher";
                comp.text = "Zwei Personen vergleichen " +
                	"\n(Bilderkennung)";

                break;
        }
    }
}
