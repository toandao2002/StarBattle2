using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using System;

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

    // only use in make level;
    public Board boardMakeLevel;
    public List<Color> stars;
    public Sprite star;
    Color colorStar;
    public List<Color> dots;
    Color colorDot;
    public Sprite dot;
    public Sprite starHint;
    public Sprite dotHint;
    public  StatusCell statusCell;
    public int region;
    public Image Bgr;
    public Image inCellBgr;
    public Image inCellBgrWrong ;
    public Image icon;
    public Image iconHint;
    public int disInRegioin ;
    public int disOutRegioin ;
    public Text txt;
    public Vector2Int pos;
    Board board;
    bool beingWrong;
    public Color hintStarColor; 
    public Color hintNoneColor;
    public List<Color> colorCells;
    public List<Color> colorCellLight;
    public List<Color> colorCellDark;
    Color color;
    public Color colorWrong;
    public Color colorWrongLight;
    public List<NameError> errors= new List<NameError>();
    int idColor;

    public List<Image> border;
    private void Awake()
    {

        disInRegioin = 1;
        disOutRegioin = 2;
        icon.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        MyEvent.ChangeTheme += ChangeTheme;
    }
    private void OnDisable()
    {
        MyEvent.ChangeTheme -= ChangeTheme;
    }
    // Start is called before the first frame update



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
                offsetRs.x = 0;
            else
                offsetRs.y = 0;
        } 
        return offsetRs;
    }
    public bool GetLogicBoder(Cell cell, Vector2 offset, int v)
    {
        Vector2 offsetRs = offset;

        if (cell != null) //
        {
            if (cell.CheckRegion(region)) // same region => boder is thin
            {
                return true;
            }
            else  // difference region => boder is stroke
            {
                return false;
            }
        }
        else // cell is empty so boder is stroke
        {
            return true;
        } 
    }
    public void InitCell(Cell l, Cell t, Cell r, Cell b , Board board)
    {
        ChangeTheme();
        this.board = board;
        if(!beingWrong)  
            inCellBgrWrong.gameObject.SetActive(false);
        Vector2 oMin = Vector2.zero;
        Vector2 oMax = Vector2.zero;
        bool bl, bt, br, bb;
     
        /*
           min.x = l
           min.y = b

           max.x = -r
           max.y = -t
        */
        if (pos == new Vector2(2, 0))
        {

        }
        oMin = GetDis(l, oMin, -1);
        oMin = GetDis(b, oMin, 1);
        oMax = GetDis(r, oMax, -1);
        oMax = GetDis(t, oMax, 1);


        oMax *= -1;
        if(board != null && board.isModeMakeLevel)
        {
            if (GameConfig.instance.GetLevelCurrent().dataBoard.CheckPosCorrectStar(pos))
                ShowStarIcon();
            txt.text = region + "";
        }
        else if (board != null && board.isFinish)
        {
            if (GameConfig.instance.GetLevelCurrent().dataBoard.CheckPosCorrectStar(pos))
                ShowStarIcon();
            txt.text = "";
        }
        else {
            txt.text = "";
        }
          

        ChangeRectranform(oMin, oMax);
        idColor = board.GetColorByRegion(region);
        color = colorCells[idColor];
        inCellBgr.color = color;
    }
    public void ChangeRectranform(Vector2 omin, Vector2 omax)
    {
         
        border[0].gameObject.SetActive(omin.x == disOutRegioin);
        
        border[3].gameObject.SetActive(omin.y == disOutRegioin);
        
        border[2].gameObject.SetActive(omax.x == -disOutRegioin);
        
        border[1].gameObject.SetActive(omax.y == -disOutRegioin);
        int sizeWith = 4;
         
        if(omax.y == 0)
        {
            border[1].gameObject.SetActive(true);
            border[1].GetComponent<RectTransform>().sizeDelta = new Vector2(0, sizeWith);
            border[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            border[1].GetComponent<RectTransform>().offsetMin = new Vector2(-sizeWith/2, 0);
            border[1].GetComponent<RectTransform>().offsetMax = new Vector2(sizeWith/2, sizeWith);
        }
        if (omax.x == 0)
        {
            border[2].gameObject.SetActive(true);
            border[2].GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            border[2].GetComponent<RectTransform>().sizeDelta = new Vector2(sizeWith, 0);
            border[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }

        if (omin.x == 0)
        {
            border[0].gameObject.SetActive(true);
            border[0].GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            border[0].GetComponent<RectTransform>().sizeDelta = new Vector2(sizeWith, 0);
            border[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0)    ;
        }
        if (omin.y == 0)
        {
            border[3].gameObject.SetActive(true);

            border[3].GetComponent<RectTransform>().offsetMin = new Vector2(-sizeWith/2, 0);
            border[3].GetComponent<RectTransform>().offsetMax = new Vector2(sizeWith/2, sizeWith);
            border[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        /*inCellBgr.GetComponent<RectTransform>().offsetMin = omin;
        inCellBgr.GetComponent<RectTransform>().offsetMax = omax;*/
    }


    #region handle click
    public void Click()
    {

        if (GameConfig.instance.levelCurent.datalevel.isfinished) return;
        if (statusCell == StatusCell.OneClick && !beEffectByMoveMouse)
        { 
            DoubleClick(true);
        }
        else if (statusCell == StatusCell.DoubleClick && !beEffectByMoveMouse )
        {
            DonClick(true);
        } 
    }
    
    public void ResetColorBgr()// by stopping coroutine some action can be wrong
    {  
        if (beingWrong)
        {
            inCellBgrWrong.gameObject.SetActive(true);

            inCellBgrWrong.color = colorWrong;
        }
        else
        {
            inCellBgrWrong.gameObject.SetActive(false);
            inCellBgr.color = color;
        }
        iconHint.gameObject.SetActive(false);
    }
    public void DonClick(bool isUser)// để phần biệt user và chức năng redo
    {
        MyEvent.ClickCell?.Invoke();
        if (isUser)
            GameContrler.instance.AddAction(new HistoryAction(this, statusCell));
        statusCell = StatusCell.None;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        ResetColorBgr();
        StopAllCoroutines();
        Check();
        if (isUser) // because when user: from star-> none so will check only user 
            board.CheckAround(pos);
        ManageAudio.Instacne.VibrateLight();

    }

    public void OneClick(bool isUser)
    {
        MyEvent.ClickCell?.Invoke();
        if (isUser)
            GameContrler.instance.AddAction(new HistoryAction(this, statusCell));
        ShowDotIcon();
        ResetColorBgr();
        StopAllCoroutines();
        
        Check();
        if (!isUser)// because when isn't user: from star-> dot so will check only isn't user
            board.CheckAround(pos);
        ManageAudio.Instacne.VibrateLight();
    }
    public void DoubleClick(bool isUser)
    {
        MyEvent.ClickCell?.Invoke();
        if (isUser)
            GameContrler.instance.AddAction(new HistoryAction(this, statusCell));
     
        ShowStarIcon();
        ResetColorBgr();
        StopAllCoroutines();
        if (board.CheckWin())
        {
            MyEvent.GameWin?.Invoke(null);

            ManageAudio.Instacne.PlaySound(NameSound.WinGame);
        }

        ManageAudio.Instacne.VibrateLight();
        Check();
        board.CheckAround(pos);


    }
    bool beEffectByMoveMouse;
    public void MoveOnCell()
    { 
        if (GameConfig.instance.levelCurent.datalevel.isfinished) return;
        if (statusCell == StatusCell.None)
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
        isExit = false;
    }
    bool isExit;
    public void OnExit()
    { 
        isExit = true;
        beEffectByMoveMouse = true;
    }
    bool isUp;
    public void onUp()
    { 

        isUp = true; 
        if (isDown && !isExit)
        {
            Click();
            isDown = false;
        } 
    }
     
    public void Check()
    {
        try
        {
            board.CheckRow(pos.x);
            board.CheckColumn(pos.y);
            board.CheckRegion(this);
        }
        catch
        {

        }
        
    }
    #endregion


    #region display
    public List<Color> borderColors;
    public List<Color> BgrColors;
    public List<Color> iconHintColors;
    

    public void ChangeTheme()
    {
        if(GameConfig.instance.nameTheme == NameTheme.Dark)
        {
            colorStar = stars[1];
            colorDot = dots[1];
            colorCells = colorCellDark;
            for(int i = 0; i<4; i++)
            {
                border[i].color = borderColors[1];
            }
            Bgr.color = BgrColors[1];
            iconHint.color = iconHintColors[1];
        }
        else
        {
            colorCells = colorCellLight;
            colorDot = dots[0];
            colorStar = stars[0];
            for (int i = 0; i < 4; i++)
            {
                border[i].color = borderColors[0];
            }
            Bgr.color = BgrColors[0];
            iconHint.color = iconHintColors[0];


        }
        color = colorCells[idColor];

        UpdateDisplayCell();
    }
    public void EffectWinStar()
    {
        icon.gameObject.transform.DOScale(0.7f, 0.3f).From(0.4f).SetEase(Ease.InOutBack);
    }
    public void ShowStatusCell(StatusCell _statusCell)
    {
        statusCell = _statusCell;
         if (_statusCell == StatusCell.OneClick)
        {
            OneClick(true);
        }
        else if (_statusCell == StatusCell.DoubleClick)
        {
            DoubleClick(true);
        }
        else
        {

        }
    }
    public void ShowDotIcon()
    {
        statusCell = StatusCell.OneClick;
        icon.gameObject.SetActive(true);
        icon.sprite = dot;
        icon.color = colorDot;
        icon.SetNativeSize();
      
        
    }
    void UpdateDisplayCell()
    {
        if(statusCell == StatusCell.OneClick)
        {
            icon.color = colorDot;
        }
        else if (statusCell == StatusCell.DoubleClick)
        {
            icon.color = colorStar;
        }
        inCellBgr.color = color;
    }
    public void ShowStarIcon()
    {
        icon.gameObject.SetActive(true);
        icon.color = colorStar;
        icon.sprite = star;
        icon.SetNativeSize();
        statusCell = StatusCell.DoubleClick;
        icon.DOFade(1f, 0.05f).From(0.5f).SetEase(Ease.InOutBack);
    }
   
    public void ResetStatus() {
        icon.gameObject.SetActive(false);
        errors = new List<NameError>();
        statusCell = StatusCell.None;
        inCellBgrWrong.gameObject.SetActive(false);
        beingWrong = false;
    }
    
    public void HightLightWrong(NameError nameError,bool showImmediately = false)
    {
        beingWrong = true;
        if (!errors.Contains(nameError)&& nameError!= NameError.Cross)
        {
            errors.Add(nameError);
        }
        else if (nameError == NameError.Cross)
        {
            errors.Add(nameError);
        }
        if (showImmediately)
        {
            ShowWrong();
        }
        else
        {

            StartCoroutine(DelayShowWrong());
        }
    }
    public void ShowWrong()
    {
        if (beingWrong == true)
        {
            // EffectShow();
            inCellBgr.color = Color.white;
            inCellBgrWrong.gameObject.SetActive(true);
            inCellBgrWrong.color = colorWrong;
            inCellBgrWrong.DOFade(1, 1f).From(0);
            EfextWrong();
        }
    }
    public void EfextWrong()
    {
        ManageAudio.Instacne.VibratMedium();
    }
    IEnumerator DelayShowWrong()
    {

        yield return new WaitForSeconds(0.7f);
        ShowWrong();


    }
    public void EffectShow()
    {
        transform.DOScale(1, 0.4f).From(0.8f).SetEase(Ease.OutElastic);
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
            inCellBgrWrong.gameObject.SetActive(false);

            inCellBgr.color = color;
        }
    }

    public void ShowHint(StatusCell stt)
    {

        if (stt == StatusCell.DoubleClick)
        {
            iconHint.gameObject.SetActive(true);
            iconHint.sprite = starHint;
            iconHint.SetNativeSize();
            inCellBgr.color = hintStarColor;
        }
        else if(stt == StatusCell.None) {
            inCellBgr.color = hintStarColor;
        }
        else if(stt == StatusCell.OneClick)
        {
            iconHint.gameObject.SetActive(true);
            iconHint.sprite = dotHint;
            iconHint.SetNativeSize();
            inCellBgr.color = hintStarColor;
        }
        inCellBgrWrong.gameObject.SetActive(false);
        EffectShow();
        StopAllCoroutines();
        StartCoroutine(ShowHintInDuration(2));
    }

    IEnumerator ShowHintInDuration(int time)
    {

        
        yield return new WaitForSeconds(time);
        if(beingWrong) 
            inCellBgr.color = colorWrong;
        else
            inCellBgr.color = color;
        iconHint.gameObject.SetActive(false);
        if(beingWrong)
            inCellBgrWrong.gameObject.SetActive(true);


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





    #region makeLevel

    public void MoveSetRegion()
    {
        if (!MakeLevelController.instance.modeSetRegion) return;
        SetRegion(MakeLevelController.instance.GetRegion());
        Vector2Int pos = GetPos();
        Cell l = boardMakeLevel.GetCellByPos(pos.x, pos.y - 1);
        Cell t = boardMakeLevel.GetCellByPos(pos.x - 1, pos.y);
        Cell r = boardMakeLevel.GetCellByPos(pos.x, pos.y + 1);
        Cell b = boardMakeLevel.GetCellByPos(pos.x + 1, pos.y);
        InitCell(l, t, r, b, boardMakeLevel);
        ReSetShowRegionForCell(l);
        ReSetShowRegionForCell(t);
        ReSetShowRegionForCell(r);
        ReSetShowRegionForCell(b);
    }
    
    public void SetStar()
    {
        if (MakeLevelController.instance.modeSetRegion) return;

        if (statusCell == StatusCell.DoubleClick)
        {
            statusCell = StatusCell.None;
            icon.gameObject.SetActive(false);
            GameConfig.instance.GetLevelCurrent().RemovePosStar(pos);
        }
        else
        {
            statusCell = StatusCell.DoubleClick;
            icon.gameObject.SetActive(true);
            icon.sprite = star;
            GameConfig.instance.GetLevelCurrent().SetStarPos(pos);

        }
        Check();
        board.CheckAround(pos);

    }
    public bool CheckMapInModeLeve()
    {
         
        bool c1= board.CheckRowLevel(pos.x);
        bool c2 =board.CheckColumnLevel(pos.y);
        bool c3 = board.CheckRegionLevel(this);
        return c1 && c2 && c3;
    }
    public bool CheckArrond()
    {
        return board.CheckAround(pos);
    }
    
    public void ReSetShowRegionForCell(Cell cell)
    {
        if (cell == null) return;
        Vector2Int pos = cell.GetPos();
        Cell l = boardMakeLevel.GetCellByPos(pos.x, pos.y - 1);
        Cell t = boardMakeLevel.GetCellByPos(pos.x - 1, pos.y);
        Cell r = boardMakeLevel.GetCellByPos(pos.x, pos.y + 1);
        Cell b = boardMakeLevel.GetCellByPos(pos.x + 1, pos.y);
        cell.InitCell(l, t, r, b, boardMakeLevel);
    }

    #endregion


    public void DelayCall(Action call, float time = 1f)
    {
        StartCoroutine(IeDelayCall(call,time));
    }
    IEnumerator IeDelayCall(Action call, float time)
    {
        yield return new WaitForSeconds(time);
        call?.Invoke();
    }


}
