using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum StatusCell
{
    None,
    OneClick,
    DoubleClick,

}
public enum NameError
{
    Row,
    Column,
    Region,
    Cross,
}

public class Cell :MonoBehaviour  
{ 
    public Sprite star;
    public Sprite dot;
    public  StatusCell statusCell;
    public int region;
    public Image inCellBgr;
    public Image Icon;
    public float disInRegioin ;
    public float disOutRegioin ;
    public Text txt;
    public Vector2Int pos;
    Board board;
    bool beingWrong;
    public List<NameError> errors= new List<NameError>();
    private void Awake()
    {

        disInRegioin = 0.5f;
        disOutRegioin = 2f;
    }
    // Start is called before the first frame update
    void Start()
    {

        Icon.color = new Color32(255, 255, 255, 0);

    }
    

    public bool CheckRegion(int region)
    {
        return this.region == region;
    }
    public Vector2 GetDis(Cell cell, Vector2 offset, int v)
    {
        Vector2 offsetRs = offset;
        if (cell != null) //
        {
            if (cell.CheckRegion(region)) // same region => boder is thin
            {
                if(v == -1)
                    offsetRs.x = disInRegioin;
                else
                    offsetRs.y = disInRegioin;
            } 
            else  // difference region => boder is stroke
            {
                if (v == -1)
                    offsetRs.x = disOutRegioin;
                else
                    offsetRs.y = disOutRegioin;
            }
        }
        else // cell is empty so boder is stroke
        {
            if (v == -1)
                offsetRs.x = disOutRegioin;
            else
                offsetRs.y = disOutRegioin;
        } 
        return offsetRs;
    }
    public void InitCell(Cell l, Cell t, Cell r, Cell b , Board board)
    {
        this.board = board;
        Vector2 oMin = Vector2.zero;
        Vector2 oMax = Vector2.zero;
        
        /*
           min.x = l
           min.y = b

           max.x = -r
           max.y = -t
        */
        oMin =  GetDis(l, oMin, -1);
        oMin =GetDis(b, oMin, 1);
        oMax= GetDis(r, oMax, -1);
        oMax= GetDis(t, oMax, 1);
     
        oMax *= -1;
       /* if (GameConfig.instance.dataBoard.CheckPosCorrect(pos))
            txt.text = "*";
        txt.text += region + "";*/
        txt.text = "";
        ChangeRectranform(oMin, oMax);

        inCellBgr.color = new Color(1 - 0.05f * region, 1 - 0.05f * region, 1 - 0.05f* region  ,255);
    }
    public void ChangeRectranform(Vector2 omin, Vector2 omax)
    {
        inCellBgr.GetComponent<RectTransform>().offsetMin = omin;
        inCellBgr.GetComponent<RectTransform>().offsetMax = omax;
    }


    #region handle click
    public void Click()
    {
      

        if (statusCell == StatusCell.OneClick && !beEffectByMoveMouse)
        { 
            DoubleClick(true);
        }
        else if (statusCell == StatusCell.DoubleClick && !beEffectByMoveMouse )
        {
            DonClick(true);
        } 
    }
    public void DonClick(bool isUser)
    {
        if (isUser)
            GameContrler.instance.AddAction(new HistoryAction(this, statusCell));
        statusCell = StatusCell.None;
        Icon.sprite = null;
        Icon.color = new Color32(255, 255, 255, 0);
        if (isUser) // because when user: from star-> none so will check only user 
            board.CheckAround(pos); 
        Check();
    }
    public void OneClick(bool isUser)
    {
        if (isUser)
            GameContrler.instance.AddAction(new HistoryAction(this, statusCell));
        statusCell = StatusCell.OneClick;
        Icon.color = new Color32(255, 255, 255, 255);
        Icon.sprite = dot;
        if(!isUser)// because when isn't user: from star-> dot so will check only isn't user
            board.CheckAround(pos);
        Check();
    }
    public void DoubleClick(bool isUser)
    {
        if(isUser)
            GameContrler.instance.AddAction(new HistoryAction(this, statusCell));
        statusCell = StatusCell.DoubleClick;
        Icon.color = new Color32(255, 255, 255, 255);
        Icon.sprite = star;
        Check();
       
        if (board.CheckWin())
        {
            MyEvent.GameWin?.Invoke();
        }

        board.CheckAround(pos);

    }
    bool beEffectByMoveMouse;
    public void MoveOnCell()
    { 
        
        if(statusCell == StatusCell.None)
        {
            OneClick(true);
            beEffectByMoveMouse = true;
        }
        
        else
        {
            beEffectByMoveMouse = false;
        } 


    }
    bool isDown;
    public void OnDown()
    { 
        isUp = false;
        isDown = true;
    }
    public void OnExit()
    {

        beEffectByMoveMouse = true;
    }
    bool isUp;
    public void onUp()
    {
        isUp = true; 
        if (isDown)
        {
            Click();
            isDown = false;
        } 
    }
    public void HandelMain()
    {

    }
    public void Check()
    {
        board.CheckRow(pos.x);
        board.CheckColumn(pos.y);
        board.CheckRegion(this);
    }
    #endregion



    #region display

    public void HightLightWrong(NameError nameError)
    {
        inCellBgr.color = Color.red;
        beingWrong = true;
        if (!errors.Contains(nameError)&& nameError!= NameError.Cross)
        {
            errors.Add(nameError);
        }
        else if (nameError == NameError.Cross)
        {
            errors.Add(nameError);

        }
    }
    public void ShowNormal(NameError nameError)
    {
       
        if (errors.Contains(nameError))
        {
            errors.Remove(nameError);
        }
        if(errors.Count ==0 && beingWrong == true)
        { 
            beingWrong = false;

            inCellBgr.color = new Color(1 - 0.05f * region, 1 - 0.05f * region, 1 - 0.05f * region, 255);
        }
    }
    #endregion


    public void SetRegion(int region)
    {
        this.region = region;
    }
    public Vector2Int GetPos()
    {
        return pos;
    }
    public void SetPos(Vector2Int p)
    {
        pos = p;
    }

       
}
