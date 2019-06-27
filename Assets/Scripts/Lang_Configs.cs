﻿using UnityEngine;
using UnityEngine.UI;

public class Lang_Configs : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    private int count, activeDescriptor;
    private string langKey, compKey;

    public Text configs, lang, langName, descTitle, scale, euclidian, cossine;
    public Slider descSlider;
    public Image slider_fill, slider_knob;
    public Button buttonFlag;
    public Sprite[] flags;

    private Descriptors desc;

    //------------------ PRIVATE METHODS --------------------

    private void Awake()
    {
        desc = FindObjectOfType<Descriptors>();
    }

    private void Start()
    {
        //Setup
        langKey = "lang";
        compKey = "descriptor";

        count = PlayerPrefs.GetInt(langKey) - 1;
        UpdateLanguage();

        activeDescriptor = PlayerPrefs.GetInt(compKey);
        descSlider.value = activeDescriptor;
        SliderColor();
    }

    //------------------ PUBLIC METHODS --------------------

    /* Change the language according to flag order */
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
                configs.text = "Configurações";

                lang.text = "Idioma:";
                langName.text = "Português";

                descTitle.text = "Descritores:";

                scale.text = "- \t\t\tEscala de Segurança \t\t\t+";
                euclidian.text = "Euclidiano";
                cossine.text = "Cosseno";

                break;

            //English
            case 2:
                configs.text = "Settings";

                lang.text = "Language:";
                langName.text = "English";

                descTitle.text = "Descriptors:";

                scale.text = "-\t\t\t\t  Security Scale \t\t\t\t+";
                euclidian.text = "Euclidian";
                cossine.text = "Cossine";

                break;

            //Deutsch
            case 3:
                configs.text = "Einstellungen";

                lang.text = "Sprache:";
                langName.text = "Deutsch";

                descTitle.text = "Deskriptoren:";

                scale.text = "-\t\t\t  Sicherheitsmaßstab\t\t\t+";
                euclidian.text = "Euclidian";
                cossine.text = "Cossine";

                break;
        }

        //Change the flag icon
        buttonFlag.image.sprite = flags[count - 1];
    }

    public void SliderColor()
    {
        activeDescriptor = (int) descSlider.value;
        PlayerPrefs.SetInt(compKey, activeDescriptor);

        desc.SetDescriptorInUse(activeDescriptor);

        slider_knob.color = new Color32(255, 255, 255, 255);

        if (activeDescriptor == 1) // Medium
            slider_knob.color = new Color32(255, 0, 0, 255);
        else if (activeDescriptor == 2) // Medium
            slider_fill.color = new Color32(255, 255, 0, 255);
        else if (activeDescriptor == 3) //Safest
            slider_fill.color = new Color32(0, 255, 0, 255);

        return;
    }
}
