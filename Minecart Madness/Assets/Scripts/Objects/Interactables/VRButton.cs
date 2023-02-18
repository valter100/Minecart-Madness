using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VRButton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private UnityEvent OnPressed = new UnityEvent();

    private bool pressed;

    private void OnCollisionEnter(Collision collision)
    {
        if (!pressed && collision.collider.gameObject.name != "Base")
        {
            pressed = true;
            animator.Play("Pressed");
            OnPressed.Invoke();
            Debug.Log("Pressed");
        }
    }
}
