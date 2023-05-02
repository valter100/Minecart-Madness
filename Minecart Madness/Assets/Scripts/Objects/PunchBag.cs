using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBag : MonoBehaviour, IPunchable
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Collider collider;
    [SerializeField] private AudioClip[] punchSounds;
    [SerializeField] private AudioSource audioSource;

    float timer;

    public void Punch()
    {
        if (timer == 0)
        {
            timer = 1f;
            meshRenderer.enabled = false;
            collider.enabled = false;
            
            if (punchSounds.Length > 0)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(punchSounds[Random.Range(0, punchSounds.Length)]);
            }  
        }
    }

    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timer < 0f)
            {
                timer = 0f;
                meshRenderer.enabled = true;
                collider.enabled = true;
            } 
        }
    }
}
