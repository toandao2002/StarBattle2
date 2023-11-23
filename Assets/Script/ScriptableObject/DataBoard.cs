using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DataBoard 
{
    public int size;
    public string dataRegion;
    public string dataPosStar;
    public List<List<int>> subRegions;
    
    public List<Vector2Int> posCorrectStar;
    public DataBoard(int size, String dataRegion, String dataPosStar )
    {
        this.size = size;
        this.dataRegion = dataRegion;
        this.dataPosStar= dataPosStar;
    }
    public DataBoard(int size, String dataRegion, String dataPosStar, List<Vector2Int> posCorrectStar )
    {
        this.size = size;
        this.dataRegion = dataRegion;
        this.dataPosStar = dataPosStar;
        this.posCorrectStar = new List<Vector2Int>();
        this.posCorrectStar.AddRange(posCorrectStar);
    }
    public void InitData()
    {
        subRegions = new List<List<int>>();
      
        string[] split = System.Text.RegularExpressions.Regex.Split(dataRegion.Trim(), @" +");
        if(split.Length == 1)
        {
            for (int i = 0; i < size; i++)
            {
                subRegions.Add(new List<int>());
                for (int j = 0; j < size; j++)
                {
                    subRegions[i].Add(0);

                }
            }
        }
        else
        {
            for (int i = 0; i < size; i++)
            {
                subRegions.Add(new List<int>());
                for (int j = 0; j < size; j++)
                {
                    subRegions[i].Add(Int32.Parse(split[i * size + j]));

                }
            }

        }


    }
    public void InitPos()
    {/*
        dataPosStar = "0 2 0 4 1 0 1 6 2 4 2 8 3 1 3 6 4 3 4 8 5 1 5 5 6 3 6 7 7 0 7 5 8 2 8 7";*/
        string[] split = System.Text.RegularExpressions.Regex.Split(dataPosStar.Trim(), @" +");
        posCorrectStar = new List<Vector2Int>();
        for (int i = 0; i < split.Length -1; i+=2)
        {
            int x = Int32.Parse(split[i ]);
            int y = Int32.Parse(split[i  + 1]);
            posCorrectStar.Add(new Vector2Int(x, y)); 
        }
    }
    public int GetRegion(int x, int y)
    {
        if (subRegions == null|| subRegions.Count ==0) InitData();
       //
        return subRegions[x][y];
         
    }
    public void  Show()
    {
        foreach(List<int> i in subRegions)
        {
            String s = "";
            foreach(int j in i)
            {
                s += (j + " ");
            }
            Debug.Log(s);
        }
    }
    public bool CheckPosCorrectStar(Vector2Int pos)
    {
        if (posCorrectStar == null)
        { 
            InitPos();
        } 
        foreach (Vector2Int i in posCorrectStar)
        {
            if(i == pos)
            {
                return true;
            }
        }
        return false;
    }
    public void SetStar(Vector2Int posStar)
    {
        if(!posCorrectStar.Contains(posStar))
        {
            posCorrectStar.Add(posStar);
        }
    }
    public void RemovePosStar(Vector2Int posStar)
    {
        posCorrectStar.Remove(posStar);
    }
    public void ResetData()
    {
        this.dataRegion = "";
        this.dataPosStar = "";
        this.posCorrectStar = new List<Vector2Int>();
        this.subRegions = new List<List<int>>();
    }
}
 