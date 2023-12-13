using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wugner.Localize
{
	public interface ILocalizationFontManager
	{
		void Init();
        TMPro.TMP_FontAsset GetFont(string fontName);
        TMPro.TMP_FontAsset GetLanguageDefaultFont(string language);
	}
}
