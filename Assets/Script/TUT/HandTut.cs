using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class HandTut : MonoBehaviour
{

    public RectTransform rec;
    public Transform s;
    public Transform e;
    public float timeClick;
    public float timeTwoClick;
    public float timeMove;
    public Image hand;
    public Vector3 posInit;
    private void OnEnable()
    {
        posInit = rec.position;
    }
    [Button]
    public void ActionClick()
    {
        timeClick = 0.3f;
        rec.DOScale(1f, timeClick).SetEase(Ease.InOutBack).SetLoops(1,LoopType.Yoyo).From(0.8f);
    }
    public void ActionTwoClick()
    {
        timeTwoClick = 0.3f;
        rec.DOScale(1, timeTwoClick).SetEase(Ease.InOutBack).SetLoops(3, LoopType.Yoyo).From(0.8f);
    }
    public void HideHand()
    {
        
        
        hand.DOFade(0, 0.4f);
    }
    public void ShowHand()
    {
        hand.DOFade(1, 0.4f).From(0);
        rec.position = posInit;
    }
    
  /*  [Button]
    public void MoveHand()
    {
        Vector3 posS; Vector3 posE;
        posS = s.position;
        posE = e.position;
        timeTwoClick = 0.3f;
        rec.DOMove(posE, timeTwoClick).From(posS).SetEase(Ease.InOutQuart);
    }*/
    public void MoveHand(Vector3 posS, Vector3 posE)
    {
        timeMove = 1f;
        rec.DOMove(posE, timeMove).From(posS).SetEase(Ease.InOutQuart);
    }
}