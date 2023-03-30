using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Threading.Tasks;

public class Exit : MonoBehaviour
{
    /// <summary>
    /// Exits this application.
    /// </summary>
    public static void ExitApplication()
    {
        #if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

        #else

        Application.Quit();

        #endif
    }

    /// <summary>
    /// Exits this application after a screen fade
    /// </summary>
    public static async void FadeExit()
    {
        SceneTransitioner.Instance.screenFade.FadeIn();
        float milliseconds = SceneTransitioner.Instance.screenFade.duration * 1000f;
        await Task.Delay((int)milliseconds);
        ExitApplication();
    }

}
