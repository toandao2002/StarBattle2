﻿using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Wugner.Localize
{
	public class LocalizationTextSelectorWindow : EditorWindow
	{
		static LocalizationTextSelectorWindow _window;
		public static void Show(VocabularyEntryType entryType, string current, Action<string> onSelect)
		{
			if (_window == null)
				_window = CreateInstance<LocalizationTextSelectorWindow>();
			_window._entryType = entryType;
			_window._current = current;
			_window._onSelect = onSelect;
			_window.ShowAuxWindow();
		}

		VocabularyEntryType _entryType;
		string _filter;
		string _current;
		Action<string> _onSelect;
        Vector2 pos = Vector2.zero;
		int _selected;
		private void OnGUI()
		{
			_filter = EditorGUILayout.TextField(_filter);

            //new EditorGUILayout.ScrollViewScope(pos, false, false))
            pos=GUILayout.BeginScrollView(pos);
            try
            {
                var entries = EditorMultiLanguageEntryCollection.Instance.GetEntries(_entryType)
                    .Where(e => string.IsNullOrEmpty(_filter) || e.ID.Contains(_filter) || e.Remark.Contains(_filter))
                    .ToList();
                {
                    var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * entries.Count);

                    {
                        var selected = entries.FindIndex(e => e.ID == _current);
                        selected = GUI.SelectionGrid(rect, selected, entries.Select(e => e.ID).ToArray(), 1, "PreferencesKeysElement");

                        _current = selected < 0 ? _current : entries[selected].ID;
                        _onSelect(_current);
                    }
                    {
                        var remarkRect = rect;
                        remarkRect.x += rect.width / 2;
                        remarkRect.height = EditorGUIUtility.singleLineHeight;
                        foreach (var e in entries)
                        {
                            if (e.ID != _current)
                                GUI.Label(remarkRect, e.Remark, "PreferencesKeysElement");
                            else
                                GUI.Label(remarkRect, e.Remark, new GUIStyle("PreferencesKeysElement") { normal = new GUIStyleState() { textColor = Color.white } });
                            remarkRect.y += EditorGUIUtility.singleLineHeight;
                        }
                    }
                }
            }
            catch { }
            GUILayout.EndScrollView();
		}
	}
}
