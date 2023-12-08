using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellTut : MonoBehaviour
{
    // only use in make level; 
    public Sprite star;
    public Sprite dot; 
    public StatusCell statusCell;
    public int region;
    public Image inCellBgr;
    public Image inCellBgrWrong;
    public Image icon; 
    public int disInRegioin;
    public int disOutRegioin; 
    public Vector2Int pos;
    BoardTut board;
    bool beingWrong; 
    public List<Color> colorCells;
    Color color;
    public Color colorWrong; 
    public List<NameError> errors = new List<NameError>();


    public List<GameObject> border;
    public GameObject borderPar;
    public Image Bgr;
    public MyButton btn;
    private void Awake()
    {

        disInRegioin = 1;
        disOutRegioin = 2;
        icon.gameObject.SetActive(false);
    }
    // Start is called before the first frame update



    public bool CheckRegion(int region)
    {
        return this.region == region;
    }
    public Vector2 GetDis(CellTut cell, Vector2 offset, int v)
    {
        Vector2 offsetRs = offset;

        if (cell != null) //
        {
            if (cell.CheckRegion(region)) // same region => boder is thin
            {
                if (v == -1)
                    offsetRs.x = disInRegioin;
                else
                    offsetRs.y = disInRegioin;
            }
            else  // difference region => boder is stroke
            {
                if (v == -1)
                    offsetRs.x = 0;
                else
                    offsetRs.y = 0;
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
    public bool GetLogicBoder(CellTut cell, Vector2 offset, int v)
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
    public void InitCell(CellTut l, CellTut t, CellTut r, CellTut b, BoardTut board)
    {
        this.board = board;
        if (!beingWrong)
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
       


        ChangeRectranform(oMin, oMax);
        if (region >= 9)
            color = colorCells[0];
        else
            color = colorCells[region];
        inCellBgr.color = color;
    }
    public void ChangeRectranform(Vector2 omin, Vector2 omax)
    {

        border[0].SetActive(omin.x == disOutRegioin);

        border[3].SetActive(omin.y == disOutRegioin);

        border[2].SetActive(omax.x == -disOutRegioin);

        border[1].SetActive(omax.y == -disOutRegioin);
        int sizeWith = 4;

        if (omax.y == 0)
        {
            border[1].SetActive(true);
            border[1].GetComponent<RectTransform>().sizeDelta = new Vector2(0, sizeWith);
            border[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            border[1].GetComponent<RectTransform>().offsetMin = new Vector2(-sizeWith / 2, 0);
            border[1].GetComponent<RectTransform>().offsetMax = new Vector2(sizeWith / 2, sizeWith);
        }
        if (omax.x == 0)
        {
            border[2].SetActive(true);
            border[2].GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            border[2].GetComponent<RectTransform>().sizeDelta = new Vector2(sizeWith, 0);
            border[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }

        if (omin.x == 0)
        {
            border[0].SetActive(true);
            border[0].GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            border[0].GetComponent<RectTransform>().sizeDelta = new Vector2(sizeWith, 0);
            border[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        if (omin.y == 0)
        {
            border[3].SetActive(true);

            border[3].GetComponent<RectTransform>().offsetMin = new Vector2(-sizeWith / 2, 0);
            border[3].GetComponent<RectTransform>().offsetMax = new Vector2(sizeWith / 2, sizeWith);
            border[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        /*inCellBgr.GetComponent<RectTransform>().offsetMin = omin;
        inCellBgr.GetComponent<RectTransform>().offsetMax = omax;*/
    }


    #region handle click
    public void Click()
    {
       
        if (statusCell == StatusCell.OneClick && !beEffectByMoveMouse)
        {
            DoubleClick(true);
        }
        else if (statusCell == StatusCell.DoubleClick && !beEffectByMoveMouse)
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
    }
    public void DonClick(bool isUser)// để phần biệt user và chức năng redo
    {
        if (board.dontClick) return;
        statusCell = StatusCell.None;
        icon.sprite = null;
        icon.gameObject.SetActive(false);
        ResetColorBgr();
        StopAllCoroutines();
        Check();
        if (isUser) // because when user: from star-> none so will check only user 
            board.CheckAround(pos);
        MyEvent.DontClickTut?.Invoke();

    }

    public void OneClick(bool isUser)
    {
        if (board.dontClick) return;
        ShowDotIcon();
        ResetColorBgr();
        StopAllCoroutines();

        Check();
        if (!isUser)// because when isn't user: from star-> dot so will check only isn't user
            board.CheckAround(pos);
        MyEvent.OneClickTut?.Invoke();

    }
    public void DoubleClick(bool isUser)
    {
        if (board.dontClick) return;
        ShowStarIcon();
        ResetColorBgr();
        StopAllCoroutines();
       

        Check();
        board.CheckAround(pos);
        
        MyEvent.DoubleClickTut?.Invoke();

    }
    bool beEffectByMoveMouse;
    public void MoveOnCell()
    { 
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
        bool win = board.CheckWin();
        if (win)
        {
            Debug.Log("win ");
            MyEvent.CheckWinTut?.Invoke();
        }
    }
    #endregion


    #region display
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
        icon.SetNativeSize();
        icon.transform.DOScale(1, 0.2f).From(0).SetEase(Ease.InOutBack);
    }
    public void ShowStarIcon()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = star;
        icon.SetNativeSize();
        statusCell = StatusCell.DoubleClick;
        icon.transform.DOScale(1, 0.1f).From(0).SetEase(Ease.InOutBack);
    }

    public void ResetStatus()
    {
        icon.gameObject.SetActive(false);
        errors = new List<NameError>();
        statusCell = StatusCell.None;
        inCellBgrWrong.gameObject.SetActive(false);
        beingWrong = false;
    }

    public void HightLightWrong(NameError nameError, bool showImmediately = false)
    {
        beingWrong = true;
        if (!errors.Contains(nameError) && nameError != NameError.Cross)
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
        }
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
        if (errors.Count == 0 && beingWrong == true)
        {

            beingWrong = false;
            inCellBgrWrong.gameObject.SetActive(false);

            inCellBgr.color = color;
        }
    }

    

   
    #endregion


    public void SetRegion(int region)
    {
        this.region = region;
        if(region == 1)
        {
            borderPar.SetActive(false);
            inCellBgr.gameObject.SetActive(false);
            Bgr.color = new Color(0, 0, 0, 0);
            btn.enabled = false;
        }
    }
    public Vector2Int GetPos()
    {
        return pos;
    }
    public void SetPos(Vector2Int p)
    {
        pos = p;
    }
 


    public void DelayCall(Action call, float time = 1f)
    {
        StartCoroutine(IeDelayCall(call, time));
    }
    IEnumerator IeDelayCall(Action call, float time)
    {
        yield return new WaitForSeconds(time);
        call?.Invoke();
    }

}
