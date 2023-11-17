using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MakeLevelController : MonoBehaviour
{
    
    public static MakeLevelController instance;
    public TMP_InputField levelInput;
    public TMP_InputField regionInput;
    public bool modeSetRegion;
    public TMP_Text txtBtnMode;
    public Level levelCurrent;
    int levelInt = -1;
    public ScriptableObjectController scr;
    public Board board;

    private void Start()
    {
        modeSetRegion = true;
    }
    public int GetLevelInt()
    { 
        levelInt = Int32.Parse(levelInput.text);
        return levelInt;
    }
    public int GetRegion()
    {
        return Int32.Parse(regionInput.text);
    }
    public void ChangeModeSetUp()
    {
        modeSetRegion = !modeSetRegion;
        if (modeSetRegion)
        {
            txtBtnMode.text = "Set Region";
        }
        else
        {
            txtBtnMode.text = "Set Star";
        }
    }
    public Level GetLevel()
    {
        if (GetLevelInt() == 0) {
            Debug.Log("level must greater 0");
            levelInput.text = "1";
        }
        if(GetLevelInt()>= GameConfig.instance.levels.Count)
        {
            Debug.Log("Case create new level");
            DataBoard databoard = new DataBoard(9,"","");
            levelCurrent = new Level();
            levelCurrent.dataBoard = databoard;
        }
        else 
            levelCurrent = GameConfig.instance.levels[GetLevelInt()-1];
        return levelCurrent;
    }
    private void Awake()
    {
        instance = this;
    }
    public void SaveLevel()
    {
        String rg="", posStar = "";
        for (int i = 0; i < board.sizeBoard; i++)
        { 
            for (int j = 0; j < board.sizeBoard; j++)
            {
                if (board.cells[i][j].statusCell == StatusCell.DoubleClick)
                {
                    posStar += (i + " " + j + " ");
                }
                rg += (board.cells[i][j].region + " ");
            }
        }
        DataBoard dataBoard = new DataBoard(9, rg, posStar);
        levelCurrent.dataBoard = dataBoard;
        scr.CreateOrReplaceAsset(levelCurrent, scr.GetPathLevel(GetLevelInt()));
    }

}
