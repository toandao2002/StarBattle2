using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Title : MonoBehaviour
{
    public TMP_Text level;
    public void SetLevel(int numlevel)
    {
        level.text = "Level : " + numlevel;
    }
}
