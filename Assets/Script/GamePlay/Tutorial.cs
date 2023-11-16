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

}
public class HintMesage
{
    public TypeHint typeHint;
    public List<Vector2Int> posHints;
    public List<Vector2Int> posStars;
    public HintMesage(TypeHint typeHint, List<Vector2Int> posHint)
    {
        this.typeHint = typeHint;
        this.posHints = posHint;
    }
    public HintMesage(TypeHint typeHint, List<Vector2Int> posHint, List<Vector2Int> posStars)
    {
        this.typeHint = typeHint;
        this.posHints = posHint;
        this.posStars = posStars;
    }
    public void SetPostHint(List<Vector2Int> posHint)
    {
        this.posHints = posHint;
    }
    public void SetPosStar(List<Vector2Int> posStars)
    {
        this.posStars = posStars;
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
            board.cells[i.x][i.y].ShowHint(false);
        }
        foreach (Vector2Int i in posStars)
        {
            board.cells[i.x][i.y].ShowHint(true);
        }
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
                    else if (isDetectedHint) break;


                }
                if (isDetectedHint) break;
            }
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
                    if (!GameConfig.instance.dataBoard.CheckPosCorrectStar(new Vector2Int(i, j))) {
                        posHint.Add(new Vector2Int(i, j));
                        hintMesage.SetPostHint(posHint);
                        return hintMesage;
                    }
                // case cell is dot and real is star
                if (board.cells[i][j].statusCell == StatusCell.OneClick)
                    if (GameConfig.instance.dataBoard.CheckPosCorrectStar(new Vector2Int(i, j)))
                    {
                        posHint.Add(new Vector2Int(i, j));
                        hintMesage.SetPostHint(posHint);
                        return hintMesage;
                    }
            }
        }
        // all cell be clicked is correct
        hintMesage = new HintMesage(TypeHint.None, new List<Vector2Int>());
        return hintMesage;
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
            HintMesage hintMesage = new HintMesage(TypeHint.MustTwoWstar, posStar);
            return hintMesage;
        }
        // case row is fisnish -> mark cell is dot 
        if (posStar.Count == 2)
        {
            HintMesage hintMesage = new HintMesage(TypeHint.TwoStarCorrect, posEmpty);
            return hintMesage;

        }
        // case row has one star and has only cell is empty
        if ((posStar.Count == 1 && posEmpty.Count == 1))
        {
            HintMesage hintMesage = new HintMesage(TypeHint.MustOneStar, posEmpty);
            return hintMesage;
        }
        // case wrong  haven't check because cell has infomation about error when user click

        // case row correct
        return new HintMesage(TypeHint.None, new List<Vector2Int>());

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
            HintMesage hintMesage = new HintMesage(TypeHint.MustTwoWstar, posStar);
            return hintMesage;
        }
        // case column is fisnish -> mark cell is dot 
        if (posStar.Count == 2)
        {
            HintMesage hintMesage = new HintMesage(TypeHint.TwoStarCorrect, posEmpty);
            return hintMesage;

        }
        // case column has one star and has only cell is empty
        if ((posStar.Count == 1 && posEmpty.Count == 1))
        {
            HintMesage hintMesage = new HintMesage(TypeHint.MustOneStar, posEmpty);
            return hintMesage;
        }
        // case wrong  haven't check because cell has infomation about error when user click

        // case row correct
        return new HintMesage(TypeHint.None, new List<Vector2Int>());
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
            return new HintMesage(TypeHint.Arround, posEmpty);
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
            return new HintMesage(TypeHint.MustClickMoreOneStarInRegion, posEmpty);
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
        else if (numCell.y == 0 && (numCell.x >= 3 ))
        {  
            posEmpty.Sort((  v1,   v2) => {
                if (v1.x.Equals(v2.x))
                {
                    return v1.y.CompareTo(v2.y);
                }
                else return v1.x.CompareTo(v2.x);
            });
            
             
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
    public HintMesage HandelCaseSpecial(List<Vector2Int> posEmpty, Board board)
    {
       
        int numx = 0;
        int numy = 0;
        
        if (posEmpty.Count >= 4)
        {
            for (int i = 1; i < posEmpty.Count; i++)
            {
                /*
                    *
                    *
                    *
                 */
                if (posEmpty[i].x - posEmpty[i - 1].x == 1 && posEmpty[i].y == posEmpty[i - 1].y)
                {
                    numx++;
                }
                //[***] -> 00 02 star - 01 dot
                if (posEmpty[i].y - posEmpty[i - 1].y == 1 && posEmpty[i].x == posEmpty[i - 1].x)
                {
                    numy++;
                }
                /*   
                    *
                    * -> 00 20 star - 10   dot
                    *
                */
            }
            if (numx == 2 || numy == 2)
            {
                RegionSpecial regionSpecial = templateSpecial.Check(posEmpty,board);
                
                List<Vector2Int> posStar = new List<Vector2Int>();
                if (regionSpecial!= null)
                { 
                    for (int i =0; i< regionSpecial.posCellImportant.Count; i ++)
                    {
                        posStar.Add(regionSpecial.posCellImportant[i] + posEmpty[0]);
                    }
                    return new HintMesage(TypeHint.MustClickMoreOneStarInRegion, posEmpty, posStar);
                }

                
            }
        }
        return new HintMesage(TypeHint.None, new List<Vector2Int>());
    }

    public HintMesage HandelCaseSpecial2 (List<Vector2Int> posEmpty, Board board)
    {
        int countMax = -200;
        List<Vector2Int> posStar= new List<Vector2Int>();
        if(posEmpty.Count >= 4)
        {

            RegionSpecial regionSpecial =  templateSpecial.Checkit(posEmpty, board);
            if(regionSpecial!= null)
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
                return new HintMesage(TypeHint.None, posEmpty, posStar);
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
            return new HintMesage(TypeHint.None, posEmpty, posStar);
        }
        return new HintMesage(TypeHint.None, new List<Vector2Int>(), posStar);
    }


    public bool Hande3(List<Vector2Int> posEmpty)
    {
        int numx = 0;
        int numy = 0;
        for (int i = 1; i < posEmpty.Count; i++)
        {
            /*
                *
                *
                *
             */
            if (posEmpty[i].x - posEmpty[i - 1].x == 1 && posEmpty[i].y == posEmpty[i - 1].y)
            {
                numx++;
            }
            //[***] -> 00 02 star - 01 dot
            if (posEmpty[i].y - posEmpty[i - 1].y == 1 && posEmpty[i].x == posEmpty[i - 1].x)
            {
                numy++;
            }
            /*   
                *
                ** -> 00 20 star - 10 11 dot
                *
            */


        }
        if (numx == 2 || numy == 2)
        {
            return true;
        }
        return false;
    }
}
