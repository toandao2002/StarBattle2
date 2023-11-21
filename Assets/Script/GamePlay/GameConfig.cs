using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{

    public static GameConfig instance;  
    public List<Level> levels;
    public Level levelCurent;
    public Level GetLevel(int id)
    {
        return levels[id];
    }
    public Level GetLevelCurrent()
    {
        return levelCurent;
    }
    public void SetLevelCurrent(int level)
    {
        levelCurent = levels[level];
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
