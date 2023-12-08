using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpContinuePlay : BasePopUP
{
    public static PopUpContinuePlay instance;
    public Image bgr;
    public TMP_Text contentTxt;
    public List<Color> colorBgr;
    public override void Awake()
    {
        base.Awake();
        instance = this;
    }
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);
        ChangeTheme();
    }
    public override void ChangeTheme()
    {
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            bgr.color = colorBgr[1];
            contentTxt.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
        }
        else
        {
            bgr.color = colorBgr[0];
            contentTxt.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Gray];
        }
    }
}
