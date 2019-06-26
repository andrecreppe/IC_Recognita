using UnityEngine;
using UnityEngine.UI;

public class ImgProcessing_Auth : MonoBehaviour
{
    //------------------ VARIABLES --------------------

    public Image result_menu;

    private Descriptors desc;
    private CameraController camcon;
    private Lang_Auth authlang;

    //---------------- PRIVATE METHODS -------------------

    private void Awake()
    {
        desc = FindObjectOfType<Descriptors>();
        camcon = FindObjectOfType<CameraController>();
        authlang = FindObjectOfType<Lang_Auth>();
    }

    /* Save the features array (256 size) to the memory */
    private void SaveFeatures(int[] features)
    {
        string featuresKey = "features", scount, final;

        for (int i = 0; i < 256; i++)
        {
            scount = i.ToString();
            final = featuresKey + scount;

            PlayerPrefs.SetInt(final, features[i]);
        }

        PlayerPrefs.Save();
    }

    /* Get the features array (256 size) saved in memory */
    private int[] GetFeatures()
    {
        int[] features = new int[256];
        string featuresKey = "features", scount, final;

        for (int i = 0; i < 256; i++)
        {
            scount = i.ToString();
            final = featuresKey + scount;

            features[i] = PlayerPrefs.GetInt(final);
        }

        return features;
    }

    /* Opens the result display to show the results */
    private void ShowResults(double resp, double tresh)
    {
        bool unlocked;

        //Set the Result Display active
        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();

        unlocked = resp <= tresh;

        Debug.Log("unlocked = " + unlocked);

        authlang.Result(unlocked);
    }

    //---------------- PUBLIC METHODS --------------------

    /* Manage the recognition calling -> Execute the comparison order */
    public void SavePerson(Texture2D img1, Texture2D img2, Texture2D img3)
    {
        int sum = 0;
        int[] f1, f2, f3, final;

        final = new int[256];

        f1 = desc.ExtractLBPFeatures(img1);
        f2 = desc.ExtractLBPFeatures(img2);
        f3 = desc.ExtractLBPFeatures(img3);

        for(int i=0; i<256; i++)
        {
            sum = (f1[i] + f2[i] + f3[i]) / 3;

            final[i] = sum;
        }

        SaveFeatures(final);
    }

    /* Simulate unlocking a phone with face (compare stored and now) */
    public void TryToUnlock(Texture2D pic)
    {
        double resp;
        int[] features, recorded;

        features = desc.ExtractLBPFeatures(pic);
        recorded = GetFeatures();

        resp = desc.CompareImages(features, recorded);

        Debug.Log("cossine = " + resp);

        ShowResults(resp, desc.ActiveTreshold());
    }

    /* Close the result display */
    public void CloseResults()
    {
        result_menu.gameObject.SetActive(!result_menu.gameObject.activeSelf);
        camcon.ChangeCameraStatus();
    }
}
