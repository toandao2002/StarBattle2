using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public static GameConfig instance;
    public DataBoard dataBoard;
    
    private void Awake()
    {
        instance = this;
    }
}
