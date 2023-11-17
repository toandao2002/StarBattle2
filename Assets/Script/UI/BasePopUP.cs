using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum NamePopUp
{
    GameWin,

}
public class BasePopUP : MonoBehaviour
{
    public NamePopUp namePopUp;
    public GameObject main;
    public GameObject Bgr;
    private float durationEffect = 0.5f;
    public void Hide() {

        main.transform.DOScale(0.4f, durationEffect).From(1).OnComplete(()=> {
            main.SetActive(false);
            Bgr.SetActive(false);
        }); 
    }
    public void Show()
    {
        Bgr.SetActive(true);
        main.SetActive(true);
        main.transform.DOScale(1f, durationEffect).From(0.4f); 
    }
}
