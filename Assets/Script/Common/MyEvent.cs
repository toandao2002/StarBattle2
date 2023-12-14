using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEvent : MonoBehaviour
{
    public static Action<System.Object> GameWin;
    public static Action UpdateSetingData;
    public static Action UpdateDataAmountHint;


    public static Action CheckWinTut;
    public static Action OneClickTut;
    public static Action DoubleClickTut;
    public static Action DontClickTut;

    public static Action ChangeTheme;
    public static Action UpdataLocalize;


    public static Action ClickBack;
    public static Action ClickCell;
    public static Action DontTouch;

    public static Action BuyPack;
}
