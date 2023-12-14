using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSuggestBuyPack : BasePopUP
{
    public Image bgr;
    public TMP_Text contentTxt;
    public TMP_Text txtTitle;
    public List<Color> colorBgr;
    public override void Awake()
    {
        base.Awake();

    }
    public override void Hide(int dir = 1)
    {
        base.Hide(dir);
        ShopUI.instance.Show();
    }
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);
        ChangeTheme();

    }
    public void WatchAdGetHint()
    {
        DataGame.SetInt(DataGame.AmountHint, DataGame.GetInt(DataGame.AmountHint) + 3);
        MyEvent.UpdateDataAmountHint?.Invoke();
        Hide();
    }
    public override void ChangeTheme()
    {
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {
            bgr.color = colorBgr[1];
            contentTxt.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
            txtTitle.color = GameConfig.instance.darkMode.colorText[(int)NameThemeText.White];
        }
        else
        {
            bgr.color = colorBgr[0];
            contentTxt.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Gray];
            txtTitle.color = GameConfig.instance.lightMode.colorText[(int)NameThemeText.Back];
        }
    }
}
