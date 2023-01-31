using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{
    [SerializeField] private bool active;

    // Activate when thrown
    public void Activate()
    {
        active = true;
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!active)
            return;

        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject, 5f);
        }
    }
}
