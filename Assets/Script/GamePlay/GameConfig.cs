using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameConfig : MonoBehaviour
{

    public static GameConfig instance;  
    public Level levelCurent;
    public TypeGame typeGame;
    public ScriptableObjectController scr;
    public LevelCommon levelCommon;
    public int timeFinishPlay;
    
    public void SetTypeGame(TypeGame typeGame)
    {
        this.typeGame = typeGame;
    }
    public List<SubLevel> GetSubsLevelByTypeGame( )
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
    public Level GetLevelCurrent()
    {
        return levelCurent;
    }
    public void SetLevelCurrent(int level)
    {
        string path = scr.GetPathLevel(typeGame,level);
        levelCurent = (Level)AssetDatabase.LoadAssetAtPath(path, typeof(Level));
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
        if(instance == null)
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
        
    }
}
