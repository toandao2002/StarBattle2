using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : BasePopUP
{
    private TypeGame typeGame;
    public void OpenLevelEasy()
    {
        typeGame = TypeGame.Easy;
        HandleChangePanel();
    }
    public void OpenLevelMedium()
    {
        typeGame = TypeGame.Medium;
        HandleChangePanel();


    }
    public void OpenLevelDifficut()
    {

        typeGame = TypeGame.Difficult;
        HandleChangePanel();

    }
    public void OpenLevelGenius()
    {
        typeGame = TypeGame.Genius;
        HandleChangePanel();

    }
    public void HandleChangePanel()
    {
        GameConfig.instance.SetTypeGame(typeGame);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel1);
        Hide();
    }
}
