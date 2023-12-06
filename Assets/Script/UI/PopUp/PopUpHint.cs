using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpHint : BasePopUP
{
    public static PopUpHint instance;
    public override void Awake()
    {
        base.Awake();
        instance = this;
    }
    public void  WatchAdGetHint()
    {
        DataGame.SetInt(DataGame.AmountHint, DataGame.GetInt(DataGame.AmountHint)+10);
        MyEvent.UpdateDataAmountHint?.Invoke();
        Hide();
    }

}
