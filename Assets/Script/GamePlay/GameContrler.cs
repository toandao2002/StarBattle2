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

   
    private void Awake()
    {
        instance = this;
        
    }
    public void Init()
    {
        historyActions = new List<HistoryAction>();
        ReActions = new List<HistoryAction>();
        tutorial = new Tutorial();
    }
    // Start is called before the first frame update
    void Start()
    {
        MyEvent.GameWin += GameWin;
    }
    public void ResetNewGame()
    {
        Init();
    }
    public void GameWin() {
        Debug.Log("Game Win");
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
    }
}
 