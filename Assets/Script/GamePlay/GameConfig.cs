using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[DefaultExecutionOrder(-1)]
public class GameConfig : MonoBehaviour
{

    public static GameConfig instance;  
    public Level levelCurent;
    public TypeGame typeGame; 
    public LevelCommon levelCommon;
    public int timeFinishPlay; 
    public DataLevelUser dataLevelComon;
    public string nameModePlay;

    public NameTheme nameTheme;
    public Theme darkMode;
    public Theme lightMode;
    public void SetTypeGame(TypeGame typeGame)
    {
        this.typeGame = typeGame;
    }
    public List<SubLevel> GetCurrentSubs( )
    {
        switch (typeGame)
        {
            case TypeGame.Easy:
                return levelCommon.levelEasy;
            case TypeGame.Medium:
                return levelCommon.levelMedium;
            case TypeGame.Difficult:
                return levelCommon.levelDifficult;
            case TypeGame.Genius:
                return levelCommon.levelGenius;
        }

        return null;
    }
    public List<SubLevel> GetSubsLevelByTypeGame(TypeGame MytypeGame)
    {
        switch (MytypeGame)
        {
            case TypeGame.Easy:
                return levelCommon.levelEasy;
            case TypeGame.Medium:
                return levelCommon.levelMedium;
            case TypeGame.Difficult:
                return levelCommon.levelDifficult;
            case TypeGame.Genius:
                return levelCommon.levelGenius;
        }

        return null;
    }
    public Level GetLevelCurrent()
    {
        return levelCurent;
    }
    public string GetPathLevel(TypeGame typeGame, int level)
    {
        string path = "GameConfig/" + typeGame.ToString() + "/Level " + level + "";
        return path;
    }
    public void SetLevelCurrent(int level )
    {
        string path =  GetPathLevel(typeGame,level); 
        levelCurent = Resources.Load<Level>(path);
    }
    public void SetTimeFiishCurrent(int timeT)
    {
        timeFinishPlay = timeT;
    }
    public void SetLevelCurrentMakeLevel(Level level)
    {
        levelCurent = level;
    }
    public Level GetCurrentLevel()
    {
        return levelCurent;
    }
     
    private void Awake()
    {
        Debug.Log("a");

        if (instance == null)
        {
            instance = this;
             
        }
        nameTheme = SettingData.GetSetting().theme;
        
        
    }
    public DataLevelUser GetDataLevelCommon()
    {
        
        if (dataLevelComon == null)
        {
            dataLevelComon = Util.ConvertStringToObejct<DataLevelUser>(DataGame.GetDataJson(DataGame.DataLevelComon));
            if(dataLevelComon == null)  
                dataLevelComon = new DataLevelUser();

        }
        return dataLevelComon;
    }

    
}
public enum TextColor
{
    Black,
    Gray,
    White,
}
 
