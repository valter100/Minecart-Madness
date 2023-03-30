using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void Open()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Open");
    }

    public void Close()
    {
        animator.SetTrigger("Close");
    }

    /// <summary>
    /// Called by animator
    /// </summary>
    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}
