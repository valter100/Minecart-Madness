using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float radius;
    [SerializeField] private float force;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        // Destroy this game object when VFX and sound is finished

        Destroy(gameObject, Mathf.Max(audioSource.clip.length, transform.GetChild(0).GetComponent<ParticleSystem>().main.duration));

        // Push and damage nearby objects

        if (radius <= 0f)
            return;

        Collider[] farColliders  = Physics.OverlapSphere(transform.position, radius * 1.5f, layerMask);
        Collider[] nearColliders = Physics.OverlapSphere(transform.position, radius       , layerMask);

        foreach (Collider collider in farColliders)
        {
            // Push rigidbody
            Rigidbody rigidbody = collider.gameObject.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(force, transform.position, radius * 1.5f, 20f);
                Debug.Log("Pushing " + collider.gameObject.name);
            }
                
            // Don't check more if not within original radius
            if (nearColliders.Contains(collider) == false)
                continue;

            // Damage enemy
            if (collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }

            // Trigger pineapple crate
            else if (collider.gameObject.tag == "Pineapple Crate")
            {
                //collider.gameObject.GetComponent<PineappleCrate>().Trigger();
            }

            // Damage cart
            else if (collider.gameObject.tag == "Cart")
            {
                //collider.gameObject.GetComponent<Cart>.TakeDamage(damage);
            }
        }
    }

    [ExecuteInEditMode]
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position, radius * 1.5f);
    }

}
