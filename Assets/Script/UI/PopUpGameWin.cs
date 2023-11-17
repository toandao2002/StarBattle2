using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpGameWin : BasePopUP
{
    private void Start()
    {
        MyEvent.GameWin += Show;
    }
    public void NextLevel()
    {
        GameConfig.instance.SetLevelCurrent(GameConfig.instance.GetCurrentLevel() + 1);
        GameContrler.instance.board.InitBoard();
        Hide();
    }
    private void OnDestroy()
    {
        MyEvent.GameWin -= Show;
    }
}
