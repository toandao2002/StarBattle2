using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeGame
{
    Easy,
    Medium,
    Difficult,

}
[CreateAssetMenu(fileName = "Level", menuName = "Data/Level", order = 1)]

[System.Serializable]
public class Level : ScriptableObject
{
    public int nameLevel;
    public TypeGame typeGame;
    public DataBoard dataBoard;
    public void SetStarPos(Vector2Int posStar)
    {
        dataBoard.SetStar(posStar);
    }
    public void RemovePosStar(Vector2Int posStar)
    {
        dataBoard.RemovePosStar(posStar);
    }
}
