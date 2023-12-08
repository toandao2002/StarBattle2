using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;
    public ManageUi manageUi;
    public void LoadScene(string nameScene)
    {
        SceneManager.LoadScene(nameScene);
    }
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    public void ResetData()
    {
        DataGame.resetData();
    }
    private void Start()
    {
        if (DataGame.GetInt(DataGame.FTurtorial) == 0)
        {
            manageUi.ShowPopUp(NamePopUp.Tutoria);
            DataGame.SetInt(DataGame.FTurtorial, 1);
            DataGame.Save();
        }
        else
        { 
        }
    }
    public void Update()
    {
        // Kiểm tra nút quay trở lại trên Android
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Xử lý sự kiện khi nút quay trở lại được nhấn
            MyEvent.ClickBack?.Invoke();
            Debug.Log("back");
        }
    }
}
