using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using DG.Tweening;
public class TutorialPanel : BasePopUP
{
    public BoardTut boardTut;
    public Sprite bgr1violet;
    public Sprite bgrgreen;
    public Image bgrBox;
    public List<TutorialData> tutorialDatas;
    public List<GameObject> previews;
    public TMP_Text meseage1;
    public List<TMP_Text> mesage2s;
    public VerticalLayoutGroup mesage2;
    public VerticalLayoutGroup mesage1H;
    // Start is called before the first frame update
    public int idTutCurrent;
    public int idMesage1;
    public int idPreview;
    public MyButton nextTut;
    public HandTut hand;
    private void OnEnable()
    {
        idTutCurrent = -1;
       
        MyEvent.CheckWinTut += NextChildTut;
    }
    private void OnDisable()
    {
        
    }
    

    void Start()
    {
        
    }
    public void NextChildTut()
    {
        idMesage1++;
        meseage1.text = Util.GetLocalizeRealString(tutorialDatas[idTutCurrent].mesage1[idMesage1]);
       
        mesage1H.enabled = false;
        Canvas.ForceUpdateCanvases();
        mesage1H.enabled = true;
        StartCoroutine(DelayOneFrame(() => {
            
           
        }));
        if (idMesage1 == tutorialDatas[idTutCurrent].mesage1.Count - 1)
        {
            ShowNextTut();
            bgrBox.sprite = bgrgreen;
            boardTut.dontClick = true;
        }
        else
        {
            bgrBox.sprite = bgr1violet;
        }
    }
    public void PlayLevel1()
    {
        GameConfig.instance.SetLevelCurrent(1);
        GameConfig.instance.SetTimeFiishCurrent(0);
        GameConfig.instance.GetLevelCurrent().datalevel.isfinished = false;
        GameConfig.instance.nameModePlay="Easy";
        GameManger.instance.manageUi.ShowPopUp(NamePopUp.GamePlay);
        GameManger.instance.manageUi.HidePopUP(NamePopUp.Tutoria);

    }
    public void NextFinishPreview()
    {
        idPreview=2;
        previews[idPreview - 2].GetComponent<RectTransform>().DOAnchorPos3DX(Screen.width, durationEffect).From(0).SetEase(ease).OnComplete(() => {
            previews[idPreview - 2].SetActive(false);
        });
        previews[idPreview].SetActive(true);
        previews[idPreview].GetComponent<RectTransform>().DOAnchorPos3DX(0, durationEffect).From(-Screen.width).SetEase(ease).OnComplete(() => {

        });
    }
    public void NextPreview()
    {
        idPreview++;
        if (idPreview == previews.Count)
        {
            PlayLevel1();
            return;
        } 

        previews[idPreview - 1].GetComponent<RectTransform>().DOAnchorPos3DX(Screen.width, durationEffect).From(0).SetEase(ease) .OnComplete(() => {
            previews[idPreview - 1].SetActive(false);
        });
        previews[idPreview].SetActive(true);
        previews[idPreview ].GetComponent<RectTransform>().DOAnchorPos3DX(0, durationEffect).From(-Screen.width).SetEase(ease) .OnComplete(() => {
            
        });
    }
    // Update is called once per frame
    public void NextTut()
    {
        boardTut.dontClick = false;
        bgrBox.sprite = bgr1violet;
        idTutCurrent++;
        idMesage1 = 0;
        if (idTutCurrent == 0)
        {
            MyEvent.DontClickTut = NextChildTut;
            MyEvent.OneClickTut = NextChildTut;
            MyEvent.DoubleClickTut = NextChildTut;
        }
        else
        {
            MyEvent.DontClickTut = null;
            MyEvent.OneClickTut = null;
            MyEvent.DoubleClickTut = null;
        }
        if(idTutCurrent == tutorialDatas.Count)
        {
            NextPreview();
            return;
        }
        meseage1.text = Util.GetLocalizeRealString(tutorialDatas[idTutCurrent].mesage1[idMesage1]);
        mesage1H.enabled = false;
        Canvas.ForceUpdateCanvases();
        mesage1H.enabled = true;
        StartCoroutine(DelayOneFrame(() => {
           
        }));
        int numM = 0;
        nextTut.gameObject.SetActive(false);
        TutorialData tutData = tutorialDatas[idTutCurrent];
        for(numM =0; numM<tutData.mesage2.Count; numM++)
        {
            mesage2s[numM].gameObject.SetActive(true);
            mesage2s[numM].text = Util.GetLocalizeRealString(tutData.mesage2[numM]);
        }
        for (numM = numM; numM< mesage2s.Count; numM++)
        {
            mesage2s[numM].gameObject.SetActive(false);

        }
        mesage2.enabled = false;
        Canvas.ForceUpdateCanvases();
        mesage2.enabled = true;
        StartCoroutine( DelayOneFrame(()=> {
            
        }));

        boardTut.Init(tutData);
        StopAllCoroutines();
        StartCoroutine(HandleHand(boardTut, tutData));
    }
    public IEnumerator HandleHand(BoardTut board, TutorialData tutData)
    {
        hand.ShowHand();
        yield return new WaitForSeconds(0.5f);
        if (tutData.posStar.Count >= 2)
        {
    
            
            Vector2Int idPosS = tutData.posStar[0];
            Vector2Int idPosE = tutData.posStar[1];
            Vector3 posS = board.cells[idPosS.x][idPosS.y].gameObject.transform.position;
            Vector3 posE = board.cells[idPosE.x][idPosE.y].gameObject.transform.position;
            hand.MoveHand(hand.posInit, posS);
            yield return new WaitForSeconds(hand.timeMove);
            hand.ActionTwoClick();
            yield return new WaitForSeconds(hand.timeTwoClick*4);

            hand.MoveHand(posS, posE);
            yield return new WaitForSeconds(hand.timeMove);
            hand.ActionTwoClick();
            yield return new WaitForSeconds(hand.timeTwoClick * 4);
            hand.MoveHand(
              board.cells[idPosE.x][idPosE.y].gameObject.transform.position, hand.posInit);
            yield return new WaitForSeconds(hand.timeMove);
            hand.HideHand();


        }
        else
        { 
            Vector2Int idPosE = tutData.posStar[0];
            hand.MoveHand(hand.posInit,
                board.cells[idPosE.x][idPosE.y].gameObject.transform.position);
            yield return new WaitForSeconds(hand.timeMove);
            hand.ActionClick();
            yield return new WaitForSeconds(hand.timeClick * 4);
            hand.MoveHand(
                board.cells[idPosE.x][idPosE.y].gameObject.transform.position, hand.posInit);
            yield return new WaitForSeconds(hand.timeMove);
            hand.HideHand();

        }
    }
    public IEnumerator DelayOneFrame(Action call1  )
    {
        yield return null;
        call1?.Invoke();
       
    }
    void ShowNextTut()
    {
        nextTut.gameObject.SetActive(true);
    }

}

