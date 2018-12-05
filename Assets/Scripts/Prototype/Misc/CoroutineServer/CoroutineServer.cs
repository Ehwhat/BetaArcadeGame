using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Misc/CoroutineServer/New Coroutine Server Asset")]
public class CoroutineServer : ScriptableObject {

    private static CoroutineRunner coroutineRunner;
    public static List<IEnumerator> runningCoroutines;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        if(coroutineRunner != null)
        {
            return;
        }
        coroutineRunner = new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();
        coroutineRunner.gameObject.hideFlags = HideFlags.HideAndDontSave;
        runningCoroutines = new List<IEnumerator>();
    }

    public static Coroutine StartCoroutine(IEnumerator routine)
    {
        
        Coroutine coroutine = coroutineRunner.StartCoroutine(routine);
        return coroutine;
    }

    public static void StopCoroutine(IEnumerator routine)
    {
        coroutineRunner.StopCoroutine(routine);
    }

    public static void StopCoroutine(Coroutine coroutine)
    {
        coroutineRunner.StopCoroutine(coroutine);
    }

    public static void StopAllCoroutines()
    {
        coroutineRunner.StopAllCoroutines();
    }

}
