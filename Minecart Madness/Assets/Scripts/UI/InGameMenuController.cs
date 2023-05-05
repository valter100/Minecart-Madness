using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private XRRayInteractor leftRayInteractor;
    [SerializeField] private XRRayInteractor rightRayInteractor;
    [SerializeField] private float raycastDistance;
    [SerializeField] private Quaternion leftRayOriginRotation;
    [SerializeField] private Quaternion rightRayOriginRotation;

    private float distance;
    private float height;
    private float originalRaycastDistance;
    private Quaternion originalLeftRayOriginRotation;
    private Quaternion originalRightRayOriginRotation;

    private void Awake()
    {
        distance = canvas.transform.localPosition.z;
        height = canvas.transform.localPosition.y;
        originalRaycastDistance = leftRayInteractor.maxRaycastDistance;
        originalLeftRayOriginRotation = leftRayInteractor.transform.GetChild(0).localRotation;
        originalRightRayOriginRotation = rightRayInteractor.transform.GetChild(0).localRotation;
    }

    private void Update()
    {
        // New input system doesn't work with the Oculus Menu button :(
        if (Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            ToggleInGameMenu();
        }  
    }

    [ContextMenu("Toggle Menu")]
    public void ToggleInGameMenu()
    {
        if (canvas.activeSelf)
        {
            canvas.SetActive(false);
            leftRayInteractor.maxRaycastDistance = originalRaycastDistance;
            rightRayInteractor.maxRaycastDistance = originalRaycastDistance;
            leftRayInteractor.transform.GetChild(0).localRotation = originalLeftRayOriginRotation;
            rightRayInteractor.transform.GetChild(0).localRotation = originalRightRayOriginRotation;
        }
        else
        {
            canvas.SetActive(true);
            mainMenu.SetActive(true);
            settingsMenu.SetActive(false);

            leftRayInteractor.maxRaycastDistance = raycastDistance;
            rightRayInteractor.maxRaycastDistance = raycastDistance;
            leftRayInteractor.transform.GetChild(0).localRotation = leftRayOriginRotation;
            rightRayInteractor.transform.GetChild(0).localRotation = rightRayOriginRotation;

            canvas.transform.position =
                new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized * distance +
                new Vector3(0, height, 0);

            canvas.transform.LookAt(new Vector3(cameraTransform.position.x, canvas.transform.position.y, cameraTransform.position.z));
            canvas.transform.Rotate(new Vector3(0f, 180f, 0f));
        }
    }

    public void BackToMainMenu()
    {
        ToggleInGameMenu();

        // Lämna eller stäng ner server

        SceneTransitioner.Instance.FadeLoad("Start");
    }
}
