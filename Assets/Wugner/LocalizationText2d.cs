using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Wugner.Localize
{
    [RequireComponent(typeof(TextMeshPro))]
    public class LocalizationText2d : BaseLocalizationUI
    {
        public override VocabularyEntryType RelatedEntryType { get { return VocabularyEntryType.Text; } }

        object[] _params;

        TextMeshPro _text;
        TextMeshPro TextComponent
        {
            get
            {
                if (_text == null)
                    _text = GetComponent<TextMeshPro>();
                return _text;
            }
        }

        public void Set(string id, params object[] objs)
        {
            _areadyHasValue = true;
            _id = id;
            _params = objs;

            var entry = Localization.GetEntry(null,_id);
            UpdateUIComponent(entry);
        }

        protected override void UpdateUIComponent(RuntimeVocabularyEntry entry)
        {
            if (string.IsNullOrEmpty(entry.FontName))
                TextComponent.font = Localization.CurrentDefaultFont;
            else
                TextComponent.font = Localization.GetFont(entry.FontName);

            var str = _params == null || _params.Length == 0 ? entry.Content : string.Format(entry.Content, _params);
            TextComponent.text = str;
        }
    }
}
