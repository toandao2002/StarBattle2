using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;
using System;

public class MyButton : Button
{
    bool Delay = true;
    public float TimeDeLay = 0;
    public bool hasEffect = true;
    public UnityEvent onEnter = new UnityEvent(), onDown = new UnityEvent(),
        onExit = new UnityEvent(), onUp = new UnityEvent();
    Vector3 initScale;
    public bool NoHasSound;
    protected override void Awake()
    {
        initScale = transform.localScale;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {

        base.OnPointerClick(eventData);
        if (!NoHasSound)
        {
            ManageAudio.Instacne?.PlaySound(NameSound.Click);
        }

    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onDown.Invoke();
        EffectDown();
        
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        onEnter.Invoke();
        EffectDown();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onUp.Invoke();
        EffectUp();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        onExit.Invoke();
        EffectUp();
    }

    
    void EffectDown()
    {
        //SoundManageLogic.Instance?.PlayButton(SoundManageLogic.Instance.btnClickSound);
        //ScaleUp();
    }

    void EffectUp()
    {
        ScaleDown();
    }

    void ScaleUp()
    {
        if (hasEffect)
        {
            transform.localScale = initScale;
            transform.DOScale(initScale * 0.9f, 0.1f).SetEase(Ease.InBounce);
        }
    }
    void ScaleDown()
    {
        if (hasEffect)
        {
            transform.localScale = initScale * 0.9f;
            transform.DOScale(initScale, 0.4f).SetEase(Ease.OutElastic);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        transform.DOKill();
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(MyButton))]
public class ButtonEffectLogicEditor : Editor
{
    MyButton mtarget;
    private void OnEnable()
    {
        mtarget = target as MyButton;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }
}
#endif