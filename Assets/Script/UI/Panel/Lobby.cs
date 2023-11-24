using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : BasePopUP
{
    private TypeGame typeGame;

    public BoxPlayedRecently BoxPlayedRecentlyPref;
    public GameObject contennt;
    public List<BoxPlayedRecently> boxPlayedRecentlies;

    public  void Start()
    {
        HistoryPlayed historyPlayed = null;
        try
        {
             historyPlayed = JsonUtility.FromJson<HistoryPlayed>(DataGame.GetDataJson(DataGame.History));

        }
        catch
        {
            Debug.Log("Dont find history");
        }
        if(historyPlayed!= null)
        {
            for (int i = historyPlayed.historys.Count -1; i>= 0; i --)
            {
                var obj = Instantiate(BoxPlayedRecentlyPref, contennt.transform);
                obj.SetData(historyPlayed.historys[i].typeGame.ToString()+"-"+ historyPlayed.historys[i].nameLevel, historyPlayed.historys[i].datalevel.dayPlay,
                    historyPlayed.historys[i].datalevel.isfinished== true?"Finished": "Process",historyPlayed.historys[i].datalevel.timeFinish
                     ,historyPlayed.historys[i].nameLevel, historyPlayed.historys[i].typeGame
                    );
                boxPlayedRecentlies.Add(obj);
            }
        }
    }
    public void SetActionForBtnTitle()
    {
        TittleUI.instacne.SetActionIconLeft(() => {
            TittleUI.instacne.ShowTittle(NamePopUp.Lobby, "Home");
            GameManger.instance.manageUi.ShowPopUp(NamePopUp.Lobby,1,-1);
            GameManger.instance.manageUi.HidePopUP(NamePopUp.ChoseLevel1,-1);
        });
    }
    public void OpenLevelEasy()
    {
        typeGame = TypeGame.Easy;
        TittleUI.instacne.ShowTittle(NamePopUp.ChoseLevel1, "Beginer");
        SetActionForBtnTitle();
        HandleChangePanel();
    }
    public void OpenLevelMedium()
    {
        typeGame = TypeGame.Medium;
        TittleUI.instacne.ShowTittle(NamePopUp.ChoseLevel1, "Medium");
        SetActionForBtnTitle();
        HandleChangePanel();


    }
    public void OpenLevelDifficut()
    {

        typeGame = TypeGame.Difficult;
        TittleUI.instacne.ShowTittle(NamePopUp.ChoseLevel1, "Difficult");
        SetActionForBtnTitle();
        HandleChangePanel();

    }
    public void OpenLevelGenius()
    {
        TittleUI.instacne.ShowTittle(NamePopUp.ChoseLevel1, "Genius");
        typeGame = TypeGame.Genius;
        SetActionForBtnTitle();
        HandleChangePanel();

    }
    public void HandleChangePanel()
    {
        GameConfig.instance.SetTypeGame(typeGame);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.ChoseLevel1);
        Hide();
    }
}
