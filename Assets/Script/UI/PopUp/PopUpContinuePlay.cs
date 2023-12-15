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
    int level;
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
    public void WatchAds()
    {
        DataLevelUser dataLevelComon = GameConfig.instance.GetDataLevelCommon();
        level = dataLevelComon.GetAmountLevelCanPlayByTypeGame(GameConfig.instance.typeGame) + 1;
        dataLevelComon.IncNumLevelCanPlayInGame(GameConfig.instance.typeGame, level);
        DataGame.SetDataJson(DataGame.DataLevelComon, Util.ConvertObjectToString<DataLevelUser>(GameConfig.instance.GetDataLevelCommon()));
        DataLevelUser dataLevelUser = GameConfig.instance.GetDataLevelCommon();
        bool bought = GameConfig.instance.GetDataPack().CheckLevelHasBeenBought(level, GameConfig.instance.typeGame, GameConfig.instance.levelCommon);
        if (!bought)
        {
            ChoseLevel_2.instance.popUpSuggestBuyPack.Show();
            return;
        }
        {
            GameConfig.instance.SetLevelCurrent(level);
            GameConfig.instance.SetTimeFiishCurrent(0);
            GameManger.instance.manageUi.ShowPopUp(NamePopUp.GamePlay);
            GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel2);
        }

        Hide(1);
    }
}
