using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
public class PopUpGameWin : BasePopUP
{
    public static PopUpGameWin instance;
    
    public TMP_Text level;
    public TMP_Text time;
    public TMP_Text congrate;

    public Sprite bgrLight;
    public Sprite bgrDark;
    private void Awake()
    {
        instance = this;
        MyEvent.ChangeTheme += ChangeTheme;
    }
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);

    }
    private void OnEnable()
    {
        
    }
    private void Start()
    {
        MyEvent.GameWin += Show2;
    }
    public void NextLevel()
    {

        int levelNext = GameConfig.instance.GetCurrentLevel().nameLevel + 1;
        
        bool bought = GameConfig.instance.GetDataPack().CheckLevelHasBeenBought(levelNext, GameConfig.instance.typeGame, GameConfig.instance.levelCommon);
        if (bought) {
            GameConfig.instance.SetLevelCurrent(levelNext);
            /* DataLevel datalevel = DataGame.GetDataLevel(GameConfig.instance.typeGame, GameConfig.instance.GetCurrentLevel().nameLevel+1);
             if (datalevel != null)
             {
                 GameConfig.instance.GetCurrentLevel().datalevel = datalevel;
             }*/
            GameConfig.instance.SetTimeFiishCurrent(GameConfig.instance.GetCurrentLevel().datalevel.timeFinish);
            GameContrler.instance.board.InitBoard();
            Hide(-1);

        }
        else
        {
            GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel2,1,-1);
            GameManger.instance.manageUi.HidePopUP(NamePopUp.GamePlay,-1);
            ChoseLevel_2.instance.popUpSuggestBuyPack.Show();
            Hide(-1);
        }

       
    }
    public void Show2(object dta)
    {
        level.text = GameContrler.instance.board.title.nameTittle.text;
        time.text = GameContrler.instance.board.myTime.timeTxt.text;
        StartCoroutine(IeShow());
        ChangeTheme();
    }
    IEnumerator IeShow()
    {
        yield return new WaitForSeconds(3);
        GameContrler.instance.SuggestPlayMoreDifficult(null);
        Show();
    }
    private void OnDestroy()
    {
        MyEvent.GameWin -= Show2;
    }
    public override void ChangeTheme()
    {
        base.ChangeTheme();
        NameTheme theme = GameConfig.instance.nameTheme;
        if (theme == NameTheme.Dark)
        {

            level.color = GameConfig.instance.darkMode.colorText[(int)TextColor.White];
            time.color = GameConfig.instance.darkMode.colorText[(int)TextColor.White];
            congrate.color = GameConfig.instance.darkMode.colorText[(int)TextColor.White];
            BgrMain.sprite = bgrDark;
        }
        else
        {

            BgrMain.sprite = bgrLight;
            level.color = GameConfig.instance.darkMode.colorText[(int)TextColor.White];
            congrate.color = GameConfig.instance.darkMode.colorText[(int)TextColor.White];
            time.color = GameConfig.instance.darkMode.colorText[(int)TextColor.White];
            
        }
        if (BgrMain2 != null)
            BgrMain2.sprite = BgrMain.sprite;

    }
    
}
