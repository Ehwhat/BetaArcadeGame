using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Tanks/Singletons/Level Loader")]
public class LevelLoader : ScriptableObject {

    public int loadingSceneIndex;
    public float minLoadingTime = 4f;

    [System.NonSerialized]
    private float loadTimeStart = 0;
    [System.NonSerialized]
    private bool alreadyLoading = false;

    public void ChangeLevel(int levelIndex)
    {
        if (!alreadyLoading)
        {
            
            loadTimeStart = Time.time;
            alreadyLoading = true;
            var op = SceneManager.LoadSceneAsync(loadingSceneIndex);
            op.completed += (AsyncOperation o) => { CoroutineServer.StartCoroutine(DelayLoading(levelIndex)); };
            
        }

    }

    private IEnumerator DelayLoading(int levelIndex)
    {
        AsyncOperation levelLoadingOperation = SceneManager.LoadSceneAsync(levelIndex);
        levelLoadingOperation.allowSceneActivation = false;
        while (levelLoadingOperation.progress < 0.9f)
        {
            Debug.Log(levelLoadingOperation.progress);
            yield return null;
        }
        float timeToLoadLevel = Time.time - loadTimeStart;
        Debug.Log("Time taken to Load Level " + timeToLoadLevel);
        float delayTime = Mathf.Max(minLoadingTime - timeToLoadLevel, 0);
        Debug.Log(delayTime);
        yield return new WaitForSeconds(delayTime);
        levelLoadingOperation.allowSceneActivation = true;
        alreadyLoading = false;
    }
}
