using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ButonLevel : MonoBehaviour
{
    public TMP_Text txtLevel;
    int level;
    public void SetLevel(int id)
    {
        txtLevel.text = id.ToString();
        level = id-1;
    }
    public void ChoseLevel()
    {
        GameConfig.instance.SetLevelCurrent(level);
        GameManger.instance.LoadScene("GamePlay");
    }
}
