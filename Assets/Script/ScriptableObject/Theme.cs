using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NameTheme
{
    White,
    Dark,
}
public enum NameThemeText
{
    Back,
    Gray,
    White,
}
[CreateAssetMenu(fileName = "Theme", menuName = "Data/Theme", order = 1)]
public class Theme : ScriptableObject
{

    [Header("Text")]
    public List<Color> colorText;
    [Header("UI")]
    public Color stageProgress;
    [Header("Play")]
    public Color tileColor;
    public Color star_dot;

    public Color bGcolor;
}
