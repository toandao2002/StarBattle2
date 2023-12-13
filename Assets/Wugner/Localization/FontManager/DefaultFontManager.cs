using System.Collections.Generic;
using UnityEngine;

namespace Wugner.Localize
{
	public class DefaultFontManager : ILocalizationFontManager
	{
		Dictionary<string, TMPro.TMP_FontAsset> _fontMap = new Dictionary<string, TMPro.TMP_FontAsset>();
		Dictionary<string, TMPro.TMP_FontAsset> _defaultFontMap = new Dictionary<string, TMPro.TMP_FontAsset>();
		
		public void Init()
		{
			var settings = Resources.Load<LocalizationConfig>("LocalizationConfig");
			foreach (var fo in settings.AllFonts)
			{
				_fontMap.Add(fo.name, fo);
			}
			foreach (var languageSettings in settings.LanguageSettings)
			{
				var language = languageSettings.Language;
				var font = languageSettings.DefaultFont != null ? languageSettings.DefaultFont : GetFont(languageSettings.DefaultFontName);
				if (font != null)
				{
					_defaultFontMap.Add(language, font);
					if (!_fontMap.ContainsKey(font.name))
						_fontMap.Add(font.name, font);
				}
			}
		}
		public TMPro.TMP_FontAsset GetFont(string fontName)
		{
			if (string.IsNullOrEmpty(fontName))
			{
				return null;
			}

            if (!_fontMap.ContainsKey(fontName))
            {
				return null;
            }
            TMPro.TMP_FontAsset f=_fontMap[fontName];
			
			return f;
		}

		public TMPro.TMP_FontAsset GetLanguageDefaultFont(string language)
		{
            TMPro.TMP_FontAsset f;
            foreach(TMPro.TMP_FontAsset font in _defaultFontMap.Values)
            {
                Debug.Log("FONT:" +font.name);
            }
			if (!_defaultFontMap.TryGetValue(language, out f))
			{
				return null;
			}
			return f;
		}
	}
}
