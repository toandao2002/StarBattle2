using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpGameWin : BasePopUP
{
    private void Start()
    {
        MyEvent.GameWin += Show2;
    }
    public void NextLevel()
    {
        GameConfig.instance.SetLevelCurrent(GameConfig.instance.GetCurrentLevel().nameLevel + 1);
        GameContrler.instance.board.InitBoard();
        Hide();
    }
    public void Show2(object dta)
    {
        Show();

    }
    private void OnDestroy()
    {
        MyEvent.GameWin -= Show2;
    }
}