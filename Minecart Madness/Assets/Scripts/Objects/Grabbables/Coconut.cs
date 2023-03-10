using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coconut : MonoBehaviour
{
    [SerializeField] private float damage;
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
        if (!active)
            return;

        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            enemy.Stun(stunTime);
        }
    }
}
