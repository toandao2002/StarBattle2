using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BoxBigLevelHome : MonoBehaviour
{
    public Image bar; 
    public TypeGame typeGame;
    public void SetRateBarLevel(float value)
    {
        bar.fillAmount = value;
    }
}

