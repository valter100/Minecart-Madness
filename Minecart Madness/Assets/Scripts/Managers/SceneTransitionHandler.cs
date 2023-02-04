using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionHandler : MonoBehaviour
{
    [SerializeField] string defaultSceneName;
    public static void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }
}
