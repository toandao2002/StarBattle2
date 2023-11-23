using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableObjectController : MonoBehaviour
{
    public T CreateOrReplaceAsset<T>(T asset, string path) where T : ScriptableObject
    {
#if UNITY_EDITOR
        T existingAsset = AssetDatabase.LoadAssetAtPath<T>(path);

        if (existingAsset == null)
        {
            AssetDatabase.CreateAsset(asset, path);
            existingAsset = asset;
        }
        else
        {
            EditorUtility.CopySerialized(asset, existingAsset);
        }

        return existingAsset;
#else
        return null;
#endif
    }
    public string GetPathLevel(TypeGame typeGame, int level)
    {
        string path = "Assets/GameConfig/"+typeGame.ToString()+"/Level " + level + ".asset";
        return path;
    }
}
