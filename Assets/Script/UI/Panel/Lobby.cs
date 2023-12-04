using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : BasePopUP
{
    private TypeGame typeGame;

    public BoxPlayedRecently BoxPlayedRecentlyPref;
    public GameObject contennt;
    public List<BoxPlayedRecently> boxPlayedRecentlies;
    public List<BoxBigLevelHome> boxBigLevelHomes;
    public GameObject content;
    private void Start()
    {
        HandelData();
    }
    public void RemoveGarbage()
    {
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    public override void Show(object data = null, int dir = 1)
    {
        base.Show(data, dir);
        HandelData();
    }
    public void HandelData()
    {
        RemoveGarbage();
        DataLevelComon dataLevelComon = GameConfig.instance.GetDataLevelCommon();

        foreach (BoxBigLevelHome i in boxBigLevelHomes)
        {
            List<SubLevel> subLevels = GameConfig.instance.GetSubsLevelByTypeGame(i.typeGame);
            int numLevel = 0;
            foreach (var j in subLevels)
            {
                numLevel += j.GetAmountLevel();
            }
            print(numLevel+" "+ (float)dataLevelComon.GetLevelPassByTypeGame(i.typeGame));
            i.SetRateBarLevel((float)dataLevelComon.GetLevelPassByTypeGame(i.typeGame) / numLevel);
        }


        HistoryPlayed historyPlayed = null;
        try
        {
            historyPlayed = JsonUtility.FromJson<HistoryPlayed>(DataGame.GetDataJson(DataGame.History));

        }
        catch
        {
            Debug.Log("Dont find history");
        }
        if (historyPlayed != null)
        {
            for (int i = historyPlayed.historys.Count - 1; i >= 0; i--)
            {
                var obj = Instantiate(BoxPlayedRecentlyPref, contennt.transform);
                 
                try
                {
                    obj.SetData(historyPlayed.historys[i].typeGame.ToString() + "-" + historyPlayed.historys[i].nameLevel, historyPlayed.historys[i].datalevel.dayPlay,
                    historyPlayed.historys[i].datalevel.isfinished == true ? "Finished" : "Process", historyPlayed.historys[i].datalevel.timeFinish
                     , historyPlayed.historys[i].nameLevel, historyPlayed.historys[i].typeGame
                    );
                    boxPlayedRecentlies.Add(obj);
                }
                catch
                {
                    Debug.Log("Loi game gan day");
                }


            }
        }
    }
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
