using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataGame 
{
    public static string Level = "Level";
    public static string History = "History";


    public static string GetDataJson(string key)
    {
        return PlayerPrefs.GetString(key);
    }
    public static void SetDataJson(string key, string json)
    {
        PlayerPrefs.SetString(key, json);
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
}
[System.Serializable]
public class SettingData
{
    //public Dictionary<PushNotificationType, int> androidPnIndexes = new Dictionary<PushNotificationType, int>();
    /*
        public bool enablePn;
        public bool requestedPn;*/

    public bool haptic = true;
    public float soundVolume = 1;
    public float musicVolume = 1;

    public bool theme;

    //public bool iOsTrackingRequested;
}

[System.Serializable]
public class DataLevel
{
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
public class HistoryPlayed
{
    int maxSave = 5;
    public List<Level> historys; 
    public void AddDatalevel(Level dataLevel)
    {
        if(historys.Count >= maxSave)
        {
            historys.RemoveAt(0);
        }
        historys.Add(dataLevel);
    }
    public List<Level> GetHistorys()
    {
        return historys;
    }
}