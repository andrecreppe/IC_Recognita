using UnityEngine;
using UnityEngine.UI;

public class Lang_HelpAuth : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count;
    private string langKey;
    private Color32 selected_color, other_color;

    public Text howTo, steps, capture, results, register;
    public Button memeButton;
    public Sprite left, right;

    private Descriptors descr;

    //------------------ PRIVATE METHODS --------------------

    private void Awake()
    {
        descr = FindObjectOfType<Descriptors>();
    }

    private void Start()
    {
        //Setup
        selected_color = new Color32(255, 255, 0, 255); //Yellowish
        other_color = new Color32(50, 50, 50, 255); //Black

        //Language setup
        langKey = "lang";
        count = PlayerPrefs.GetInt(langKey);

        UpdateCapture();
    }

    private void UpdateButtons()
    {
        switch (count)
        {
            case 1:
                capture.text = "Capturar";
                results.text = "Resultados";
                break;

            case 2:
                capture.text = "Capture";
                results.text = "Results";
                break;

            case 3:
                capture.text = "Aufnehmen";
                results.text = "Ergebnis";
                break;
        }
    }

    //------------------ PUBLIC METHODS --------------------

    /* Change the language according to flag order */
    public void UpdateCapture()
    {
        capture.color = selected_color;
        register.color = other_color;
        results.color = other_color;

        switch (count)
        {
            //Português
            case 1:
                howTo.text = "Como capturar a face:";
                steps.text = "1) Alinhe seu rosto com a\n\tborda vermelha\n\n" +
                    "2) Aperte o botão;\n\n" +
                    "2.1) Caso a imagem fique\n" +
                    "\truim, clique no canto \n" +
                    "\tinferor e visualize-a para\n" +
                    "\tpoder deletar;\n\n" +
                    "3) Após capturar as 2\n" +
                    "\timagens, clique em \"Go\"\n" +
                    "\te espere pelo resultado!";

                UpdateButtons();

                break;

            //English
            case 2:
                howTo.text = "How to capture the face:";
                steps.text = "1) Align your face with the \n" +
                    "\tred border;\n\n" +
                    "2) Press the button;\n\n" +
                    "2.1) If the image get bad,\n" +
                    "\tclick in the lower corner\n" +
                    "\tto visualize it and you\n" +
                    "\tcan delete it;\n\n" +
                    "3) After capturing both\n" +
                    "\timages, press \"Go\" and\n" +
                    "\twait for the result!";

                UpdateButtons();

                break;

            //Deutsch
            case 3:
                howTo.text = "Wie das Gesicht aufnehmen:";
                steps.text = "1) Richten Sie das Gesicht\n" +
                    "\tmit dem rotem Rand;\n\n" +
                    "2) Drücken Sie die Taste;\n" +
                    "2.1) Wenn das Bild schleckt\n" +
                    "\tbleiben, klicken Sie unten,\n" +
                    "\tum zu sehen und kann\n" +
                    "\tgelöscht werden;\n\n" +
                    "3) Nachdem 2 Fotos gemaht,\n" +
                    "\tdrücken Sie die Taste\n" +
                    "\t\"Go!\" und warten für das\n" +
                    "\tErgebnis!";

                UpdateButtons();

                break;
        }
    }

    public void UpdateRegister()
    {
        capture.color = other_color;
        register.color = selected_color;
        results.color = other_color;

        switch (count)
        {
            //Português
            case 1:
                howTo.text = "Como salvar uma face:";
                steps.text = "1) Alinhe seu rosto com a\n" +
                	"\tborda vermelha;\n\n" +
                	"2) Aperte o botão azul;\n\n" +
                	"3) Repita o processo 3x;\n\n" +
                	"4) Salve as capturas\n" +
                	"\tpressionando o botão\n" +
                	"\tnovamente para salvar.\n\n" +
                	"*se necessário registar uma\n" +
                	"face nova, é só repetir o\n" +
                	"processo novamente.";

                UpdateButtons();

                break;

            //English
            case 2:
                howTo.text = "How to register a face:";
                steps.text = "1) Align your face within the\n" +
                	"\tred border;\n\n" +
                	"2) Press the blue button;\n\n" +
                	"3) Repeat the process 3x;\n\n" +
                	"4) Save the captures by\n" +
                	"\tpressing the button\n" +
                	"\tagain to save them.\n\n" +
                	"*if is necessary to register a\n" +
                	"new face, just repeat the\n" +
                	"process again. ";

                UpdateButtons();

                break;

            //Deutsch
            case 3:
                howTo.text = "";
                steps.text = "1) Richten Sie Ihr Gesicht\n" +
                	"\tinnerhalb der roter Rand; \n\n" +
                	"2) Drücken Sie die blaue \n" +
                	"\tTaste; \n\n" +
                	"3) Wiederholen Sie den \n" +
                	"\tVorgang 3x; \n\n" +
                	"4) Speichern Sie die \n" +
                	"\tAufnahmen. \n\n" +
                	"*wenn es notwendig ist, eine \n" +
                	"neues Gesicht, wiederholen \n" +
                	"Sie einmal.";

                UpdateButtons();

                break;
        }
    }

    public void UpdateResults()
    {
        capture.color = other_color;
        register.color = other_color;
        results.color = selected_color;

        switch (count)
        {
            //Português
            case 1:
                howTo.text = "Como ler o resultado:";

                steps.text = "- O valor <b>Pdist</b> após o" +
                    "\nreconhecimento representa" +
                    "\na distância entre as duas" +
                    "\nimagens;" +
                    "\n\n- Quanto menor esse valor" +
                    "\nfor, mais semelhantes são;" +
                    "\n\n" + descr.DescriptorInfo(count);

                capture.text = "Capturar";
                results.text = "Resultados";

                break;

            //English
            case 2:
                howTo.text = "How to read the results";

                steps.text = "- The value <b>Pdist</b> after the" +
                    "\nrecognition represents the " +
                    "\ndistance between the two " +
                    "\nimages;" +
                    "\n\n- Lower values represent a" +
                    "\nbigger similarity;" +
                    "\n\n" + descr.DescriptorInfo(count);

                capture.text = "Capture";
                results.text = "Results";

                break;

            //Deutsch
            case 3:
                howTo.text = "Wie das Ergebnis liest";

                steps.text = "- Der Wert Pdist nach der " +
                    "\nErkennung stellt den " +
                    "\nAbstand zwischen den " +
                    "\nbeiden Bildern dar;" +
                    "\n\n- Niedrigere Werte stellen " +
                    "\neine größere Ähnlichkeit dar;" +
                    "\n\n" + descr.DescriptorInfo(count);

                capture.text = "Aufnehmen";
                results.text = "Ergebnis";

                break;
        }
    }

    /* Flip the meme! */
    /* 'Modern problems require modern solutions' */
    public void FlipMeme()
    {
        if (memeButton.image.sprite == right)
            memeButton.image.sprite = left;
        else
            memeButton.image.sprite = right;
    }
}
