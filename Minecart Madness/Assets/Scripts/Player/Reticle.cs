using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField] private GameObject crosshairCanvasPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private Transform cameraTransform;
    private GameObject crosshair;
    private bool hidden;
    private RaycastHit raycastHit;

    private void Start()
    {
        cameraTransform = transform.parent.parent.parent.Find("Camera");
        crosshair = Instantiate(crosshairCanvasPrefab);
        crosshair.GetComponent<Canvas>().worldCamera = cameraTransform.GetComponent<Camera>();
        Hide();
    }

    public void Hide()
    {
        hidden = true;
        crosshair.SetActive(false);
    }

    public void Show()
    {
        hidden = false;
        crosshair.SetActive(true);
    }

    private void Update()
    {
        if (Physics.Raycast(firePoint.position, firePoint.forward, out raycastHit))
        {
            if (hidden)
                Show();

            crosshair.transform.position = raycastHit.point;
            crosshair.transform.LookAt(cameraTransform.position);
        }

        else
        {
            if (!hidden)
                Hide();
        }
    }

    private void OnDestroy()
    {
        Destroy(crosshair);
    }
}
