using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingData 
{
    public bool vibrate = true;
    public float soundVolume = 1;
    public float musicVolume = 1;

    public bool theme;
    public static SettingData GetSetting()
    {
        SettingData settingData = Util.ConvertStringToObejct<SettingData>(DataGame.GetDataJson(DataGame.SettingData));
        if(settingData == null)
        {
            settingData = new SettingData();
        }
        return settingData;
    }
    public void SaveSetingData()
    {
        string json = Util.ConvertObjectToString(this);
        DataGame.SetDataJson(DataGame.SettingData, json);
        MyEvent.UpdateSetingData?.Invoke();
    }
    public void TurnSound()
    {
        soundVolume = Mathf.Abs(1 - soundVolume);
        SaveSetingData();
    }
    public void TurnMusic()
    {
        musicVolume = Mathf.Abs(1 - musicVolume);
        SaveSetingData();
    }
    public void Vibration()
    {
        vibrate = !vibrate;
        SaveSetingData();
    }
}
