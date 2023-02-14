using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Script from: https://github.com/Novaborn-dev/VR-Hands-with-Unity-XR

public class HandAnimator : MonoBehaviour
{
    [SerializeField] private float thumbMoveTime;
    [SerializeField] private Animator animator;
    [SerializeField] private HandController handController;

    private float thumbValue;

    private void Update()
    {
        AnimateHand();
    }

    private void AnimateHand()
    {
        if (handController.PrimaryTouched || handController.SecondaryTouched)
            thumbValue += 1f / thumbMoveTime * Time.deltaTime;
        else
            thumbValue -= 1f / thumbMoveTime * Time.deltaTime;

        thumbValue = Mathf.Clamp(thumbValue, 0f, 1f);

        animator.SetFloat("Trigger", handController.TriggerValue);
        animator.SetFloat("Grip", handController.GripValue);
        animator.SetFloat("Thumb", thumbValue);
    }
}