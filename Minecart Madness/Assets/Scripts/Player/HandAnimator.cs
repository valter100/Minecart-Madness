using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;

public class HandAnimator : NetworkBehaviour
{
    [SerializeField] private float thumbMoveTime;
    [SerializeField] private Animator animator;

    private HandController handController;
    private float thumbValue;

    private void Start()
    {
        handController = GetComponentInParent<HandController>();

        if (handController.HandMode == HandMode.Dynamic)
            animator.SetTrigger("Dynamic");
        else
            animator.SetTrigger("Closed");
    }

    private void Update()
    {
        //if (!IsOwner)
        //    return;

        if (handController.HandMode == HandMode.Dynamic)
            AnimateDynamicHand();
        else
            AnimateStaticHand();
    }

    private void AnimateDynamicHand()
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

    private void AnimateStaticHand()
    {
        if (handController.HandState == handController.OldHandState)
            return;

        switch (handController.HandState)
        {
            case HandState.Closed:   animator.SetTrigger("Closed"); break;
            case HandState.Grabbing: animator.SetTrigger("Closed"); break;
            case HandState.Hovering: animator.SetTrigger("Normal"); break;
            case HandState.Aiming:   animator.SetTrigger("Normal"); break;
            case HandState.Casting:  animator.SetTrigger("Open");   break;
        }
    }

}