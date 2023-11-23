using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableLevel : MonoBehaviour
{
    public ButonLevel btnLevelPrefab;
    public List<ButonLevel> btnLevels;
    public GameObject content;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i< 10; i++)
        {
            btnLevels.Add(Instantiate(btnLevelPrefab, content.transform));
            btnLevels[i].SetLevel(i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
