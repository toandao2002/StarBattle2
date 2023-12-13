using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public  class LevelFinished
{
    public List<int> level;
    public LevelFinished()
    {
        level = new List<int>();
    }
}
[System.Serializable]
public class DataLevelUser 
{
    [SerializeField]
    public List<LevelFinished> numLevelPass;
    public DataLevelUser()
    {
        numLevelPass = new List<LevelFinished>(){
            new LevelFinished(),
            new LevelFinished(),
            new LevelFinished(),
            new LevelFinished()
        };
    }
    public int GetLevelPassByTypeGame(TypeGame typeGame)
    {
        int id = (int)typeGame;
        return numLevelPass[id].level.Count;
    }
    public void IncNumLevelPassInGame(TypeGame typeGame,int idLevel)
    {

        int id = (int)typeGame;
        if (!numLevelPass[id].level.Contains(idLevel)  )    
            numLevelPass[id].level.Add(idLevel);
    }
    public void DecNumLevelPassInGame(TypeGame typeGame)
    {

   
    }
    public bool CheckLevelFinish(TypeGame typeGame, int level)
    {
        int id = (int)typeGame;

        return level -1 <= numLevelPass[id].level.Count;
    }
}
