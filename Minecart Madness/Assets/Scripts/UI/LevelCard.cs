using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class LevelCard : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Toggle[] stars;

    public void SetStars(int nrOfStars)
    {
        for (int i = 0; i < stars.Length; ++i)
        {
            stars[i].isOn = i < nrOfStars;
        }
    }

    public void OnClick()
    {
        CanvasAudio.Instance.Play("Confirm");

        LevelsMenu levelsMenu = GameObject.Find("Canvas").transform.Find("Levels Menu").GetComponent<LevelsMenu>();

        //if (levelsMenu.IsMultiplayer())
            StartCoroutine(Coroutine_DelayedCreateRelay());

        SceneTransitioner.Instance.FadeLoad(sceneName);
    }

    private IEnumerator Coroutine_DelayedCreateRelay()
    {
        yield return new WaitForSeconds(SceneTransitioner.Instance.screenFade.duration);
        yield return null;
        NetworkManager.Singleton.GetComponent<TestRelay>().CreateRelay();
        yield return 0;
    }
}
