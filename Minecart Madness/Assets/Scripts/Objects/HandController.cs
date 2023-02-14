using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum HandType
{
    Left,
    Right
}

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
        inputDevice = GetInputDevice();
    }

    private InputDevice GetInputDevice()
    {
        InputDeviceCharacteristics controllerCharacteristic = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;

        controllerCharacteristic = controllerCharacteristic | (handType == HandType.Left ? InputDeviceCharacteristics.Left : InputDeviceCharacteristics.Right);

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristic, inputDevices);

        return inputDevices[0];
    }

    private void Update()
    {
        GetInputValues();
    }

    private void GetInputValues()
    {
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue);
        inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out primaryTouched);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out secondaryTouched);
    }

}
