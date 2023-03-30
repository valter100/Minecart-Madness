using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Animator))]
public class SwitchAnimator : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private Animator animator;
    [SerializeField] private string onTrigger = "On";
    [SerializeField] private string offTrigger = "Off";

    public void TriggerAnimator()
    {
        if (toggle.isOn)
            animator.SetTrigger(onTrigger);
        else
            animator.SetTrigger(offTrigger);
    }
}
