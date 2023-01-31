using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Script from: https://github.com/Novaborn-dev/VR-Hands-with-Unity-XR

public enum HandType
{
    Left,
    Right
}

public class HandAnimator : MonoBehaviour
{
    [SerializeField] private HandType handType;
    [SerializeField] private float thumbMoveTime;
    [SerializeField] private Animator animator;

    private InputDevice inputDevice;
    private float triggerValue;
    private float thumbValue;
    private float gripValue;

    private void Start()
    {
        inputDevice = GetInputDevice();
    }

    private void Update()
    {
        AnimateHand();
    }

    private InputDevice GetInputDevice()
    {
        InputDeviceCharacteristics controllerCharacteristic = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;

        if (handType == HandType.Left)
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Left;
        }
        else
        {
            controllerCharacteristic = controllerCharacteristic | InputDeviceCharacteristics.Right;
        }

        List<InputDevice> inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristic, inputDevices);

        return inputDevices[0];
    }

    private void AnimateHand()
    {
        inputDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue);
        inputDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue);

        inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out bool primaryTouched);
        inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out bool secondaryTouched);

        if (primaryTouched || secondaryTouched)
            thumbValue += 1f / thumbMoveTime * Time.deltaTime;
        else
            thumbValue -= 1f / thumbMoveTime * Time.deltaTime;

        thumbValue = Mathf.Clamp(thumbValue, 0f, 1f);

        animator.SetFloat("Trigger", triggerValue);
        animator.SetFloat("Grip", gripValue);
        animator.SetFloat("Thumb", thumbValue);
    }
}