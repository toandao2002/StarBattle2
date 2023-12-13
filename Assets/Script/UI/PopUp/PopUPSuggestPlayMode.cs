using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class PopUPSuggestPlayMode : BasePopUP
{
    public static PopUPSuggestPlayMode instance;

    public Image bgr;
    public TMP_Text contentTxt;
    public TMP_Text txtTitle;
    public List<Color> colorBgr;
    public TypeGame typeGame;
    public GameObject MainPopUp;
    private void Awake()
    {
        instance = this;
    }
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);
        this.typeGame = (TypeGame)data;
        ChangeTheme();
        MainPopUp.GetComponent<RectTransform>().DOAnchorPos3DY(0, 1f).From(-1000).SetEase(Ease.InOutBack);
    }
    public void PlayMode()
    {
        GameConfig.instance.SetTypeGame(typeGame);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel2,1,-1);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.GamePlay,-1);
        PopUpGameWin.instance.Hide();
        Hide(-1);
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
