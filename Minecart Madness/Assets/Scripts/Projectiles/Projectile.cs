using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Projectile : NetworkBehaviour
{
    [Header("Values")]
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [Range(0f, 1f)]
    [SerializeField] private float accuracy;
    [SerializeField] float lifeTime;
    float timeLived;

    [Header("Collision")]
    [SerializeField] private bool sphereCast;
    [SerializeField] private LayerMask spherecastCollisionMask;

    [Header("Sound effects")]
    [SerializeField] private AudioClip muzzleSound;
    [SerializeField] private AudioClip hitSound;
    
    [Header("References")]
    [SerializeField] private GameObject muzzleVFX;
    [SerializeField] private GameObject trailVFX;
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioSource audioSource;

    private bool collided;
    private float radius;

    public float Radius => radius;

    private void Awake()
    {
        radius = GetComponent<SphereCollider>().radius;

        if (hitVFX != null)
            hitVFX.SetActive(false);
    }

    private void Start()
    {
        // Play muzzle sound
        if (muzzleSound != null)
            audioSource.PlayOneShot(muzzleSound);

        // Skew trajectory
        if (accuracy != 1f)
        {
            // In what angle the projectile will be skewed
            float zAxisOffset = Random.Range(0f, 360f);

            // How skewed the projectile's trajectory will be
            float xAxisOffset = Random.Range(0f, 180f * Mathf.Abs(1 - accuracy));

            // Rotate transform to simulate inaccuracy (first Z then X)
            transform.Rotate(0, 0f, zAxisOffset);
            transform.Rotate(xAxisOffset, 0f, 0f);
        }

        // Detach muzzle VFX
        if (muzzleVFX != null)
        {
            muzzleVFX.transform.parent = null;
            Destroy(muzzleVFX, muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);
        }
    }

    private void Update()
    {
        Vector3 originalMove = transform.forward * speed * Time.deltaTime;
        timeLived += Time.deltaTime;

        if (sphereCast && originalMove.magnitude >= radius * 2f)
        {
            RaycastHit raycastHit;

            if (Physics.SphereCast(transform.position, radius, transform.forward, out raycastHit, originalMove.magnitude, spherecastCollisionMask))
            {
                transform.position = transform.position + transform.forward * raycastHit.distance;
            }

            else
            {
                transform.position += originalMove;
            }    
        }

        else
        {
            transform.position += originalMove;
        }

        if(timeLived >= lifeTime && IsOwner)
        {
            destroyProjectileServerRPC();
        }
    }

    [ServerRpc]
    void destroyProjectileServerRPC()
    {
        GetComponent<NetworkObject>().Despawn(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collided)
            return;

        collided = true;

        // Deal damage
        if (damage != 0)
        {
            if (collision.gameObject.tag == "Enemy" && IsOwner)
                collision.gameObject.GetComponent<Enemy>().TakeDamageServerRPC(damage);

            //else if (collision.gameObject.tag == "Cart")
                //collision.gameObject.GetComponent<Cart>().TakeDamage(damage);
        }

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;

        // Activate and detach hit VFX
        if (hitVFX != null)
        {
            hitVFX.SetActive(true);
            hitVFX.transform.parent = null;
            hitVFX.transform.position = pos;
            hitVFX.transform.rotation = rot;
            Destroy(hitVFX, hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>().main.duration);
        }

        // Instantiate explosion
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, pos, rot);
        }

        // Create temporary audio source for hit sound
        GameObject tempObject = new GameObject("Temp Audio Source");
        tempObject.AddComponent<AudioSource>().PlayOneShot(hitSound);
        Destroy(tempObject, hitSound.length);

        // Destroy projectile
        Destroy(gameObject);
    }

}
