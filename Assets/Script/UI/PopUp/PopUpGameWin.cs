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
            level.color = GameConfig.instance.darkMode.colorText[(int)TextColor.Black];
            congrate.color = GameConfig.instance.darkMode.colorText[(int)TextColor.Black];
            time.color = GameConfig.instance.darkMode.colorText[(int)TextColor.Black];
            
        }
        if (BgrMain2 != null)
            BgrMain2.sprite = BgrMain.sprite;

    }
}
