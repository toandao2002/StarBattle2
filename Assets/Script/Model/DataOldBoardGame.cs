
using System.Collections.Generic;
using UnityEngine;
 
public class DataOldBoardGame
{ 
    public List<int> cells = new List<int>();
    public StatusCell GetStatus(Vector2Int pos)
    {
        
        int x = pos.x;
        int y = pos.y;
        return (StatusCell) cells[x*9+y];
    }
}
