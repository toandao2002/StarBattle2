using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelCommon", menuName = "Data/LevelCommon", order = 1)]

[System.Serializable]
public class LevelCommon : ScriptableObject
{
    public List<SubLevel> levelEasy;
    public List<SubLevel> levelMedium;
    public List<SubLevel> levelDifficult;
    public List<SubLevel> levelGenius;
     
}
[System.Serializable]
public class SubLevel
{
    public string nameSubLevel;
    public bool isNeedBuy;
    public List<Level> levels;
    public int GetAmountLevel() { return levels.Count; }
}
