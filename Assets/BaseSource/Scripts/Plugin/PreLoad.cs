
using UnityEngine;

public class PreLoad
{
    #region Method Run with Unity Cycles
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
    private static void RunAfterSubsystemRegistration()
    {
        Debug.LogWarning("Preload Subsystem Registration");
    }
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
#endif
    private static void RunAfterAssembliesLoaded()
    {
        Debug.LogWarning("Preload Assemblies Loaded");

        InitFaceBook();
    }

#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod()]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
#endif
    private static void RunBeforeSplashScreen()
    {
        Debug.LogWarning("Preload Before Splash Screen");


    }
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
    private static void RunBeforerScreenLoad()
    {
        Debug.LogWarning("Preload Before Screen Load");

        InitFirebase();
    }
#if UNITY_EDITOR
    [RuntimeInitializeOnLoadMethod]
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
#endif
    private static void RunAfterScreenLoad()
    {
        Debug.LogWarning("Preload After Screen Load");
    }
    #endregion


    static void InitFaceBook()
    {
        Debug.LogWarning("Load facebook");
        var fb = new FacebookManager();
        fb.Init();
    }

    static void InitFirebase()
    {
        
        Debug.LogWarning("Load Firebase");
        var fb = new FireBaseManager();
        fb.InitFirebase();

        FireBaseManager.Instance = fb;
    }

}
