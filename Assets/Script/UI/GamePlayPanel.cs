using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : BasePopUP
{
    public Sprite bgrDark;
    public Sprite bgrLight;

    public Image bgrBottom;
    public List<Color> bgrBottomColor;
    public List<TMP_Text> txt;
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);
        ChangeTheme();
    }
    public override void ChangeTheme()
    {
        base.ChangeTheme();
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            BgrMain.sprite = bgrDark;
            for(int i = 0; i < txt.Count; i++)
            {
                txt[i].color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
            }
            bgrBottom.color = bgrBottomColor[1];
        }
        else
        {
            BgrMain.sprite = bgrLight;
            for (int i = 0; i < txt.Count; i++)
            {
                txt[i].color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Gray];
            }
            bgrBottom.color = bgrBottomColor[0];
        }
         
    }
}
