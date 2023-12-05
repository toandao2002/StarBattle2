using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class PopUpGameWin : BasePopUP
{
    public static PopUpGameWin instance;
    private void Awake()
    {
        instance = this;
    }
    public TMP_Text level;
    public TMP_Text time;
    private void Start()
    {
        MyEvent.GameWin += Show2;
    }
    public void NextLevel()
    {

     
        GameConfig.instance.SetLevelCurrent(GameConfig.instance.GetCurrentLevel().nameLevel + 1);
        /* DataLevel datalevel = DataGame.GetDataLevel(GameConfig.instance.typeGame, GameConfig.instance.GetCurrentLevel().nameLevel+1);
         if (datalevel != null)
         {
             GameConfig.instance.GetCurrentLevel().datalevel = datalevel;
         }*/
        GameConfig.instance.SetTimeFiishCurrent(GameConfig.instance.GetCurrentLevel().datalevel.timeFinish);
        GameContrler.instance.board.InitBoard();
       
        Hide(-1);
    }
    public void Show2(object dta)
    {
        level.text = GameContrler.instance.board.title.nameTittle.text;
        time.text = GameContrler.instance.board.myTime.timeTxt.text;
        StartCoroutine(IeShow());
    }
    IEnumerator IeShow()
    {
        yield return new WaitForSeconds(2);
        Show();
    }
    private void OnDestroy()
    {
        MyEvent.GameWin -= Show2;
    }
}
