using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpContinuePlay : BasePopUP
{
    public static PopUpContinuePlay instance;
    public override void Awake()
    {
        base.Awake();
        instance = this;
    }
}
