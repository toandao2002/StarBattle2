using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public bool isON;
    public MyButton btn;
    public GameObject Circle;
    public Action TurnOn;
    public Action TurnOff;
    public TMP_Text state;
    public Image bgr;
    public List<Sprite> sprites;
    private void Start()
    {
        btn.onClick.AddListener(Click);
    }
    public void Click()
    {
        isON = !isON;
        float duration = 0.3f;
        if (isON)
        {
            bgr.sprite = sprites[0];
            state.text = "ON";
            TurnOn?.Invoke() ;
            Circle.transform.DOLocalMoveX(18, 0.3f);
        }
        else
        {
            bgr.sprite = sprites[1];
            state.text = "OFF";
            Circle.transform.DOLocalMoveX(-18, 0.3f);
            TurnOff?.Invoke();
        }
    }
    
}
