using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEvent : MonoBehaviour
{
    public static Action<System.Object> GameWin;
    public static Action UpdateSetingData;


    public static Action CheckWinTut;
    public static Action OneClickTut;
    public static Action DoubleClickTut;
    public static Action DontClickTut;
}
