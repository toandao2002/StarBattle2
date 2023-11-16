using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionSpecial
{
    public List<Vector2Int> poscells;
    public List<Vector2Int> posCellImportant;
    public RegionSpecial(List<Vector2Int> poscells, List<Vector2Int> posCellImportant)
    {
        this.poscells = poscells;
        this.posCellImportant = posCellImportant;
    }
    public bool CheckFit(List<Vector2Int> posEmpty, Board board)
    { 
        int countCell = 0;
        int countFit = 0;

        for (int i = 0; i < poscells.Count; i++)
        {
            Vector2Int posCheck = poscells[i] + posEmpty[0];
            if (posEmpty.Contains(posCheck))
            {
                countCell++;
                if (posCellImportant.Contains(posCheck))
                {
                    countFit++;


                }
            }
            else
            {
                Vector2Int posCheckRealBoard = poscells[i] + posEmpty[0];
                if(board.CheckPosCorect(posCheckRealBoard.x, posCheckRealBoard.y))
                    if (board.cells[posCheckRealBoard.x][posCheckRealBoard.y].statusCell == StatusCell.OneClick && !posCellImportant.Contains(poscells[i]) )
                    { 
                        countCell++;
                    }
                    else if (board.cells[posCheckRealBoard.x][posCheckRealBoard.y].statusCell == StatusCell.DoubleClick && posCellImportant.Contains(poscells[i]))
                    {
                        countFit++;
                        countCell++;
                    }
            }
        }
       
        {
            if (countFit == posCellImportant.Count && countCell == poscells.Count )
            {
                return true;

            }
        }
        
        return false;
        
    }
    public bool CheckFit2(List<Vector2Int> posEmpty, Board board)
    {
        int countCell = 0;
        int countFit = 0;

        for (int i = 0; i < posEmpty.Count; i++)
        {
            Vector2Int posCheck = posEmpty[i] - posEmpty[0];
            if (poscells.Contains(posCheck))
            {
                countCell++;
                
            }
             
        }
 

        return countCell == posEmpty.Count;

    }
}
public class TemplateSpecial 
{
    /*
                    template1 :
                                *
                                **
                                *
                    template2 :
                                *
                               **
                                *  
                    Template3 :
                                **
                                **
                                 *
                    Template4 :
                                **
                                **
                                *
                    Template5 :
                                 *  
                                **
                                **
                    Template6 :
                                *
                                **
                                **
                    Template7 :
                                ***
                                **
                    Template8 :
                                **
                                ***
                                
                    Template9 :
                                ***
                                 **
                    Template10 :
                                 **
                                ***
                 */
    public List<RegionSpecial> posTemplates = new List<RegionSpecial>()
    {
        //1_1
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(1,0),
                    new Vector2Int(2,0),

                },new List<Vector2Int>(){ 
                    new Vector2Int(2,0)
                }),
        //1_2
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(1,1),
                    new Vector2Int(2,0),

                },new List<Vector2Int>(){
                    new Vector2Int(0,0),
                    new Vector2Int(2,0)
                }),
        //1_3
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,0),
                    new Vector2Int(2,1),

                },new List<Vector2Int>(){
                    new Vector2Int(0,0), 
                }),
        //1_4
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(2,-1),
                    new Vector2Int(2,0),

                },new List<Vector2Int>(){
                    new Vector2Int(0,0), 
                }),
        //1_5
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(1,-1),
                    new Vector2Int(1,0),
                    new Vector2Int(2,0),

                },new List<Vector2Int>(){
                    new Vector2Int(0,0),
                    new Vector2Int(2,0),
                }),
         //1_6
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(1,1),
                    new Vector2Int(2,1),

                },new List<Vector2Int>(){ 
                    new Vector2Int(2,1),
                }),

       //2-1
        
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(1,0),
                    new Vector2Int(1,1),
                    new Vector2Int(1,2),

                },new List<Vector2Int>(){
                    new Vector2Int(1,2),
                }),
        //2-2
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,1),
                    new Vector2Int(1,0),
                    new Vector2Int(1,1),
                    new Vector2Int(1,2),

                },new List<Vector2Int>(){
                    new Vector2Int(1,0),
                    new Vector2Int(1,2),
                }),
         //2-3
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,2),
                    new Vector2Int(1,0),
                    new Vector2Int(1,1),
                    new Vector2Int(1,2),

                },new List<Vector2Int>(){
                    new Vector2Int(1,0),
                }),
        //2-4
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,2),
                    new Vector2Int(1,2),

                },new List<Vector2Int>(){
                    new Vector2Int(0,0),
                }),
        //2-5
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,2),
                    new Vector2Int(1,1),

                },new List<Vector2Int>(){
                    new Vector2Int(0,0),
                    new Vector2Int(0,2),
                }),
       //2-6
       new RegionSpecial( new List<Vector2Int>() {
                    new Vector2Int(0,0),
                    new Vector2Int(0,1),
                    new Vector2Int(0,2),
                    new Vector2Int(1,0),

                },new List<Vector2Int>(){ 
                    new Vector2Int(0,2),
                }),
        //3

       new RegionSpecial(new List<Vector2Int>() {
                        new Vector2Int(0,0),
                        new Vector2Int(0,1),
                        new Vector2Int(1,0),
                        new Vector2Int(1,1),
                        new Vector2Int(2,1),

                    }, new List<Vector2Int>()
                        {
                            new Vector2Int(2,1)
                        }),
        //4
        new RegionSpecial(new List<Vector2Int>() {
                        new Vector2Int(0,0),
                        new Vector2Int(0,1),
                        new Vector2Int(1,0),
                        new Vector2Int(1,1),
                        new Vector2Int(2,0),
                    },new List<Vector2Int>()
                        {
                            new Vector2Int(2,0)
                        }),
        //5
        new RegionSpecial(new List<Vector2Int>() {
                        new Vector2Int(0,0),
                        new Vector2Int(1,-1),
                        new Vector2Int(1,0),
                        new Vector2Int(2,-1),
                        new Vector2Int(2,0),

                    },new List<Vector2Int>()
                        {
                            new Vector2Int(0,0)
                        }),
        //6
        new RegionSpecial(new List<Vector2Int>() {
                        new Vector2Int(0,0),
                        new Vector2Int(1,0),
                        new Vector2Int(1,1),
                        new Vector2Int(2,0),
                        new Vector2Int(2,1),

                    },new List<Vector2Int>()
                        {
                            new Vector2Int(0,0)
                        }),
        //7
        new RegionSpecial(new List<Vector2Int>() {
                        new Vector2Int(0,0),
                        new Vector2Int(0,1),
                        new Vector2Int(0,2),
                        new Vector2Int(1,0),
                        new Vector2Int(1,1),

                    },
                    new List<Vector2Int>()
                        {
                            new Vector2Int(0,2)
                        }),
        //8
        new RegionSpecial(new List<Vector2Int>() {
                        new Vector2Int(0,0),
                        new Vector2Int(0,1),
                        new Vector2Int(1,0),
                        new Vector2Int(1,1),
                        new Vector2Int(1,2),

                    },new List<Vector2Int>()
                        {
                            new Vector2Int(1,2)
                        }),
        //9
        new RegionSpecial(new List<Vector2Int>() {
                        new Vector2Int(0,0),
                        new Vector2Int(0,1),
                        new Vector2Int(0,2),
                        new Vector2Int(1,1),
                        new Vector2Int(1,2),
                    },new List<Vector2Int>()
                        {
                            new Vector2Int(0,0)
                        }),
        //10
        new RegionSpecial(new List<Vector2Int>() {
                        new Vector2Int(0,1),
                        new Vector2Int(0,2),
                        new Vector2Int(1,-1),
                        new Vector2Int(1,0),
                        new Vector2Int(1,1),
                    },new List<Vector2Int>()
                        {
                            new Vector2Int(1,-1)
                        }),

    };
    
    public RegionSpecial Check(List<Vector2Int> posEmpty, Board board)
    {
        foreach (RegionSpecial i in posTemplates)
        {
            if (i.CheckFit(posEmpty,board))
            {
                return i;
            }
        }
        return null;
    }
    public RegionSpecial Checkit(List<Vector2Int> posEmpty, Board board)
    {
        foreach (RegionSpecial i in posTemplates)
        {
            if (i.CheckFit2(posEmpty, board))
            {
                return i;
            }
        }
        return null;
    }
}
