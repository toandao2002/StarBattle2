using System.Collections.Generic;
using UnityEngine;
using System.Threading;
namespace Wugner.Localize
{
    public class DefaultVocabularyManager : ILocalizationVocabularyManager
	{
		Dictionary<string, Dictionary<string, RuntimeVocabularyEntry>> _languageToEntryMap = new Dictionary<string, Dictionary<string, RuntimeVocabularyEntry>>();
		public virtual void Init()
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
                else if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified || Application.systemLanguage == SystemLanguage.ChineseTraditional)
                {
                    PlayerPrefs.SetString("Language", "cn");
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
                PlayerPrefs.SetInt("FirstLang", 1);
            }
            var assetsList = Resources.Load<VocabulariesAsset>("Vocabularies_"+PlayerPrefs.GetString("Language","en"));
            //for (int i=0;i<assetsList.Length;i++)
            //{

                LoadEntries(assetsList);
            //}
        }

        void LoadEntries(VocabulariesAsset asset)
        {
            if (asset != null && asset.VocabularyEntries != null)
            {
                List<VocabularyEntry> entries = asset.VocabularyEntries;
                for (int i=0;i<entries.Count;i++)
                {
                    var entry = entries[i];
                    Dictionary<string, RuntimeVocabularyEntry> temp;
                    if (!_languageToEntryMap.TryGetValue(entry.Language, out temp))
                    {
                        temp = new Dictionary<string, RuntimeVocabularyEntry>();
                        _languageToEntryMap.Add(entry.Language, temp);
                    }

                    temp.Add(entry.ID, new RuntimeVocabularyEntry()
                    {
                        ID = entry.ID,
                        Content = entry.Content,
                        FontName = entry.FontName,
                    });
                }

            }
        }

		public virtual Dictionary<string, RuntimeVocabularyEntry> GetByLanguage(string language)
		{
			Dictionary<string, RuntimeVocabularyEntry> v;
			if (!_languageToEntryMap.TryGetValue(language, out v))
            {
                LoadEntries(Resources.Load<VocabulariesAsset>("Vocabularies_" + language));
                if (!_languageToEntryMap.TryGetValue(language, out v))
                {
                    return null;
                }
            }
			return v;
		}
	}
}
