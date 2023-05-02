using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Manages access to Screen Space Ambient Occlusion settings
/// </summary>
class SSAOConfigurator
{
    private readonly object ssaoSettings;
    private readonly FieldInfo _radius;
    private readonly FieldInfo _intensity;

    public SSAOConfigurator()
    {
        static ScriptableRendererFeature findRenderFeature(Type type)
        {
            FieldInfo field = reflectField(typeof(ScriptableRenderer), "m_RendererFeatures");
            ScriptableRenderer renderer = UniversalRenderPipeline.asset.scriptableRenderer;
            var list = (List<ScriptableRendererFeature>)field.GetValue(renderer);

            foreach (ScriptableRendererFeature feature in list)
                if (feature.GetType() == type)
                    return feature;

            throw new Exception($"Could not find instance of {type.AssemblyQualifiedName} in the renderer features list");
        }

        static FieldInfo reflectField(Type type, string name) =>
            type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic) ??
            throw new Exception($"Could not reflect field [{type.AssemblyQualifiedName}].{name}");

        Type tSsaoFeature = Type.GetType("UnityEngine.Rendering.Universal.ScreenSpaceAmbientOcclusion, Unity.RenderPipelines.Universal.Runtime", true);
        FieldInfo fSettings = reflectField(tSsaoFeature, "m_Settings");
        ScriptableRendererFeature ssaoFeature = findRenderFeature(tSsaoFeature);
        ssaoSettings = fSettings.GetValue(ssaoFeature) ?? throw new Exception("ssaoFeature.m_Settings was null");

        _radius = reflectField(ssaoSettings.GetType(), "Radius");
        _intensity = reflectField(ssaoSettings.GetType(), "Intensity");
    }

    public float Radius
    {
        get => (float)_radius.GetValue(ssaoSettings);
        set => _radius.SetValue(ssaoSettings, value);
    }

    public float Intensity
    {
        get => (float)_intensity.GetValue(ssaoSettings);
        set => _intensity.SetValue(ssaoSettings, value);
    }
}