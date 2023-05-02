using UnityEngine;
using UnityEngine.Rendering.Universal;
using Pixelplacement;
using UnityEngine.Device;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance { get; private set; }

    [SerializeField] private UniversalRendererData rendererData;
    [Range(0f, 5f)] public float duration = 0.5f;

    private ScreenFadeFeature screenFadeFeature;
    private Material fadeMaterial;

    private void Start()
    {
        // Find the, and set the feature's material
        SetupFadeFeature();
    }

    private void SetupFadeFeature()
    {
        // Look for the screen fade feature
        ScriptableRendererFeature feature = rendererData.rendererFeatures.Find(item => item is ScreenFadeFeature);
        screenFadeFeature = (ScreenFadeFeature)feature;

        // Duplicate material so we don't change the renderer's asset
        fadeMaterial = Instantiate(screenFadeFeature.settings.material);
        screenFadeFeature.settings.runTimeMaterial = fadeMaterial;
        screenFadeFeature.settings.isEnabled = false;
    }

    /// <summary>
    /// Fade to black
    /// </summary>
    public float FadeIn()
    {
        screenFadeFeature.settings.isEnabled = true;
        Tween.ShaderFloat(fadeMaterial, "_Alpha", 1, duration, 0);
        return duration;
    }

    /// <summary>
    /// Fade to clear
    /// </summary>
    public float FadeOut()
    {
        StartCoroutine(Coroutine_DisableFeature());
        Tween.ShaderFloat(fadeMaterial, "_Alpha", 0, duration, 0);
        return duration;
    }

    private IEnumerator Coroutine_DisableFeature()
    {
        yield return new WaitForSeconds(duration);
        screenFadeFeature.settings.isEnabled = false;

    }
}
