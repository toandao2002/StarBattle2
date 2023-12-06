using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLevelUser 
{
    public List<int> numLevelPass;
    public DataLevelUser()
    {
        numLevelPass = new List<int>() { 0, 0, 0, 0 };
    }
    public int GetLevelPassByTypeGame(TypeGame typeGame)
    {
        int id = (int)typeGame;
        return numLevelPass[id];
    }
    public void IncNumLevelPassInGame(TypeGame typeGame)
    {

        int id = (int)typeGame;
        numLevelPass[id]+=1;
    }
    public void DecNumLevelPassInGame(TypeGame typeGame)
    {

        int id = (int)typeGame;
        numLevelPass[id] -= 1;
    }
    public bool CheckLevelFinish(TypeGame typeGame, int level)
    {
        int id = (int)typeGame;

        return level -1 <= numLevelPass[id];
    }
}
