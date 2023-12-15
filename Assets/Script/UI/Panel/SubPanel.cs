using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPanel : BasePopUP
{
    public static SubPanel instance;
    private void Awake()
    {
        instance = this;
    }
}
