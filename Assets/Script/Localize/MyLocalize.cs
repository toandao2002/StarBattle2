using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLocalize : MonoBehaviour
{
    public static MyLocalize Instance;
     
    [SerializeField] private List<BoxChoseLanguage> languagesBox;
    private Dictionary<string, BoxChoseLanguage> dict = new Dictionary<string, BoxChoseLanguage>();
    [SerializeField] private Transform holder;
   
    private void Awake()
    {
        PostInit();
    }
    public  void PostInit()
    {
        if (PlayerPrefs.GetInt("FirstLang", 0) == 0)
        {
            if (Application.systemLanguage == SystemLanguage.English)
            {
                PlayerPrefs.SetString("Language", "en");
            }
            else if (Application.systemLanguage == SystemLanguage.Vietnamese)
            {
                PlayerPrefs.SetString("Language", "vi");
            }
            else if (Application.systemLanguage == SystemLanguage.Spanish)
            {
                PlayerPrefs.SetString("Language", "es");
            }
            else if (Application.systemLanguage == SystemLanguage.German)
            {
                PlayerPrefs.SetString("Language", "gm");
            }
            else if (Application.systemLanguage == SystemLanguage.Italian)
            {
                PlayerPrefs.SetString("Language", "ita");
            }
            else if (Application.systemLanguage == SystemLanguage.Japanese)
            {
                PlayerPrefs.SetString("Language", "jp");
            }
            else if (Application.systemLanguage == SystemLanguage.Korean)
            {
                PlayerPrefs.SetString("Language", "kr");
            }
            else if (Application.systemLanguage == SystemLanguage.Russian)
            {
                PlayerPrefs.SetString("Language", "ru");
            }
            else if (Application.systemLanguage == SystemLanguage.French)
            {
                PlayerPrefs.SetString("Language", "fr");
            }
            else if (Application.systemLanguage == SystemLanguage.Turkish)
            {
                PlayerPrefs.SetString("Language", "tr");
            }
            else if (Application.systemLanguage == SystemLanguage.Arabic)
            {
                PlayerPrefs.SetString("Language", "ar");
            }
            else if (Application.systemLanguage == SystemLanguage.Portuguese)
            {
                PlayerPrefs.SetString("Language", "pt");
            }
            else if (Application.systemLanguage == SystemLanguage.Indonesian)
            {
                PlayerPrefs.SetString("Language", "id");
            }
            PlayerPrefs.SetInt("FirstLang", 1);
        }

        Wugner.Localize.Localization.Instance.SwitchLanguage(PlayerPrefs.GetString("Language", "en"));
        string[] keys = { "en", "fr", "vi", "es", "gm", "in", "ita", "jp", "kr", "ru", "tr", "ar", "pt", "id" };
        string[] names =
        {
            "English", "French", "Vietnamese", "Espano", "German", "Hindi", "Italian", "Japanese", "Korean", "Russian",
            "Turkey", "Arabic", "Portugal","Indonesian"
        };
         
        for (int i = 0; i < holder.childCount; i++)
        {
             
            languagesBox[i].Init(keys[i], names[i]);
            //holder.GetChild(i).GetComponentInChildren<UnityEngine.UI.Image>().sprite = flagSprites[i];
            
        }

        for (int i = 0; i < languagesBox.Count; i++)
        {
            dict.Add(keys[i], languagesBox[i]);
        }

        Instance = this;
    }

    bool first = false;
    public Color defC = new Color(135f / 255f, 135f / 255, 135f / 255);
    public Color pickC = new Color(255 / 255f, 121 / 255f, 50 / 255f);

    public void SetUp(bool first = false)
    {
        this.first = first;
        UpdateSelected();

        if (!first)
        {
            //FireBaseManager.Instance.LogEvent("Panel_LanguageOpen");
        }

    }

    void UpdateSelected()
    {
        for (int i = 0; i < languagesBox.Count; i++)
        {
            languagesBox[i].bgr.color = Color.white;
            languagesBox[i].text.color = defC;
        }

        dict[PlayerPrefs.GetString("Language", "en")].text.color = pickC;
        dict[PlayerPrefs.GetString("Language", "en")].bgr.color = Color.gray;
    }


    

    public void Select(string key)
    {
        PlayerPrefs.SetString("Language", key);
        Wugner.Localize.Localization.Instance.SwitchLanguage(key);
        UpdateSelected();

        //FireBaseManager.Instance.LogEvent("Panel_LanguageSwitch_" + key);
        // SoundManager.Instance.PlaySFX(SFX.clickSFX);
        
    }
}
