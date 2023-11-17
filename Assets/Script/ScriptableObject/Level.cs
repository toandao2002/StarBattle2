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

public class Level : ScriptableObject
{
    public TypeGame typeGame;
    public DataBoard dataBoard;
   
}
