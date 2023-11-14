using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HistoryAction 
{
    public Cell cell;
    public StatusCell statusCell;
    public HistoryAction(Cell cell, StatusCell statusCell)
    {
        this.cell = cell;
        this.statusCell = statusCell;
    }

}
