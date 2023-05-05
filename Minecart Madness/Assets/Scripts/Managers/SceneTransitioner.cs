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
        // Fade in
        screenFade.FadeIn();
        yield return new WaitForSeconds(screenFade.duration);

        // Wait two extra frames to ensure the screen is completely black before loading scene
        yield return null;
        yield return null; 
        SceneManager.LoadScene(sceneName);

        // Wait for scene to load and player to spawn before fading out (estimate)
        yield return new WaitForSeconds(1.5f); 
        screenFade.FadeOut();
    }

}
