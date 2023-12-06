using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeHint{
    None,
    MoreTwoStar,
    MustTwoWstar,
    MustOneStar,
    TwoStarCorrect,
    ManyDot,
    IncorrectPos,
    Arround,
    MustClickMoreOneStarInRegion,
    ShouldOneStarInRegion,
    ShouldClickStarInRegion,
    ShouldDotBecauseSquare,
    ShoudlColumnAround,
    MustMarkDot,
    MarkDotUnlessHasCellEmpty,
}
public class HintMesage
{
    public TypeHint typeHint;
    public List<Vector2Int> posHints;
    public List<Vector2Int> posStars;
    public List<Vector2Int> posDots;
    public HintMesage(TypeHint typeHint, List<Vector2Int> posHint)
    {
        this.typeHint = typeHint;
        this.posHints = posHint;
    }
    public HintMesage(TypeHint typeHint, List<Vector2Int> posHint, List<Vector2Int> posStars, List<Vector2Int> posDots)
    {
        this.typeHint = typeHint;
        this.posHints = posHint;
        this.posStars = posStars;
        this.posDots = posDots;
    }
    public void SetPostHint(List<Vector2Int> posHint)
    {
        this.posHints = posHint;
    }
    public void SetPosStar(List<Vector2Int> posStars)
    {
        this.posStars = posStars;
    }
    public void SetPosDot(List<Vector2Int> posDots)
    {

        this.posDots = posDots;
    }
    public void SetTypeHint(TypeHint typeHint)
    {
        this.typeHint = typeHint;
    }
    public override string ToString()
    {
        string s = typeHint.ToString() +" posHint: "+ DebugLogList(posHints)+ " Posstars: "+DebugLogList(posStars);
        return s;
    }
    public string  DebugLogList(List<Vector2Int> l)
    {
        string s = "";
        if (l == null) return "";
        foreach (Vector2Int i in l)
        {
            s += (" " + i.ToString());
        }
        return s;
    }
    public void ShowHint(Board board)
    {
        foreach(Vector2Int i in posHints)
        {
            board.cells[i.x][i.y].ShowHint(StatusCell.None);
        }
        foreach (Vector2Int i in posDots)
        {
            board.cells[i.x][i.y].ShowHint(StatusCell.OneClick);
        }
        foreach (Vector2Int i in posStars)
        {
            board.cells[i.x][i.y].ShowHint(StatusCell.DoubleClick);
        }
        HintMesageUI.instance.ShowHint(typeHint);
    }
}
public class Tutorial
{
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

    public HintMesage Hint(Board board)
    {
        if (GameConfig.instance.GetCurrentLevel().datalevel.isfinished) return null;
        HintMesage hintMesage;
        bool isDetectedHint = false;
        // case incorect position of star or dot
        hintMesage = FindIncorrestPos(board);
        if (hintMesage.posHints.Count != 0)
        {
            isDetectedHint = true;
        }
        if (!isDetectedHint)
            for (int i = 0; i < board.sizeBoard; i++)
            {
                for (int j = 0; j < board.sizeBoard; j++)
                {
                    // case cell is star and real isn't star
                    if (board.cells[i][j].statusCell == StatusCell.None && !isDetectedHint)
                    {
                        hintMesage = CheckRow(board, i);
                        if (hintMesage.posHints.Count != 0)
                        {
                            isDetectedHint = true;
                            break;

                        }

                        hintMesage = CheckColumn(board, j);
                        if (hintMesage.posHints.Count != 0)
                        {
                            isDetectedHint = true;
                            break;

                        }
                        hintMesage = CheckRegion(board, board.cells[i][j]);
                        if (hintMesage.posHints.Count != 0)
                        {
                            isDetectedHint = true;
                            break;

                        }
                        hintMesage = CheckRegion(board, board.cells[i][j]);
                        if (hintMesage.posHints.Count != 0)
                        {
                            isDetectedHint = true;
                            break;

                        }
                        hintMesage = CheckAroundRegionOfCell(board, board.cells[i][j].pos);
                        if (hintMesage.posHints.Count != 0)
                        {
                            isDetectedHint = true;
                            break;

                        }
                    }
                    if (board.cells[i][j].statusCell == StatusCell.DoubleClick && !isDetectedHint)
                    {
                        hintMesage = CheckAround(board, new Vector2Int(i, j));
                        if (hintMesage.posHints.Count != 0)
                        {
                            isDetectedHint = true;
                            break;
                        }
                    }
                    else if (!isDetectedHint)  // suppose this cell is star then check these cells around 
                    { 
                    
                    
                    }


                }
                if (isDetectedHint) break;
            }
        ManageAudio.Instacne.PlaySound(NameSound.Hint);
        Debug.Log(hintMesage);
        return hintMesage;

        // 

    }
   
    public HintMesage FindIncorrestPos(Board board)
    {
        List<Vector2Int> posHint = new List<Vector2Int>();
        HintMesage hintMesage = new HintMesage(TypeHint.IncorrectPos, new List<Vector2Int>());
        for (int i = 0; i < board.sizeBoard; i++)
        {
            for (int j = 0; j < board.sizeBoard; j++)
            {
                // case cell is star and real isn't star
                if (board.cells[i][j].statusCell == StatusCell.DoubleClick)
                    if (!GameConfig.instance.GetLevelCurrent().dataBoard.CheckPosCorrectStar(new Vector2Int(i, j))) {
                        posHint.Add(new Vector2Int(i, j));
                        hintMesage.SetPostHint(posHint);
                        hintMesage.SetPosStar(new List<Vector2Int>());
                        hintMesage.SetPosDot(new List<Vector2Int>());
                        return hintMesage;
                    }
                // case cell is dot and real is star
                if (board.cells[i][j].statusCell == StatusCell.OneClick)
                    if (GameConfig.instance.GetLevelCurrent().dataBoard.CheckPosCorrectStar(new Vector2Int(i, j)))
                    {
                        posHint.Add(new Vector2Int(i, j));
                        hintMesage.SetPostHint(posHint);
                        hintMesage.SetPosStar(new List<Vector2Int>());

                        hintMesage.SetPosDot(new List<Vector2Int>());
                        return hintMesage;
                    }
            }
        }
        // all cell be clicked is correct
        hintMesage = new HintMesage(TypeHint.None, new List<Vector2Int>());
        return hintMesage;
    }

    public HintMesage CheckRegionInRowOrColumn (List <Vector2Int> posEmpty, Board board, int amountStar)
    {
        var subRegion = new Dictionary<int, List<Vector2Int>>();
        for (int i = 0; i < posEmpty.Count; i++)
        {
            int region = board.cells[posEmpty[i].x][posEmpty[i].y].region;
            if (subRegion.ContainsKey(region))
            {
                subRegion[region].Add(posEmpty[i]);
            }
            else
            {
                subRegion[region] = new List<Vector2Int>();
                subRegion[region].Add(posEmpty[i]);
            }
        } 
        List<int> regionHasOneStar = new List<int>();
        foreach (int i in subRegion.Keys)
        {

            List<bool> check = new List<bool>(new bool[board. sizeBoard * board.sizeBoard]);
            Vector2Int numCell = CountCellEmpty(board, subRegion[i][0], i, check, new List<Vector2Int>()); 
            if(numCell.y == 1 && numCell.x == subRegion[i].Count)
            {
                regionHasOneStar.Add(i);
            }

        }
        // case 2 star in row or column must be in this 2 region 
        if (regionHasOneStar.Count == 2&& subRegion.Count >3 && amountStar ==0 )
        {
            List<Vector2Int> posHint = new List<Vector2Int>();
            List<Vector2Int> posDot = new List<Vector2Int>();
            foreach (int i in subRegion.Keys)
            { 
                if (regionHasOneStar.Contains(i))
                {
                    posHint.AddRange(subRegion[i]);
                }
                else
                {
                    posDot.AddRange(subRegion[i]);
                }

            }
            return new HintMesage(TypeHint.MustMarkDot, posHint, new List<Vector2Int>(), posDot);
        }
        else if(amountStar == 1 && regionHasOneStar.Count == 1&& subRegion.Count>=1)
        {
            List<Vector2Int> posHint = new List<Vector2Int>();
            List<Vector2Int> posDot = new List<Vector2Int>();
            foreach (int i in subRegion.Keys)
            {
                if (regionHasOneStar.Contains(i))
                {
                    posHint.AddRange(subRegion[i]);
                }
                else
                {
                    posDot.AddRange(subRegion[i]);
                }

            }
            return new HintMesage(TypeHint.MustMarkDot, posHint, new List<Vector2Int>(), posDot);
        }
        return new HintMesage(TypeHint.MustMarkDot, new List<Vector2Int>(), new List<Vector2Int>(), new List<Vector2Int>());
    }
    public HintMesage CheckRow(Board board, int row)
    {


        List<Vector2Int> posStar = new List<Vector2Int>();
        List<Vector2Int> posDot = new List<Vector2Int>();
        List<Vector2Int> posEmpty = new List<Vector2Int>();
        for (int i = 0; i < board.sizeBoard; i++)
        {

            if (board.cells[row][i].statusCell == StatusCell.OneClick)
            {
                posDot.Add(new Vector2Int(row, i));
            }
            if (board.cells[row][i].statusCell == StatusCell.DoubleClick)
            {
                posStar.Add(new Vector2Int(row, i));
            }
            if (board.cells[row][i].statusCell == StatusCell.None)
            {
                posEmpty.Add(new Vector2Int(row, i));
            }

        }
        // case must be mark  is star 
        if (posEmpty.Count == 2 && posStar.Count == 0)
        {
            HintMesage hintMesage = new HintMesage(TypeHint.MustTwoWstar, posStar, posStar, new List<Vector2Int>());
            return hintMesage;
        }
        // case row is fisnish -> mark cell is dot 
        if (posStar.Count == 2)
        {
            posStar.AddRange(posDot);
            HintMesage hintMesage = new HintMesage(TypeHint.TwoStarCorrect, posStar, new List<Vector2Int>(),posEmpty);
            return hintMesage;

        }
        // case row has one star and has only cell is empty
        if ((posStar.Count == 1 && posEmpty.Count == 1))
        {
            HintMesage hintMesage = new HintMesage(TypeHint.MustOneStar, posEmpty,posEmpty, new List<Vector2Int>());
            return hintMesage;
        }
        if (posEmpty.Count >= 2&& posEmpty.Count <= 4&&  posStar.Count == 0)
        {
            HintMesage hintMesage = CheckSquare(posEmpty, board);
            if (hintMesage.posHints.Count != 0)
                return hintMesage;
        }
        if(posEmpty.Count>2 && !CheckCellsIsSameRegion(posEmpty, board))
            return CheckRegionInRowOrColumn(posEmpty,board,posStar.Count);


        // case wrong  haven't check because cell has infomation about error when user click

        // case row correct
        return new HintMesage(TypeHint.None, new List<Vector2Int>(), new List<Vector2Int>(), new List<Vector2Int>());

    }
    public bool CheckCellsIsSameRegion(List<Vector2Int> posEmpty, Board board)
    {
        int region = board.cells[posEmpty[0].x][posEmpty[0].y].region;
        for (int i = 0;i< posEmpty.Count; i++)
        {
            int row = posEmpty[i].x, column = posEmpty[i].y;
            if( board.cells[row][column].region != region) return false;
        }
        return true;
    }
    public HintMesage CheckColumn(Board board, int column)
    {
        List<Vector2Int> posStar = new List<Vector2Int>();
        List<Vector2Int> posDot = new List<Vector2Int>();
        List<Vector2Int> posEmpty = new List<Vector2Int>();
        for (int i = 0; i < board.sizeBoard; i++)
        {

            if (board.cells[i][column].statusCell == StatusCell.OneClick)
            {
                posDot.Add(new Vector2Int(i, column));
            }
            if (board.cells[i][column].statusCell == StatusCell.DoubleClick)
            {
                posStar.Add(new Vector2Int(i, column));
            }
            if (board.cells[i][column].statusCell == StatusCell.None)
            {
                posEmpty.Add(new Vector2Int(i, column));
            }

        }
        // case must be mark  is star 
        if (posEmpty.Count == 2 && posStar.Count == 0)
        {
            HintMesage hintMesage = new HintMesage(TypeHint.MustTwoWstar, posStar, posStar, new List<Vector2Int>());
            return hintMesage;
        }
        // case column is fisnish -> mark cell is dot 
        if (posStar.Count == 2)
        {
            posStar.AddRange(posDot);
            HintMesage hintMesage = new HintMesage(TypeHint.TwoStarCorrect, posStar, new List<Vector2Int>(), posEmpty);
            return hintMesage;

        }
        // case column has one star and has only cell is empty
        if ((posStar.Count == 1 && posEmpty.Count == 1))
        {
            HintMesage hintMesage = new HintMesage(TypeHint.MustOneStar, posEmpty, posEmpty, new List<Vector2Int>());
            return hintMesage;
        }
        if (posEmpty.Count >= 2 && posEmpty.Count <= 4 && posStar.Count == 0)
        { 
            HintMesage hintMesage = CheckSquare(posEmpty, board);
            if (hintMesage.posHints.Count != 0)
                return hintMesage;
        }
        if (posEmpty.Count > 2)
            return CheckRegionInRowOrColumn(posEmpty, board, posStar.Count);
        // case wrong  haven't check because cell has infomation about error when user click

        // case row correct
        return new HintMesage(TypeHint.None, new List<Vector2Int>());
    }

    public HintMesage CheckSquare(List<Vector2Int> posEmpty,Board board)
    {
        List<Vector2Int>  posDot = new List<Vector2Int>();
        List<Vector2Int>  posStar = new List<Vector2Int>();
        for (int i = 0; i < posEmpty.Count; i++)
        {
            int count = board.CountAndCheckCellEmptyAround(posEmpty[i], posEmpty);
            if (count == 1)/// khong con nuoc di
            {
                posDot.Add(posEmpty[i]);
            }
            List<Vector2Int> PostEmtpyCheck = new List<Vector2Int>();
            PostEmtpyCheck.AddRange(posEmpty);
            PostEmtpyCheck.Remove(posEmpty[i]);

            if (templateSpecial.checkFitSquare(PostEmtpyCheck, board))
            {
                posStar.Add(posEmpty[i]);
            }

        }
        List<Vector2Int> posHint = new List<Vector2Int>();
        posHint.AddRange(posStar);
        posHint.AddRange(posDot);
        if (posStar.Count == 2)
            return new HintMesage(TypeHint.MustTwoWstar, posHint, posStar, posDot);
        else  if(posStar.Count == 1)
        {
            return new HintMesage(TypeHint.MustOneStar, posEmpty, posStar, posDot);
        }
        else
        {
            return new HintMesage(TypeHint.ShouldDotBecauseSquare, posHint, posStar, posDot);
        }
    }
    public HintMesage CheckAroundRegionOfCell( Board board,Vector2Int pos)
    {
        int sizeBoard = board.sizeBoard; 
        List<Vector2Int> posEmpty = new List<Vector2Int>();
        Cell CellCurrent = board.cells[pos.x][pos.y];
        if(pos == new Vector2Int(2, 7))
        {

        }
        foreach (Vector2Int d in direction)
        {
            Vector2Int posNext = pos + d;
            if (!board.CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }
            Cell cellNext = board.cells[posNext.x][posNext.y];
            if (cellNext.statusCell == StatusCell.None && cellNext.region != CellCurrent.region )
            {
                if(pos == new Vector2Int(2, 7))
                {
                    
                }
                posEmpty = new List<Vector2Int>();
                List<bool> check = new List<bool>(new bool[sizeBoard * sizeBoard]);
                Vector2Int rs =  GetCellEmptyInReGionDontNextOneStar(board,posNext,cellNext.region, check, posEmpty, pos);
                
                if(rs.y ==0  && rs.x<=4)
                {
                    posEmpty.Sort((v1, v2) => {
                        if (v1.x.Equals(v2.x))
                        {
                            return v1.y.CompareTo(v2.y);
                        }
                        else return v1.x.CompareTo(v2.x);
                    });
                    List<Vector2Int> posDot = new List<Vector2Int>();
                    //if empty cells is a part of square -> cell is dot
                    if (templateSpecial.checkFitSquare(posEmpty, board))
                    {
                        posDot.Add(pos); 
                    }
                    if(posDot.Count != 0)
                        return new HintMesage(TypeHint.ShouldDotBecauseSquare, posEmpty, new List<Vector2Int>(), posDot);
                }
                else if (rs.y == 1 && rs.x == 0)
                {

                    List<Vector2Int> posDot = new List<Vector2Int>(); 
                    posDot.Add(pos);
                    return new HintMesage(TypeHint.ShouldDotBecauseSquare, posDot, new List<Vector2Int>(), posDot);
                }
                else
                {
                    Debug.Log(posEmpty);
                }
            }

        } 
        return new HintMesage(TypeHint.None, new List<Vector2Int>(), new List<Vector2Int>(), new List<Vector2Int>());
        
        
    }
    public Vector2Int GetCellEmptyInReGionDontNextOneStar(Board board, Vector2Int pos, int region, List<bool> Check, List<Vector2Int> posEmtpy, Vector2Int posStar)
    {
        Cell cellCurent = board.cells[pos.x][pos.y];
        int numCellEmpty = 0;
        if (cellCurent.statusCell == StatusCell.None && !NextOneStar(board, pos,posStar))
        {
            posEmtpy.Add(cellCurent.pos);
            numCellEmpty = 1;
        }
        else
        {
            numCellEmpty = 0;
        }
        int NumStar = cellCurent.statusCell == StatusCell.DoubleClick ? 1 : 0;
        Vector2Int result = new Vector2Int(numCellEmpty, NumStar);
        Check[pos.x * board.sizeBoard + pos.y] = true;
        foreach (Vector2Int d in direction)
        {

            Vector2Int posNext = pos + d;
            if (!board.CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }


            Cell cellNext = board.cells[posNext.x][posNext.y];
            if (Check[posNext.x * board.sizeBoard + posNext.y] == false && cellNext.region == region)
            {
                result += GetCellEmptyInReGionDontNextOneStar(board, posNext, region, Check, posEmtpy,posStar);
            }
        }
        return result;  
    }
    public bool NextOneStar(Board board, Vector2Int pos, Vector2Int posStar)
    {

        Cell CellCurrent = board.cells[pos.x][pos.y];
        foreach (Vector2Int d in direction)
        {
            Vector2Int posNext = posStar + d;
            if (!board.CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            } 
            if (pos == posNext )
            {
                return true;
            }


        }
        return false;
    }

    
    public HintMesage CheckAround(Board board, Vector2Int pos)
    {


        List<Vector2Int> posEmpty = new List<Vector2Int>();
        Cell CellCurrent = board.cells[pos.x][pos.y];
        foreach (Vector2Int d in direction)
        {
            Vector2Int posNext = pos + d;
            if (!board.CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }
            Cell cellNext = board.cells[posNext.x][posNext.y];
            if (cellNext.statusCell == StatusCell.None && CellCurrent.statusCell == StatusCell.DoubleClick)
            {
                posEmpty.Add(posNext);
            }

        }
        if (posEmpty.Count == 0)
            return new HintMesage(TypeHint.None, new List<Vector2Int>());
        else
        {
            return new HintMesage(TypeHint.Arround, posEmpty, new List<Vector2Int>(), posEmpty);
        }
    }
    public HintMesage CheckRegion(Board board, Cell cell)
    {
        int sizeBoard = board.sizeBoard;
        List<bool> check = new List<bool>(new bool[sizeBoard * sizeBoard]);
        List<Vector2Int> posEmpty = new List<Vector2Int>();
        Vector2Int numCell = CountCellEmpty(board, cell.pos, cell.region, check, posEmpty);
        check = new List<bool>(new bool[sizeBoard * sizeBoard]);
        if (numCell.y == 1 && numCell.x == 1)
        {
            return new HintMesage(TypeHint.MustClickMoreOneStarInRegion, posEmpty, posEmpty, new List<Vector2Int>());
        }
        else if (numCell.y == 0 && numCell.x == 2)
        {
            return new HintMesage(TypeHint.MustClickMoreOneStarInRegion, posEmpty, posEmpty, new List<Vector2Int>());
        }
        else if (numCell.y == 0 && (numCell.x >= 3 ))
        {  
            posEmpty.Sort((  v1,   v2) => {
                if (v1.x.Equals(v2.x))
                {
                    return v1.y.CompareTo(v2.y);
                }
                else return v1.x.CompareTo(v2.x);
            });
            // case cell all in row or colunm
            var hintMesage = CheckEmptyCellsInRegionByRowAndColunm(posEmpty, board);
            if(hintMesage.posHints.Count > 0)
            {
                return hintMesage;
            }

            if (posEmpty.Count >= 3&& posEmpty.Count <=5)
            {
                return HandelCaseSpecial2(posEmpty, board);
            }
        }
        return new HintMesage(TypeHint.None, new List<Vector2Int>());
    }

    public Vector2Int CountCellEmpty(Board board, Vector2Int pos, int region, List<bool> Check,List<Vector2Int> posEmtpy)
    {
        Cell cellCurent = board.cells[pos.x][pos.y];
        int numCellEmpty = cellCurent.statusCell == StatusCell.None ? 1 : 0;
        int NumStar = cellCurent.statusCell == StatusCell.DoubleClick ? 1 : 0;
        Vector2Int result = new Vector2Int(numCellEmpty, NumStar);
        if (cellCurent.statusCell == StatusCell.None) posEmtpy.Add(cellCurent.pos);
        Check[pos.x * board.sizeBoard + pos.y] = true;
        foreach (Vector2Int d in direction)
        {

            Vector2Int posNext = pos + d;
            if (!board.CheckPosCorect(posNext.x, posNext.y))
            {
                continue;
            }


            Cell cellNext = board.cells[posNext.x][posNext.y];
            if (Check[posNext.x * board.sizeBoard + posNext.y] == false && cellNext.region == region)
            {
                result += CountCellEmpty( board,posNext, region, Check, posEmtpy);
            }
        }
        return result;
    }
    /*
            
        [***] -> 00 02 star - 01 dot
        
        *
        ** -> 00 20 star- 10 11 dot
        *
        
        *
       ** -> 00 20 star- 10 1-1 dot
        *
        
     */

    TemplateSpecial templateSpecial = new TemplateSpecial();

    public List<Vector2Int> MarkRowDot(int row, Board board, List<Vector2Int> posExcept)
    {
        List<Vector2Int> posEmpty = new List<Vector2Int>();
        for (int i = 0; i < board.sizeBoard; i++)
        {

            if (posExcept.Contains(board.cells[row][i].pos))
            {
                continue;
            }
            if ( board.cells[row][i].statusCell == StatusCell.None)
            {
                posEmpty.Add(new Vector2Int(row, i));
            }

        }
        return posEmpty;
    }
    public List<Vector2Int> MarkColumnDot(int column, Board board, List<Vector2Int> posExcept)
    {
        List<Vector2Int> posEmpty = new List<Vector2Int>();
        for (int i = 0; i < board.sizeBoard; i++)
        {

            if (posExcept.Contains(board.cells[i][column].pos))
            {
                continue;
            }
            if (board.cells[i][column].statusCell == StatusCell.None)
            {
                posEmpty.Add(new Vector2Int(i, column));
            }

        }
        return posEmpty;
    }
    // case: in region hasn't any star and empty cells cover in row or column
    public HintMesage CheckEmptyCellsInRegionByRowAndColunm(List<Vector2Int> posEmpty, Board board)
    {
        // check row
        bool inRow = true;
        bool inColumn = true;
        List<Vector2Int> posDot =new List<Vector2Int>();
        for (int i = 1; i<posEmpty.Count;i++)
        {
            if(posEmpty[i].x != posEmpty[i - 1].x)
            {
                inRow = false;
               
                break;
            }
        }
        if (inRow)
        {
            posDot = MarkRowDot(posEmpty[0].x, board, posEmpty);
            if(posDot.Count > 0)
            {
                return new HintMesage(TypeHint.MustMarkDot, posEmpty, new List<Vector2Int>(), posDot);
            }
        }

        // check column
        for (int i = 1; i < posEmpty.Count; i++)
        {
            if (posEmpty[i].y != posEmpty[i - 1].y)
            {
                inColumn = false;
                break;
            }
        }
        if (inColumn)
        {
            posDot = MarkColumnDot(posEmpty[0].y, board, posEmpty);
            if (posDot.Count > 0)
            {
                return new HintMesage(TypeHint.MustMarkDot, posEmpty, new List<Vector2Int>(), posDot);
            }
        }
        return new HintMesage(TypeHint.None, new List<Vector2Int>(), new List<Vector2Int>(), new List<Vector2Int>());
    }
    // handel case in one region empty cells 
    public HintMesage HandelCaseSpecial2 (List<Vector2Int> posEmpty, Board board)
    {
        int countMax = -200;
        List<Vector2Int> posStar= new List<Vector2Int>();
        List<Vector2Int> posDot= new List<Vector2Int>();
        if (posEmpty.Count >= 4)
        {

            RegionSpecial regionSpecial = templateSpecial.Checkit(posEmpty, board);
            if (regionSpecial != null)
            {
                for (int i = 0; i < posEmpty.Count; i++)
                {
                    int count = board.CountAndCheckCellEmptyAround(posEmpty[i], posEmpty);
                    if (count == 1) // khong con nuoc di
                    {
                        posDot.Add(posEmpty[i]);
                    }
                    if (count > countMax)
                    {
                        countMax = count;
                        posStar = new List<Vector2Int>();
                        posStar.Add(posEmpty[i]);
                    }
                    else if (count == countMax)
                    {
                        posStar.Add(posEmpty[i]);
                    }
                }
                if (posStar.Count <= 2)
                    return new HintMesage(TypeHint.ShouldOneStarInRegion, posEmpty, posStar, posDot);
                else
                {
                    return new HintMesage(TypeHint.ShouldOneStarInRegion, new List<Vector2Int>(), new List<Vector2Int>(), posDot);

                }

            }
            else if (regionSpecial == null)
            {
                for (int i = 0; i < posEmpty.Count; i++)
                {
                    int count = board.CountAndCheckCellEmptyAround(posEmpty[i], posEmpty);
                    if (count == 1)/// khong con nuoc di
                    {
                        posDot.Add(posEmpty[i]);
                    }
                    List<Vector2Int> PostEmtpyCheck = new List<Vector2Int>();
                    PostEmtpyCheck.AddRange(posEmpty);
                    PostEmtpyCheck.Remove(posEmpty[i]);

                    if (templateSpecial.checkFitSquare(PostEmtpyCheck, board))
                    {
                        posStar.Add(posEmpty[i]);
                    }

                }

                List<Vector2Int> posHint = new List<Vector2Int>();
                posHint.AddRange(posStar);
                posHint.AddRange(posDot);
                if(posStar.Count !=0)
                    return new HintMesage(TypeHint.ShouldOneStarInRegion, posEmpty, posStar, posDot);
                if (posStar.Count == 0&& posDot.Count != 0)
                    return new HintMesage(TypeHint.MarkDotUnlessHasCellEmpty, posEmpty, posStar, posDot);

            }
        }
        else
        {
            for (int i = 0; i < posEmpty.Count; i++)
            {
                int count = board.CountAndCheckCellEmptyAround(posEmpty[i], posEmpty);
                if (count > countMax)
                {
                    countMax = count;
                    posStar = new List<Vector2Int>();
                    posStar.Add(posEmpty[i]);
                }
                else if (count == countMax)
                {
                    posStar.Add(posEmpty[i]);
                }
            }
            if(posStar.Count <=2&& posStar.Count >0)
                return new HintMesage(TypeHint.ShouldClickStarInRegion, posEmpty, posStar, posEmpty);
        }
        return new HintMesage(TypeHint.None, new List<Vector2Int>(), new List<Vector2Int>(), new List<Vector2Int>());
    }




    #region get cells empty
    public void GetCellsEmtpyInRow(int row)
    {

    }
    #endregion
}
