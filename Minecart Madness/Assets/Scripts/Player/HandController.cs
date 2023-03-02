using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum HandType { Left, Right }
public enum HandState { Closed, Hovering, Aiming, Casting }
public enum HandMode { Dynamic, Static }

public class HandController : MonoBehaviour
{
    [SerializeField] private HandMode handMode;
    [SerializeField] private HandType handType;

    private SpellCaster spellCaster;
    private HandAnimator handAnimator;

    private bool hoveringInteractable;
    private HandState handState;
    private HandState oldHandState;
    private InputDevice inputDevice;

    private float triggerValue;
    private float gripValue;
    private bool primaryTouched;
    private bool secondaryTouched;

    public bool HoveringInteractable => hoveringInteractable;
    public HandMode HandMode => handMode;
    public HandState HandState => handState;
    public HandState OldHandState => oldHandState;
    public InputDevice InputDevice => inputDevice;
    public float TriggerValue => triggerValue;
    public float GripValue => gripValue;
    public bool PrimaryTouched => primaryTouched;
    public bool SecondaryTouched => secondaryTouched;

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

    public void LinkHand(HandAnimator handAnimator)
    {
        this.handAnimator = handAnimator;
        spellCaster = handAnimator.GetComponent<SpellCaster>();
    }

    private void Update()
    {
        GetInputValues();
        DetermineState();

        if (handState == HandState.Casting)
            spellCaster.TryCastSpell();
    }

    private void GetInputValues()
    {
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue);
        inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out primaryTouched);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out secondaryTouched);
    }

    private void DetermineState()
    {
        oldHandState = handState;

        if (hoveringInteractable)
        {
            handState = gripValue > 0.05f ? HandState.Closed : HandState.Hovering;
        }

        else
        {
            if (gripValue > 0.05f)
                handState = triggerValue > 0.05f ? HandState.Casting : HandState.Aiming;
            else
                handState = HandState.Closed;
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
