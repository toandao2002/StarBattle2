using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoxBigLevelHome : MonoBehaviour
{
    public Image bar;
    public Color barLight;
    public Color barDark;
    public Image bgr;
    public Sprite bgrLight;
    public Sprite bgrDark;
    public TypeGame typeGame;
    public TMP_Text nameLevel;
     
    private void OnEnable()
    {
        ChangeTheme();
        MyEvent.ChangeTheme += ChangeTheme;
    }
    private void OnDisable()
    {
        MyEvent.ChangeTheme -= ChangeTheme;
    }
    public void ChangeTheme()
    {
        if (GameConfig.instance.nameTheme == NameTheme.Dark)
        {
            bar.color = barDark;
            bgr.sprite = bgrDark;
            nameLevel.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
        }
        else
        {
            bar.color = barLight;
            bgr.sprite = bgrLight;
            nameLevel.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Back];
        }
    }
    public void SetRateBarLevel(float value)
    {
        bar.fillAmount = value;
    }
}

