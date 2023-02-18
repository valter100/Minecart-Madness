using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum HandType { Left, Right }

public class HandController : MonoBehaviour
{
    [SerializeField] private HandType handType;
    [SerializeField] private HandAnimator handAnimator;

    private InputDevice inputDevice;
    private float triggerValue;
    private float gripValue;
    private bool primaryTouched;
    private bool secondaryTouched;

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

    private void Update()
    {
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue);
        inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out primaryTouched);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out secondaryTouched);
    }

}
