using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;
    public void LoadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
}
