using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChaseMacMillan.CurveDesigner;
using UnityEditor;
using UnityEngine.EventSystems;

[ExecuteAlways]
public class Rail : MonoBehaviour
{
    [SerializeField] Curve3D barsCurve;
    [SerializeField] Curve3D supportsCurve;

    [SerializeField] private Mesh barsMesh;
    [SerializeField] private Mesh supportsMesh;

    [SerializeField] private Material barsMaterial;
    [SerializeField] private Material supportsMaterial;

    public Curve3D Curve => barsCurve;

    [ExecuteInEditMode]
    private void OnEnable()
    {
        #if UNITY_EDITOR
        if (PrefabUtility.IsAnyPrefabInstanceRoot(gameObject))
        {
            PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
            Debug.Log("Rail prefab unpacked");
        }
        #endif
    }

    [ContextMenu("Update Supports And Cart")]
    private void UpdateSupportsAndCart()
    {
        if (supportsCurve)
        {
            supportsCurve.enabled = false;
            DestroyImmediate(supportsCurve.gameObject);
        }

        supportsCurve = Instantiate(barsCurve, transform).GetComponent<Curve3D>();
        supportsCurve.name = "Rail Supports";

        barsCurve.meshToTile = barsMesh;
        supportsCurve.meshToTile = supportsMesh;

        barsCurve.endTextureLayer.material = barsMaterial;
        barsCurve.edgeTextureLayer.material = barsMaterial;
        barsCurve.backTextureLayer.material = barsMaterial;
        barsCurve.mainTextureLayer.material = barsMaterial;
        barsCurve.WriteMaterialsToRenderer();

        supportsCurve.endTextureLayer.material = supportsMaterial;
        supportsCurve.edgeTextureLayer.material = supportsMaterial;
        supportsCurve.backTextureLayer.material = supportsMaterial;
        supportsCurve.mainTextureLayer.material = supportsMaterial;
        supportsCurve.WriteMaterialsToRenderer();

        #if UNITY_EDITOR
        UnityEditor.Selection.objects = new Object[] { supportsCurve.gameObject };
        #endif

        transform.Find("Cart").GetComponent<Cart>().MoveToStartPosition();
    }

}
