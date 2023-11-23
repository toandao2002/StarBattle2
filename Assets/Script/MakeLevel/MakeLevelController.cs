using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEditor;

public class MakeLevelController : MonoBehaviour
{
    
    public static MakeLevelController instance;
    public TMP_InputField levelInput;
    public TMP_InputField regionInput;
    public TMP_Dropdown typeLevel;
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
    public void SetModeSetRegion()
    {
        modeSetRegion = true;
        txtBtnMode.text = "Set Region";
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
        levelCurrent = (Level)AssetDatabase.LoadAssetAtPath(scr.GetPathLevel((TypeGame)typeLevel.value, GetLevelInt()), typeof(Level));
        if (levelCurrent == null)
        {
            Debug.Log("Case create new level");
            DataBoard databoard = new DataBoard(9,"","");
            levelCurrent = new Level(Int32.Parse(levelInput.text) );
            levelCurrent.dataBoard = databoard;
        }
        else
        {
             
            
        }
           
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
        List<Vector2Int> posCorrectStar = GameConfig.instance.GetCurrentLevel().dataBoard.posCorrectStar;
        DataBoard dataBoard = new DataBoard(9, rg, posStar,posCorrectStar);
        levelCurrent.dataBoard = dataBoard;
        scr.CreateOrReplaceAsset(levelCurrent, scr.GetPathLevel((TypeGame)typeLevel.value,  GetLevelInt()));
    }
    public void NextRegion()
    {
        int num = Int32.Parse(regionInput.text)+1;
        if (num > 9) num = 1;
        regionInput.text = (num )+"";
    }
    public void BackRegion()
    {
        int num = Int32.Parse(regionInput.text)-1;
        if (num < 1) num = 9;

        regionInput.text = (num) + "";
    }

}
