using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    [SerializeField] string defaultSceneName;
    public static void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public static void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public static void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
