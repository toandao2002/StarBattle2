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

    public List<ParticleSystem> fx;
    public List<GameObject> PosFx;
    private void Awake()
    {
        instance = this;
        MyEvent.GameWin += EfWin;
    }
  
    public void Init()
    {
        for (int i = 0; i < 2; i++)
        { 
            fx[i].gameObject.SetActive(false);
           
        }
        historyActions = new List<HistoryAction>();
        ReActions = new List<HistoryAction>();
        tutorial = new Tutorial();
        if(!board.isModeMakeLevel)
        { 
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
        SaveData();
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
        DataLevelUser dataLevelComon = GameConfig.instance.GetDataLevelCommon();
        dataLevelComon.IncNumLevelPassInGame(GameConfig.instance.GetCurrentLevel().typeGame);
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
        if(hintMesage!= null)   
            hintMesage.ShowHint(board);
    }
    
    public void BackHome()
    {
        PopUpGameWin.instance.main.SetActive(false);
        SaveData();
        GameManger.instance.manageUi.HidePopUP(NamePopUp.GamePlay,-1);
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.Lobby,1,-1);
    }
    public void SaveDataBoardDontFinish()
    {
        try
        {
            DataOldBoardGame dataOldBoardGame = new DataOldBoardGame();
            int cnti = 0, cntj = 0;
            foreach (List<Cell> i in board.cells)
            {
                foreach (Cell j in i)
                {
                    dataOldBoardGame.cells.Add((int)j.statusCell);
                    cntj++;
                }
                cntj = 0;
                cnti++;
            }
            string json = Util.ConvertObjectToString(dataOldBoardGame);
            DataGame.SetDataJson(DataGame.OldBoard + GameConfig.instance.typeGame + GameConfig.instance.GetLevelCurrent().nameLevel, json);

        }
        catch
        {
            Debug.Log("Dont data");
        }
    }
    public void DeLeteOldDataBoardFinish()
    {
        DataGame.SetDataJson(DataGame.OldBoard + GameConfig.instance.typeGame + GameConfig.instance.GetLevelCurrent().nameLevel, "{}");
        DataGame.Save();
    }
    public void SaveData()
    {
        if (!board.isModeMakeLevel )
        {
            dataLevel.timeFinish = myTime.timeRun;
            string json = Util.ConvertObjectToString<DataLevel>(dataLevel);
            SaveDataBoardDontFinish();
            DataGame.SetDataJson(DataGame.Level + GameConfig.instance.typeGame + GameConfig.instance.GetLevelCurrent().nameLevel, json);
            DataGame.SetDataJson(DataGame.History, Util.ConvertObjectToString<HistoryPlayed>(historyPlayed));
            DataGame.SetDataJson(DataGame.DataLevelComon, Util.ConvertObjectToString(GameConfig.instance.dataLevelComon));
            DataGame.Save();
        }
    }
    
    public void EfWin(object obj)
    {
        for(int i = 0; i < 2; i++)
        {
            fx[i].transform.position = PosFx[i].transform.position;
            fx[i].gameObject.SetActive(true);
            fx[i].Play();
        }
    }
    

}
 