using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Toggle environmentToggle;
    [SerializeField] private Toggle hdrToggle;
    [SerializeField] private Toggle postProcessingToggle;
    [SerializeField] private Toggle ssaoToggle;
    [SerializeField] private TMP_Dropdown shadowsDropdown;
    [SerializeField] private TMP_Dropdown renderScaleDropdown;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI screenResolutionText;
    [SerializeField] private TextMeshProUGUI gameResolutionText;

    [SerializeField] UniversalRendererData rendererData;

    [SerializeField] private float ssaoIntensity = 0.8f;
    [SerializeField] private float fpsRefreshRate = 4;

    private SSAOConfigurator ssaoConfigurator;
    private UniversalRenderPipelineAsset urpAsset;
    private GameObject environment;
    private float timer;
    
    private void Awake()
    {
        environment = GameObject.Find("Environment");
        urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.renderPipelineAsset;
        ssaoConfigurator = new SSAOConfigurator();
        screenResolutionText.text = Screen.currentResolution.ToString();
    }

    private void OnEnable()
    {
        ResetToggle(environmentToggle, environment.activeSelf);
        ResetToggle(hdrToggle, urpAsset.supportsHDR);
        ResetToggle(postProcessingToggle, Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing);
        ResetToggle(ssaoToggle, ssaoConfigurator.Intensity != 0f);

        void ResetToggle(Toggle toggle, bool on)
        {
            toggle.isOn = on;
            toggle.animator.Update(1f);
            toggle.animator.PlayInFixedTime(on ? "On" : "Off", 0, 0f);
        }

        shadowsDropdown.value = (int)GameObject.Find("Directional Light").GetComponent<Light>().shadows;
        shadowsDropdown.RefreshShownValue();
    }

    private void Update()
    {
        timer -= Time.unscaledDeltaTime;

        if (timer <= 0f)
        {
            timer += 1f / fpsRefreshRate;

            int fps = (int)(1f / Time.unscaledDeltaTime);
            fpsText.text = fps.ToString();

            if (fps > 30)
                fpsText.color = Color.green;
            else if (fps > 20)
                fpsText.color = Color.yellow;
            else
                fpsText.color = Color.red;
        }
        
        gameResolutionText.text = Screen.width + " x " + Screen.height; 
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

        // Causes gray screen? 
        // Camera.main.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = value;
    }

    public void SetSSAO(bool value)
    {
        ssaoConfigurator.Intensity = value ? ssaoIntensity : 0f;
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
