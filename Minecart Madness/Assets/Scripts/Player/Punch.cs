using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPunchable
{
    public abstract void Punch();
}

public class Punch : MonoBehaviour
{
    private enum State { None, Initiated, Punching }

    [SerializeField] private float velocityThreshold;
    [SerializeField] private float dotProductThreshold;
    [SerializeField] private float distanceThreshold;
    [SerializeField] private SphereCollider collider;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private AudioClip[] wooshSounds;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ParticleSystem particleSystem;

    private HandController handController;
    private Vector3 startPosition;
    private State state;

    private void Start()
    {
        handController = GetComponentInParent<HandController>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.None:

                // Check to initiate
                if (VelocityCheck())
                {
                    state = State.Initiated;
                    startPosition = transform.position + collider.center;
                }  
                break;

            case State.Initiated:

                // Check to cancel
                if (!VelocityCheck())
                {
                    state = State.None;
                }
                // Check to start punch
                else if (DistanceCheck())
                {
                    state = State.Punching;
                    OnPunchStart();
                    Punching();
                }
                break;

            case State.Punching:

                // Check to end
                if (!VelocityCheck())
                {
                    state = State.None;
                    OnPunchEnd();
                }
                // Continue punch
                else
                {
                    Punching();
                }
                break;
        }
    }

    private bool VelocityCheck()
    {
        return handController.Velocity.magnitude >= velocityThreshold
            /*&& Vector3.Dot(handController.Velocity.x.normalized, (handController.transform.position - handController.PlayerCamera.transform.position).normalized) >= dotProductThreshold*/;
    }

    private bool DistanceCheck()
    {
        return Vector3.Distance(transform.position + collider.center, startPosition) >= distanceThreshold;
    }

    private void OnPunchStart()
    {
        //meshRenderer.material.color = Color.green;
        audioSource.pitch = Random.Range(1.1f, 1.3f);
        audioSource.PlayOneShot(wooshSounds[Random.Range(0, wooshSounds.Length)]);
    }

    private void OnPunchEnd()
    {
        //meshRenderer.material.color = Color.red;
    }

    private void Punching()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + collider.center, collider.radius, layerMask);
        IPunchable punchable;
        bool hit = false;

        foreach (Collider otherCollider in colliders)
        {
            punchable = otherCollider.GetComponent<IPunchable>();

            if (punchable != null)
            {
                punchable.Punch();
                hit = true;
            }  
        }

        if (hit)
        {
            particleSystem.Play(true);
        }  
    }
}
