using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "Data/TutorialData", order = 1)]

[System.Serializable]
public class TutorialData : ScriptableObject
{
    public List<string> mesage1;
    public List <string> mesage2;
    public Vector2Int sizeBoard;
    public List<Vector2Int> posStar;
    [SerializeField]
    public List<int> region;
}
