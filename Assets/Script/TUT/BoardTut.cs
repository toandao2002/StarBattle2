using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTut : MonoBehaviour
{
    public CellTut cellPref; 
    public List<List<CellTut>> cells;
    public int sizeBoardx;
    public int sizeBoardy;
    public GameObject content;
    public bool dontClick;
    List<Vector2Int> direction = new List<Vector2Int>() {
        new Vector2Int(0,-1),  //l
        new Vector2Int(-1,-1),  //l-t
        new Vector2Int(-1,0),//t
        new Vector2Int(-1,1),  //r -t
        new Vector2Int(0,1),//r
        new Vector2Int(1,1),  //r- b
        new Vector2Int(1,0),//b
        new Vector2Int(1,-1),  //b -l
    };
    TutorialData tutData;
    public void RemoveGarbage()
    {
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
    }
    public void Init(TutorialData tutData)
    {
        this.tutData = tutData;
        sizeBoardx = tutData.sizeBoard.x;
        sizeBoardy = tutData.sizeBoard.y;

        RemoveGarbage();
        cells = new List<List<CellTut>>();
        for (int i = 0; i < sizeBoardx; i++)
        {
            cells.Add(new List<CellTut>());
            for (int j = 0; j < sizeBoardy; j++)
            {
                CellTut cell = Instantiate(cellPref, content.transform);

                cell.ResetStatus();

                cells[i].Add(cell);
                cell.SetPos(new Vector2Int(i, j));
                cell.SetRegion(tutData.region[i* sizeBoardy + j]);
            }
        }
        foreach (List<CellTut> i in cells)
        {
            foreach (CellTut c in i)
            {

                Vector2Int pos = c.GetPos();
                CellTut l = GetCellByPos(pos.x, pos.y - 1);
                CellTut t = GetCellByPos(pos.x - 1, pos.y);
                CellTut r = GetCellByPos(pos.x, pos.y + 1);
                CellTut b = GetCellByPos(pos.x + 1, pos.y);
                c.InitCell(l, t, r, b, this);
                StatusCell _statusCell = StatusCell.None;
                 
                 
            }
        }
    }
    #region Check
    public CellTut GetCellByPos(int x, int y)
    {
        if (CheckPosCorect(x, y))
            return cells[x][y];
        return null;
    }
    public bool CheckPosCorect(int x, int y)
    {
        if (x < 0 || x >= sizeBoardx || y < 0 || y >= sizeBoardy)
            return false;
        return true;
    }

    public void CheckRow(int row)
    {
        int numDot = 0, numStar = 0;
        for (int i = 0; i < sizeBoardy; i++)
        {
            if (cells[row][i].statusCell == StatusCell.OneClick) // dot
            {
                numDot++;
            }
            else if (cells[row][i].statusCell == StatusCell.DoubleClick) // star
            {
                numStar++;
            }
        }
        if ((numDot >= sizeBoardy - 1  || numStar >= 3) && sizeBoardy>=3)
        {
            HighLightRow(row, false);
        }
        else
        {
            HighLightRow(row, true);
        }
    }
    public void CheckColumn(int column)
    {

        int numDot = 0, numStar = 0;
        for (int i = 0; i < sizeBoardx; i++)
        {
            if (cells[i][column].statusCell == StatusCell.OneClick) // dot
            {
                numDot++;
            }
            else if (cells[i][column].statusCell == StatusCell.DoubleClick) // star
            {
                numStar++;
            }
        }
        if ((numDot >= sizeBoardx - 1   || numStar >= 3) && sizeBoardx>=3)
        {
            HighLightColumn(column, false);
        }
        else
        {
            HighLightColumn(column, true);
        }
    }
    public bool CheckAround(Vector2Int pos)
    {
        bool hasError = false;
        int numError = 0;
        CellTut CellCurrent = cells[pos.x][pos.y];
        foreach (Vector2Int d in direction)
        {
            Vector2Int posNext = pos + d;
            if (!CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }
            CellTut cellNext = cells[posNext.x][posNext.y];
            if (cellNext.statusCell == CellCurrent.statusCell && CellCurrent.statusCell == StatusCell.DoubleClick)
            {
                cellNext.HightLightWrong(NameError.Cross);
                CellCurrent.HightLightWrong(NameError.Cross);
                hasError = true;
            }
            else
            {
                if (cellNext.errors.Contains(NameError.Cross))
                {
                    CellCurrent.ShowNormal(NameError.Cross);

                }
                cellNext.ShowNormal(NameError.Cross);
            }
        }
        return !hasError;


    }
    //use in hint 
    public int CountAndCheckCellEmptyAround(Vector2Int pos, List<Vector2Int> posEmpty)
    {
        bool hasError = false;
        int numCellEffect = 0;
        CellTut CellCurrent = cells[pos.x][pos.y];

        foreach (Vector2Int d in direction)
        {
            Vector2Int posNext = pos + d;
            if (!CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }
            CellTut cellNext = cells[posNext.x][posNext.y];
            if (cellNext.statusCell == StatusCell.DoubleClick)
            {

                return 100;
            }
            if (posEmpty.Contains(posNext))
            {
                numCellEffect += 1;
            }

        }
        return posEmpty.Count - numCellEffect;


    }

    public void CheckRegion(CellTut cell)
    {
        List<bool> check = new List<bool>(new bool[sizeBoardx * sizeBoardy]);
        Vector2Int numCell = CountCellEmpty(cell.pos, cell.region, check);
        check = new List<bool>(new bool[sizeBoardx * sizeBoardy]);
        if (numCell.x + numCell.y < 3) return;
        if ((numCell.x + numCell.y < 2 || numCell.y >= 3) )
        {
            HighLightRegion(cell.pos, cell.region, check, false);
        }
        else
        {
            HighLightRegion(cell.pos, cell.region, check, true);
        }
    }


    // x is cell smpty, y is  cell star
    public Vector2Int CountCellEmpty(Vector2Int pos, int region, List<bool> Check)
    {
        CellTut cellCurent = cells[pos.x][pos.y];
        int numCellEmpty = cellCurent.statusCell == StatusCell.None ? 1 : 0;
        int NumStar = cellCurent.statusCell == StatusCell.DoubleClick ? 1 : 0;
        Vector2Int result = new Vector2Int(numCellEmpty, NumStar);
        Check[pos.x * sizeBoardy + pos.y] = true;
        foreach (Vector2Int d in direction)
        {

            Vector2Int posNext = pos + d;
            if (!CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }


            CellTut cellNext = cells[posNext.x][posNext.y];
            if (Check[posNext.x * sizeBoardy + posNext.y] == false && cellNext.region == region)
            {
                result += CountCellEmpty(posNext, region, Check);
            }
        }
        return result;
    }
    public bool checkCorrect(Vector2Int pos)
    {
        foreach (Vector2Int i in tutData.posStar)
        {
            if (i == pos)
            {
                return true;
            }
        }
        return false;
    }
    public bool CheckWin()
    {
        if (sizeBoardx == sizeBoardy && sizeBoardy == 1) return false;
        int cntCorrect = 0;
        for (int i = 0; i < sizeBoardx; i++)
        {
            for (int j = 0; j < sizeBoardy; j++)
            {
                if (cells[i][j].statusCell == StatusCell.DoubleClick)
                {
                    if (!checkCorrect(cells[i][j].pos))
                    {
                        Debug.Log("ko dung");
                        return false;
                    }
                    else
                    {
                        cntCorrect++;
                    }
                }
            }
        }
        Debug.Log("Correct");
        return cntCorrect == tutData.posStar.Count;
    }
    #endregion
    #region show wrong
    public void HighLightRegion(Vector2Int pos, int region, List<bool> Check, bool correct)
    {
        Check[pos.x * sizeBoardy + pos.y] = true;
        if (correct)
        {
            cells[pos.x][pos.y].ShowNormal(NameError.Region);
        }
        else
        {
            cells[pos.x][pos.y].HightLightWrong(NameError.Region);
        }
        foreach (Vector2Int d in direction)
        {
            Vector2Int posNext = pos + d;
            if (!CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }
            CellTut cellNext = cells[posNext.x][posNext.y];
            if (Check[posNext.x * sizeBoardy + posNext.y] == false && cellNext.region == region)
            {
                HighLightRegion(posNext, region, Check, correct);

            }
        }

    }
    public void HighLightRow(int row, bool correct)
    {
        for (int i = 0; i < sizeBoardy; i++)
        {
            if (correct)
            {
                cells[row][i].ShowNormal(NameError.Row);
            }
            else
            {
                cells[row][i].HightLightWrong(NameError.Row);
            }
        }
    }

    public void HighLightColumn(int colunm, bool correct)
    {
        for (int i = 0; i < sizeBoardx; i++)
        {
            if (correct)
            {
                cells[i][colunm].ShowNormal(NameError.Column);
            }
            else
            {
                cells[i][colunm].HightLightWrong(NameError.Column);
            }
        }
    }
    #endregion
}
