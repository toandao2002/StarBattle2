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
    private void OnEnable()
    {
        idTutCurrent = -1;
        NextTut();
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
        meseage1.text = tutorialDatas[idTutCurrent].mesage1[idMesage1];
       
        mesage1H.enabled = false;
        StartCoroutine(DelayOneFrame(() => {
            Canvas.ForceUpdateCanvases();
            mesage1H.enabled = true;
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
        meseage1.text = tutorialDatas[idTutCurrent].mesage1[idMesage1];
        mesage1H.enabled = false;
        StartCoroutine(DelayOneFrame(() => {
            Canvas.ForceUpdateCanvases();
            mesage1H.enabled = true;
        }));
        int numM = 0;
        nextTut.gameObject.SetActive(false);
        TutorialData tutData = tutorialDatas[idTutCurrent];
        for(numM =0; numM<tutData.mesage2.Count; numM++)
        {
            mesage2s[numM].gameObject.SetActive(true);
            mesage2s[numM].text = tutData.mesage2[numM];
        }
        for (numM = numM; numM< mesage2s.Count; numM++)
        {
            mesage2s[numM].gameObject.SetActive(false);

        }
        mesage2.enabled = false;
        StartCoroutine( DelayOneFrame(()=> {
            Canvas.ForceUpdateCanvases();
            mesage2.enabled = true;
        }));

        boardTut.Init(tutData);
       
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

