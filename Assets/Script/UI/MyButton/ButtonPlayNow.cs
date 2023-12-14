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
    public void SetInit(string level)
    {
        this.nameLevel.text = level;
    }
    public void SetLevel( int level, int timeF)
    {
        this.nameLevel.text =  Util.GetLocalizeRealString(Util.GetIdLocalLizeTypeGame(GameConfig.instance.typeGame)) +"-"+ level;
        this.level = level;
        this.timef = timeF;
    }
    public void ChoseLevel()
    {
        DataLevelUser dataLevelUser = GameConfig.instance.GetDataLevelCommon();
        /*  if (!dataLevelUser.CheckLevelFinish(GameConfig.instance.typeGame, level))
          {
              PopUpContinuePlay.instance.Show();
              return;
          }*/


        GameConfig.instance.SetLevelCurrent(level);
        GameConfig.instance.SetTimeFiishCurrent(timef);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.GamePlay);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel2);
    }
}
