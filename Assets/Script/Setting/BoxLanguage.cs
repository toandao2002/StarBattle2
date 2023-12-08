using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxLanguage : BoxSetting
{
    // Start is called before the first frame update
    public GameObject popUP;
    public bool isOpenPopUp;
    public void ShowPopUpChoseLanguage()
    {
        isOpenPopUp = !isOpenPopUp;
        popUP.SetActive(isOpenPopUp);
    }
    
}
