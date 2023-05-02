using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] private GameObject crosshairPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool smooth;
    [SerializeField] private LayerMask raycastLayerMask;

    private HandController handController;
    private GameObject crosshair;
    private bool hidden;
    private RaycastHit raycastHit;
    private Vector3 originalCrosshairScale;
    private Vector3 targetPosition;

    public Vector3 CrosshairPosition => crosshair.transform.position;
    public bool CrosshairVisible => !hidden;

    private void Start()
    {
        handController = GetComponentInParent<HandController>();
        cameraTransform = transform.parent.parent.parent.Find("Camera");
        crosshair = Instantiate(crosshairPrefab);
        originalCrosshairScale = crosshair.transform.localScale;
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
        if (Physics.Raycast(firePoint.position, firePoint.forward, out raycastHit, 500f, raycastLayerMask))
        {
            targetPosition = raycastHit.point;

            // If hidden
            if (hidden)
            {
                // Check to show crosshair and move it to target
                if (handController.HandState == HandState.Aiming || handController.HandState == HandState.Casting)
                {
                    Show();
                    crosshair.transform.position = targetPosition;
                }

                // Cancel
                else
                {
                    return;
                }
            }

            // If visible
            else
            {
                // Check to hide crosshair and cancel
                if (handController.HandState != HandState.Aiming && handController.HandState != HandState.Casting)
                {
                    Hide();
                    return;
                }

                // Move it to target smoothly or instant
                else
                {
                    if (smooth)
                    {
                        // Calculate how much to move the crosshair towards the target position
                        Vector3 difference = targetPosition - crosshair.transform.position;
                        Vector3 move = 0.2f * difference + 0.06f * difference * difference.magnitude;

                        // Bias the move when directed towards the camera
                        //float dot = Vector3.Dot(move.normalized, (cameraTransform.position - crosshair.transform.position).normalized);
                        //move *= 0.3f * (dot * dot) + 1f;

                        // Clamp move to maximum 95% of difference
                        if (move.magnitude > difference.magnitude * 0.95f)
                            move = difference * 0.95f;

                        // Move crosshair
                        crosshair.transform.position += move;
                    }

                    else
                    {
                        crosshair.transform.position = targetPosition;
                    }
                }
            }

            // Rotate and scale crosshair
            if (!hidden)
            {
                crosshair.transform.LookAt(cameraTransform.position);
                crosshair.transform.Rotate(0f, 180f, 0f);
                crosshair.transform.localScale = originalCrosshairScale * Vector3.Distance(cameraTransform.position, crosshair.transform.position);
            }
        }

        // No raycast hit
        else
        {
            // Hide
            if (!hidden)
                Hide();
        }
    }

    private void OnDestroy()
    {
        Destroy(crosshair);
    }
}
