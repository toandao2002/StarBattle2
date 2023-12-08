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

    //  first in game show tut
    public static string FTurtorial = "FTurtorial";

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
        if(historys.Count >= maxSave)
        {
            historys.RemoveAt(0);
        }
        LevelHistoryPlay level = new LevelHistoryPlay(dataLevel.nameLevel, dataLevel.typeGame, dataLevel.datalevel.dayPlay, dataLevel.datalevel.isfinished, dataLevel.datalevel.timeFinish); ;
        historys.Add(level);
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