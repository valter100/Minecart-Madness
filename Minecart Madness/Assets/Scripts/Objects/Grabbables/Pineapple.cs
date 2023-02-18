using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pineapple : MonoBehaviour
{
    [SerializeField] private Explosion explosionPrefab;
    [SerializeField] private bool active;

    // Activate when thrown
    public void Activate()
    {
        active = true;
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter()
    {
        if (!active)
            return;

        // Explode
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
