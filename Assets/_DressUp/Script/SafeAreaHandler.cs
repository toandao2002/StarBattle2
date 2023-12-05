using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaHandler : MonoBehaviour
{
    [SerializeField]
    Rect rect;
    [SerializeField]
    Vector2 min, max;
    // Start is called before the first frame update
    [SerializeField]
    RectTransform rt;
    void Start()
    {
        //Debug.Log("SAFE AREA: " + Screen.safeArea.size+" ");
        this.rect = Screen.safeArea;
        rt = GetComponent<RectTransform>();
        Debug.Log("SAFE AREA: " + Screen.safeArea.size + " " + gameObject.name);
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, 0);
            rt.offsetMax = new Vector2(rt.offsetMax.x, -((Screen.height - rect.height) - rect.y) / 2f);
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            rt.offsetMin = new Vector2((Screen.width - rect.width), 0);
            rt.offsetMax = new Vector2(rt.offsetMax.x, -((Screen.height - rect.height) - rect.y) / 2f);
        }
        min = rt.offsetMin;
        max = rt.offsetMax;
        //rt.offsetMin = new Vector2(0, rect.y);
        //rt.offsetMax = new Vector2(0, -rect.y);
    }
    private void Update()
    {
        rt.offsetMin = min;
        rt.offsetMax = max;

    }
}