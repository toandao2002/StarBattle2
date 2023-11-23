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
    
    
    public Level GetLevelCurrent()
    {
        return levelCurent;
    }
    public void SetLevelCurrent(int level)
    {
        string path = scr.GetPathLevel(typeGame,level);
        levelCurent = (Level)AssetDatabase.LoadAssetAtPath(path, typeof(Level));
         
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
