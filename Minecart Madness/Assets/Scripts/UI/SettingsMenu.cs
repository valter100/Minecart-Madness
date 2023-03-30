using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Toggle environmentToggle;
    [SerializeField] private Toggle hdrToggle;
    [SerializeField] private Toggle postProcessingToggle;
    [SerializeField] private TMP_Dropdown shadowsDropdown;
    [SerializeField] private TMP_Dropdown renderScaleDropdown;
    private UniversalRenderPipelineAsset urpAsset;
    private GameObject environment;
    
    private void Start()
    {
        environment = GameObject.Find("Environment");
        urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;

        environmentToggle.isOn = environment.activeSelf;
        hdrToggle.isOn = urpAsset.supportsHDR;
        postProcessingToggle.isOn = Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing;

        shadowsDropdown.value = (int)GameObject.Find("Directional Light").GetComponent<Light>().shadows;
        shadowsDropdown.RefreshShownValue();
    }

    public void SetEnvironment(bool value)
    {
        environment.SetActive(value);
    }

    public void SetHDR(bool value)
    {
        urpAsset.supportsHDR = value;
    }

    public void SetPostProcessing(bool value)
    {
        GameObject.Find("Post Processing").GetComponent<Volume>().enabled = value;
        //Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = value;
    }

    public void SetShadows(int value)
    {
        GameObject.Find("Directional Light").GetComponent<Light>().shadows = (LightShadows)value;
    }

    public void SetRenderScale(int value)
    {
        urpAsset.renderScale = float.Parse(renderScaleDropdown.options[value].text, CultureInfo.InvariantCulture.NumberFormat);
    }
}
