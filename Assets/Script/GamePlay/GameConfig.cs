using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{

    public static GameConfig instance; 
    public int levelCurent;
    public List<Level> levels;
    public Level GetLevel(int id)
    {
        return levels[id];
    }
    public Level GetLevelCurrent()
    {
        return levels[levelCurent];
    }
    public void SetLevelCurrent(int level)
    {
        levelCurent = level;
    }
    public int GetCurrentLevel()
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
