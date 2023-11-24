using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameContrler : MonoBehaviour
{
    public static GameContrler instance;
    public List<HistoryAction> historyActions; 
    public List<HistoryAction> ReActions; 
    public Tutorial tutorial;
    public Board board;
    public MyTime myTime;
    public DataLevel dataLevel;
    HistoryPlayed historyPlayed = null;
    private void Awake()
    {
        instance = this;
        
    }
  
    public void Init()
    {
        historyActions = new List<HistoryAction>();
        ReActions = new List<HistoryAction>();
        tutorial = new Tutorial();
        if(!board.isModeMakeLevel)
        {
            myTime.StopAllCoroutines();
            if (!GameConfig.instance.GetCurrentLevel().datalevel.isfinished)  
                myTime.CountTime(GameConfig.instance.timeFinishPlay);
            else
            {
                myTime.SetTime(GameConfig.instance.timeFinishPlay);
            }
            dataLevel = GameConfig.instance.GetCurrentLevel().datalevel;
            dataLevel.dayPlay = System.DateTime.Today.ToString();
            UpdateHistoryPlayed();
        }
    }
    public void UpdateHistoryPlayed()
    {
        
        try
        {

           historyPlayed  = JsonUtility.FromJson<HistoryPlayed>( DataGame.GetDataJson(DataGame.History));
        }
        catch(Exception e) 
        {

        }
        if(historyPlayed == null)
        {
            historyPlayed = new HistoryPlayed();
            historyPlayed.historys = new List<Level>();
            historyPlayed.AddDatalevel(GameConfig.instance.GetCurrentLevel());
        }
        else
        {
            historyPlayed.AddDatalevel(GameConfig.instance.GetCurrentLevel());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MyEvent.GameWin += GameWin;
       
    }
    private void OnDestroy()
    {
        MyEvent.GameWin -= GameWin;
    }

    public void ResetNewGame()
    {
        Init();
    }
    public void gamewin()
    {
        GameWin(null);
    }
    public void GameWin(object obj) {
        Debug.Log("Game Win");
        dataLevel.isfinished = true;
        SaveData();
        myTime.StopAllCoroutines();
    }
    public void AddAction(HistoryAction  history)
    {
        historyActions.Add(history);
    }
    public void Undo()
    {
        if(historyActions .Count != 0)
        {
            HistoryAction history = historyActions[historyActions.Count - 1];
            ReActions.Add(history);
            historyActions.RemoveAt(historyActions.Count - 1);
            if(history.statusCell == StatusCell.None)
            {
                history.cell.DonClick(false);
            }
            else if (history.statusCell == StatusCell.OneClick)
            {
                history.cell.OneClick(false);
            }
            else if(history.statusCell== StatusCell.DoubleClick)
            {
                history.cell.DoubleClick(false);
            }
        }
    }
    public void Redo()
    {
        if (ReActions.Count != 0)
        {
            HistoryAction history = ReActions[ReActions.Count - 1];
            historyActions.Add(history);
            ReActions.RemoveAt(ReActions.Count - 1);
            Debug.Log(history.statusCell);
            if (history.statusCell == StatusCell.None)
            {
                history.cell.DonClick(true);
            }
            else if (history.statusCell == StatusCell.OneClick)
            {
                history.cell.OneClick(true);
            }
            else if (history.statusCell == StatusCell.DoubleClick)
            {
                history.cell.DoubleClick(true);
            }
        }
    }
    public void Hint()
    {
        HintMesage  hintMesage= tutorial.Hint(board);
        hintMesage.ShowHint(board);
    }
    public void BackHome()
    {
        GameManger.instance.LoadScene("Home");
        SaveData();
    }
    public void SaveData()
    {
        if (!board.isModeMakeLevel )
        {
            dataLevel.timeFinish = myTime.timeRun;
            string json = Util.ConvertObjectToString<DataLevel>(dataLevel);
            DataGame.SetDataJson(DataGame.Level + GameConfig.instance.typeGame + GameConfig.instance.GetLevelCurrent().nameLevel, json);
            DataGame.SetDataJson(DataGame.History, Util.ConvertObjectToString<HistoryPlayed>(historyPlayed));
            DataGame.Save();
        }
    }
}
 