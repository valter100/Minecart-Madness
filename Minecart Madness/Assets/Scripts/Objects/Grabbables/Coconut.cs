using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Coconut : NetworkBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float stunTime;
    [SerializeField] private bool active;

    // Activate when thrown
    public void Activate()
    {
        active = true;
        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!active /*|| !IsOwner*/)
            return;

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamageServerRPC(damage);
            enemy.Stun(stunTime);
        }
    }
}
