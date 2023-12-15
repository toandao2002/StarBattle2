using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataGame 
{
    public static string Level = "Level";
    public static string OldBoard = "OldBoard";
    public static string History = "History";
    public static string DataLevelComon = "DataLevelComon";
    public static string SettingData = "SettingData";
    public static string AmountHint = "AmountHint";


    //
    public static string ShowFirtSub = "ShowFirtSub";
    public static string FirstPlayGame = "FirstPlayGame";
    //

    //  first in game show tut
    public static string FTurtorial = "FTurtorial";

    // shop
    public static string Datapack = "Datapack";

    public static string GetDataJson(string key)
    {
        return PlayerPrefs.GetString(key);
    }
    public static void SetDataJson(string key, string json)
    {
        PlayerPrefs.SetString(key, json);
    }
    
    public static int GetInt (string key)
    {
        return PlayerPrefs.GetInt(key);
    }
    public static bool CheckContain(string key)
    {
        return PlayerPrefs.HasKey(key);
    }
    public static void SetInt(string key, int val)
    {
        PlayerPrefs.SetInt(key, val);
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }
    public static void resetData()
    {
        PlayerPrefs.DeleteAll();
    }
    public static DataLevel GetDataLevel(TypeGame typeGame, int level)
    {
        string json =  PlayerPrefs.GetString(Level +typeGame + level);
        DataLevel dataLevel = JsonUtility.FromJson<DataLevel>(json);
        return dataLevel;
    }
    public static DataOldBoardGame GetDataOldBoardGame(TypeGame typeGame, int level)
    {
        string json = PlayerPrefs.GetString(OldBoard + typeGame + level);
        DataOldBoardGame dataOldBoardGame = JsonUtility.FromJson<DataOldBoardGame>(json);
        return dataOldBoardGame;
    }

     
}
 

[System.Serializable]
public class DataLevel
{
    [SerializeField]
    public string dayPlay;
    public bool isfinished;
    public int timeFinish;
    public DataLevel (string dayPlay ,bool isfinished, int timeFinish)
    {
        this.dayPlay = dayPlay;
        this.isfinished = isfinished;
        this.timeFinish = timeFinish;

    }
    public DataLevel( bool isfinished, int timeFinish)
    { 
        this.isfinished = isfinished;
        this.timeFinish = timeFinish;

    }
}
[Serializable]
public class LevelHistoryPlay
{
    public int nameLevel;
    public TypeGame typeGame;
    public string dayPlay;
    public bool isfinished;
    public int timeFinish;
    public LevelHistoryPlay(int nameLevel, TypeGame typeGame, string dayPlay, bool isFinished, int timeFinish)
    {
        this.nameLevel = nameLevel;
        this.typeGame = typeGame;
        this.dayPlay = dayPlay;
        this.isfinished = isFinished;
        this.timeFinish = timeFinish;

    }
}
[System.Serializable]
public class HistoryPlayed
{
    int maxSave = 30;
    [SerializeField]
    public List<LevelHistoryPlay> historys;
    public void AddDatalevel(Level dataLevel)
    {
        if (historys == null) historys = new List<LevelHistoryPlay>();
        if (historys.Count >= maxSave)
        {
            historys.RemoveAt(0);
        }
        LevelHistoryPlay level = new LevelHistoryPlay(dataLevel.nameLevel, dataLevel.typeGame, dataLevel.datalevel.dayPlay, dataLevel.datalevel.isfinished, dataLevel.datalevel.timeFinish); ;
        LevelHistoryPlay levelhistory = CheckLevelHasBeenExist(dataLevel);

        if (historys == null)
        {
            historys.Add(level);
            
        }
        else
        {
            historys.Remove(levelhistory);
            historys.Add(level);
        }
    }
    public LevelHistoryPlay CheckLevelHasBeenExist(Level dataLevel)
    {
        foreach (LevelHistoryPlay i in historys)
        {
            if(i.typeGame == dataLevel.typeGame && i.nameLevel == dataLevel.nameLevel)
            {
                return i;
            }
        }
        return null;
    }
    public List<LevelHistoryPlay> GetHistorys()
    {
        return historys;
    }
    public override string ToString()
    {
        return base.ToString();
    }

  
}

