using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public MyTime myTime;
    public TittleUI title;
    public List<Cell> cellsRaw;
    public List<List<Cell>> cells;
    public int sizeBoard;
    public bool isModeMakeLevel;
    public bool isFinish;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (isModeMakeLevel)
        {
            InitBoardMakeLevel(false);
        }
        else
        {
            InitBoard();
        }
    }
    public void InitBoardMakeLevel(bool isClear)
    {
        MakeLevelController.instance.SetModeSetRegion();
        GameContrler.instance.ResetNewGame();
        if (isClear)
        {
            MakeLevelController.instance.GetLevel().dataBoard.ResetData();
        }
        GameConfig.instance.SetLevelCurrentMakeLevel(MakeLevelController.instance.GetLevel());
        title.SetLevelTittle(GameConfig.instance.nameModePlay +" "+GameConfig.instance.GetCurrentLevel().nameLevel);
        cells = new List<List<Cell>>();

        for (int i = 0; i < sizeBoard; i++)
        {
            cells.Add(new List<Cell>());
            for (int j = 0; j < sizeBoard; j++)
            {
                Cell cell = cellsRaw[i * sizeBoard + j];
                cell.ResetStatus();

                cells[i].Add(cell);
                cell.SetPos(new Vector2Int(i, j)); 
                cell.SetRegion(MakeLevelController.instance.GetLevel().dataBoard.GetRegion(i, j)); 
                 

            }
        }
        foreach (List<Cell> i in cells)
        {
            foreach (Cell c in i)
            {

                Vector2Int pos = c.GetPos();
                Cell l = GetCellByPos(pos.x, pos.y - 1);
                Cell t = GetCellByPos(pos.x - 1, pos.y);
                Cell r = GetCellByPos(pos.x, pos.y + 1);
                Cell b = GetCellByPos(pos.x + 1, pos.y);
                c.InitCell(l, t, r, b, this);
                
            }
        }
    }
    public void ReStart()
    {
        GameContrler.instance. DeLeteOldDataBoardFinish();
        if (isFinish == true)
        {
            DataLevelComon dataLevelComon = GameConfig.instance.GetDataLevelCommon();
            dataLevelComon.DecNumLevelPassInGame(GameConfig.instance.GetCurrentLevel().typeGame);
        }
        isFinish = false;
        GameConfig.instance.GetCurrentLevel().ReStart();
        GameConfig.instance.timeFinishPlay = 0;
        GameContrler.instance.ResetNewGame();
        InitBoard();
    }
    public void ResetTimer()
    {
        myTime.StopAllCoroutines();
        if (!GameConfig.instance.GetCurrentLevel().datalevel.isfinished)
            myTime.CountTime(GameConfig.instance.timeFinishPlay);
        else
        {
            myTime.SetTime(GameConfig.instance.timeFinishPlay);
        }
    }
    DataOldBoardGame dataOldBoardGame ;
    public  void UpdateOldDataStaus()
    {
        if(!isFinish)    
           dataOldBoardGame = DataGame.GetDataOldBoardGame(GameConfig.instance.typeGame, GameConfig.instance.GetLevelCurrent().nameLevel);

    }
    public void InitBoard()
    {
        UpdateOldDataStaus();
        ResetTimer();
       
        isFinish = GameConfig.instance.GetCurrentLevel().datalevel.isfinished;
        GameContrler.instance.ResetNewGame();
        title.SetLevelTittle(GameConfig.instance.nameModePlay+ " "+ GameConfig.instance.GetCurrentLevel().nameLevel);
        cells = new List<List<Cell>>();

        for (int i = 0; i < sizeBoard; i++)
        {
            cells.Add(new List<Cell>());
            for (int j = 0; j < sizeBoard; j++)
            {
                Cell cell = cellsRaw[i * sizeBoard + j];
                 
                cell.ResetStatus();

                cells[i].Add(cell);
                cell.SetPos(new Vector2Int(i, j));

                cell.SetRegion(GameConfig.instance.GetLevelCurrent().dataBoard.GetRegion(i,j)); ;
                if(dataOldBoardGame!= null&& dataOldBoardGame.cells!= null&&dataOldBoardGame.cells.Count !=0 &&!isFinish)
                {
                     
                        cell.statusCell =dataOldBoardGame.GetStatus(cell.pos);

                    
                     
                    
                }
            }
        }
        StartCoroutine(delay());
    }

    IEnumerator delay()
    {
        yield return null;
        foreach (List<Cell> i in cells)
        {
            foreach (Cell c in i)
            {

                Vector2Int pos = c.GetPos();
                Cell l = GetCellByPos(pos.x, pos.y - 1);
                Cell t = GetCellByPos(pos.x - 1, pos.y);
                Cell r = GetCellByPos(pos.x, pos.y + 1);
                Cell b = GetCellByPos(pos.x + 1, pos.y);
                c.InitCell(l, t, r, b, this);
                if(!isFinish)
                    c.ShowStatusCell();
            }
        }
    }
    #region  check
    public void CheckRow(int row)
    {
        int numDot = 0, numStar = 0;
        for (int i = 0; i < sizeBoard; i++)
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
        if (numDot >= sizeBoard -1|| numStar >= 3)
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
        for (int i = 0; i < sizeBoard; i++)
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
        if (numDot >= sizeBoard -1 || numStar >= 3)
        {
            HighLightColumn(column,false);
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
        Cell CellCurrent = cells[pos.x][pos.y];
        foreach (Vector2Int d in direction)
        {
            Vector2Int posNext = pos + d;
            if(!CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }
            Cell cellNext = cells[posNext.x][posNext.y];
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
    public int CountAndCheckCellEmptyAround(Vector2Int pos,List<Vector2Int> posEmpty)
    {
        bool hasError = false;
        int numCellEffect = 0;
        Cell CellCurrent = cells[pos.x][pos.y];
        
        foreach (Vector2Int d in direction)
        {
            Vector2Int posNext = pos + d;
            if (!CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }
            Cell cellNext = cells[posNext.x][posNext.y];
            if (  cellNext.statusCell == StatusCell.DoubleClick)
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

    public void CheckRegion(Cell cell)
    {
        List<bool> check = new List<bool>(new bool[sizeBoard * sizeBoard]);
        Vector2Int numCell = CountCellEmpty(cell.pos, cell.region, check);
        check = new List<bool>(new bool[sizeBoard * sizeBoard]);

        if (numCell.x  + numCell.y< 2  || numCell.y>=3)
        {
            HighLightRegion(cell.pos, cell.region, check, false);
        }
        else
        {
            HighLightRegion(cell.pos, cell.region, check, true);
        }
    }


    // x is cell smpty, y is  cell star
    public Vector2Int CountCellEmpty(Vector2Int pos, int region,List<bool> Check )
    {
        Cell cellCurent = cells[pos.x][pos.y];
        int numCellEmpty = cellCurent.statusCell == StatusCell.None?1:0 ;
        int NumStar = cellCurent.statusCell == StatusCell.DoubleClick ? 1 : 0;
        Vector2Int result = new Vector2Int(numCellEmpty, NumStar);
        Check[pos.x * sizeBoard + pos.y] = true;
        foreach (Vector2Int d in direction)
        {
           
            Vector2Int posNext = pos + d;
            if (!CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }
          

            Cell cellNext = cells[posNext.x][posNext.y];
            if (Check[posNext.x * sizeBoard + posNext.y]== false && cellNext.region == region )
            {
                result += CountCellEmpty(posNext,  region,  Check);
            }
        }
        return result;
    }

    public  bool CheckWin()
    {
        int cntCorrect = 0;
        for (int i = 0; i < sizeBoard; i++)
        { 
            for (int j = 0; j < sizeBoard; j++)
            {
                if(cells[i][j].statusCell== StatusCell.DoubleClick)
                {
                    if(!GameConfig.instance.GetLevelCurrent().dataBoard.CheckPosCorrectStar(cells[i][j].pos)){
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
        
        return cntCorrect == GameConfig.instance.GetLevelCurrent().dataBoard.posCorrectStar.Count;
    }
    #endregion


    #region show wrong
    public void HighLightRegion(Vector2Int pos, int region, List<bool> Check, bool correct)
    {
        Check[pos.x * sizeBoard + pos.y] = true; 
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
            Cell cellNext = cells[posNext.x][posNext.y];
            if (Check[posNext.x * sizeBoard + posNext.y] == false && cellNext.region == region)
            {
                HighLightRegion(posNext, region, Check,  correct);
               
            }
        }
        
    }
    public void HighLightRow(int row, bool correct)
    {
        for (int i = 0; i < sizeBoard; i++)
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
        for (int i = 0; i < sizeBoard; i++)
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




    public Cell GetCellByPos(int x , int y)
    {
        if (CheckPosCorect(x, y))
            return cells[x][y];
        return null;
    }
    public bool CheckPosCorect(int x, int y)
    {
        if (x < 0 || x >= sizeBoard || y < 0 || y >= sizeBoard)
            return false;
        return true;
    }



    #region MakeLevel
    public void Clear()
    {
        InitBoardMakeLevel(true);
    }

    public bool CheckRowLevel(int row)
    {
        int numDot = 0, numStar = 0;
        for (int i = 0; i < sizeBoard; i++)
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
        if (  numStar != 2)
        {
            HighLightRow(row, false);
            return false;
        }
        else
        {
            HighLightRow(row, true);
            return true;
        }
    }
    public bool CheckColumnLevel(int column)
    {

        int numDot = 0, numStar = 0;
        for (int i = 0; i < sizeBoard; i++)
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
        if (numStar != 2)
        {
            HighLightColumn(column, false);
            return false;
        }
        else
        {
            HighLightColumn(column, true);
            return true;
        }
    }

    public bool CheckRegionLevel(Cell cell)
    {
        List<bool> check = new List<bool>(new bool[sizeBoard * sizeBoard]);
        Vector2Int numCell = CountCellEmpty(cell.pos, cell.region, check);
        check = new List<bool>(new bool[sizeBoard * sizeBoard]);

        if (  numCell.y != 2)
        {
            HighLightRegion(cell.pos, cell.region, check, false);
            return false;
        }
        else
        {
            HighLightRegion(cell.pos, cell.region, check, true);
            return true;
        }
    }
    public void CheckCorrectMap()
    {
        foreach(Cell i in cellsRaw)
        {
            bool c = true;
            c = c && i.CheckMapInModeLeve();
            
            if (i.statusCell == StatusCell.DoubleClick)
            { 
                c = c&& i.CheckArrond();
            }
            if(c == false)
            {
                return;
            }

        }
    }
    #endregion
}
