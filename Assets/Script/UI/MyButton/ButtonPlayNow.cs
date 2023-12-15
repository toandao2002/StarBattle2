using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonPlayNow : MonoBehaviour
{
    public Image bgr;
    public Sprite bgrLigth;
    public Sprite bgrDark;
    public TMP_Text nameLevel;
    int level;
    int timef;
    TypeGame typeGame;
    private void Awake()
    {
        MyEvent.UpdataLocalize += UpdataLocalize ;
    }
    public void SetInit(string level)
    {
        this.nameLevel.text = level;
    }
    void  UpdataLocalize()
    {
        nameLevel.text = Util.GetLocalizeRealString(Util.GetIdLocalLizeTypeGame(typeGame)) + "-" + level;
    }
    public void SetLevel( int level, int timeF,TypeGame typeGame)
    {
        this.typeGame = typeGame ;
        this.level = level;
        this.timef = timeF;
        UpdataLocalize();
    }
    public void ChoseLevel()
    {
        DataLevelUser dataLevelUser = GameConfig.instance.GetDataLevelCommon();
        /*  if (!dataLevelUser.CheckLevelFinish(GameConfig.instance.typeGame, level))
          {
              PopUpContinuePlay.instance.Show();
              return;
          }*/
        bool bought = GameConfig.instance.GetDataPack().CheckLevelHasBeenBought(level, typeGame, GameConfig.instance.levelCommon);
        if (!bought)
        {
            ChoseLevel_2.instance.popUpSuggestBuyPack.Show();
            return;
        }
        GameConfig.instance.SetTypeGame(typeGame);
        GameConfig.instance.SetLevelCurrent(level);
        GameConfig.instance.SetTimeFiishCurrent(timef);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.GamePlay);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel2);
    }
}
