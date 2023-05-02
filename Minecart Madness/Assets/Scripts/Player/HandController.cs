using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public enum HandType { Left, Right }
public enum HandState { Closed, Grabbing, Hovering, Aiming, Casting }
public enum HandMode { Dynamic, Static }

public class HandController : NetworkBehaviour
{
    [SerializeField] private HandMode handMode;
    [SerializeField] private HandType handType;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private ControllerVelocity controllerVelocity;
    [SerializeField] private Camera playerCamera;
    //[SerializeField] private ActionBasedController actionBasedController;

    private bool hoveringInteractable;
    private HandState handState;
    private HandState oldHandState;
    private InputDevice inputDevice;

    private float triggerValue;
    private float gripValue;
    private bool primaryTouched;
    private bool secondaryTouched;
    private bool primaryPressed;
    private bool secondaryPressed;

    public bool HoveringInteractable => hoveringInteractable;
    public HandMode HandMode => handMode;
    public HandState HandState => handState;
    public HandState OldHandState => oldHandState;
    public InputDevice InputDevice => inputDevice;
    public float TriggerValue => triggerValue;
    public float GripValue => gripValue;
    public bool PrimaryTouched => primaryTouched;
    public bool SecondaryTouched => secondaryTouched;
    public bool PrimaryPressed => primaryPressed;
    public bool SecondaryPressed => secondaryPressed;
    public Vector3 Velocity => controllerVelocity.Velocity;
    public Camera PlayerCamera => playerCamera;

    private void Start()
    {
        InputDeviceCharacteristics controllerCharacteristic =
            InputDeviceCharacteristics.HeldInHand |
            InputDeviceCharacteristics.Controller |
            (handType == HandType.Left ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right);

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristic, inputDevices);

        try
        {
            inputDevice = inputDevices[0];
        }
        catch
        {
            Debug.LogWarning(handType + " controller not found.");
        }
    }

    private void Update()
    {
        GetInputValues();
        DetermineState();
        UpdateInteractionLayers();
    }
   
    private void GetInputValues()
    {
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue);
        inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out primaryTouched);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out secondaryTouched);
        inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out primaryPressed);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryPressed);
    }

    private void DetermineState()
    {
        oldHandState = handState;

        if (handState == HandState.Grabbing)
        {
            if (gripValue >= 0.05f)
                return;
        }

        if (hoveringInteractable)
        {
            handState = gripValue > 0.05f ? HandState.Grabbing : HandState.Hovering;
        }

        else
        {
            if (gripValue > 0.05f)
                handState = triggerValue > 0.05f ? HandState.Casting : HandState.Aiming;
            else
                handState = HandState.Closed;
        }
    }

    private void UpdateInteractionLayers()
    {
        if (handState == HandState.Aiming || handState == HandState.Casting)
        {
            // Interact with nothing
            rayInteractor.interactionLayers = 0;
        }
        else
        {
            // Interact with everything
            rayInteractor.interactionLayers = -1;
        }
    }

    public void HoverEntered()
    {
        hoveringInteractable = true;
    }

    public void HoverExited()
    {
        hoveringInteractable = false;
    }

}
