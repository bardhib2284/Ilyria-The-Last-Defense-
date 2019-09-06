using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);
    }
    public void GoToFightScene()
    {
        StartCoroutine(LoadAsynchronously("FIGHT_SCENE"));
    }

    public void GoToHomeScene()
    {
        StartCoroutine(LoadAsynchronously("HOME_SCENE"));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        var g = SceneManager.LoadSceneAsync(sceneName);
        while (!g.isDone)
        {
            Debug.Log(g.progress);
            yield return null;
        }
    }
}
