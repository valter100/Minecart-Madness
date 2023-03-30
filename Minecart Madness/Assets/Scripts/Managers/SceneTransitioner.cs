using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public static SceneTransitioner Instance { get; private set; }

    [SerializeField] public ScreenFade screenFade;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        screenFade.FadeOut();
    }

    public void InstantLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void FadeLoad(string sceneName)
    {
        if (screenFade && screenFade.duration != 0f)
            StartCoroutine(Coroutine_FadeLoad(sceneName));
        else
            SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Coroutine_FadeLoad(string sceneName)
    {
        screenFade.FadeIn();
        yield return new WaitForSeconds(screenFade.duration);
        yield return null;
        SceneManager.LoadScene(sceneName);
        screenFade.FadeOut();
    }

}
