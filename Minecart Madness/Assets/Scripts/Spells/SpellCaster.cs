using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpellCaster : NetworkBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Transform firePoint;

    [Range(0f, 20f)]
    [SerializeField] private float fireRate;
    [SerializeField] private CrosshairController crosshairController;
    [SerializeField] private NetworkPlayer player;
    [SerializeField] private ParticleSystem particleSystem;

    private HandController handController;
    private float cooldown;

    private void Start()
    {
        handController = GetComponentInParent<HandController>();
        player = GetComponentInParent<NetworkPlayer>();
    }

    public void SetSpell(GameObject spellPrefab)
    {
        this.spellPrefab = spellPrefab;
    }

    private void Update()
    {
        if (cooldown != 0f)
        {
            cooldown -= Time.deltaTime;

            if (cooldown < 0f)
                cooldown = 0f;
        }

        if (handController && particleSystem)
        {
            if (handController.HandState == HandState.Aiming || handController.HandState == HandState.Casting)
            {
                if (particleSystem.isStopped)
                    particleSystem.Play(true);

                if (handController.HandState == HandState.Casting)
                    TryCastSpell();
            }

            else
            {
                if (particleSystem.isPlaying)
                    particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    [ContextMenu("Try Cast Spell")]
    public void TryCastSpell()
    {
        if (cooldown == 0f && !player.Stunned())
        {
            cooldown = 1f / fireRate;
            CastSpell();
        }
    }

    public void CastSpell()
    {
        GameObject go = null;

        if (crosshairController && crosshairController.CrosshairVisible)
        {
            go = Instantiate(spellPrefab, firePoint.position, Quaternion.LookRotation(crosshairController.CrosshairPosition - firePoint.position));
        }
        else
        {
            go = Instantiate(spellPrefab, firePoint.position, firePoint.rotation);
        }

        go.GetComponent<NetworkObject>().Spawn(true);
        //CastSpellClientRpc();
    }

    [ClientRpc]
    public void CastSpellClientRpc()
    {
        //if (crosshairController && crosshairController.CrosshairVisible)
        //{
        //    Instantiate(spellPrefab, firePoint.position, Quaternion.LookRotation(crosshairController.CrosshairPosition - firePoint.position));
        //}
        //else
        //{
        //    Instantiate(spellPrefab, firePoint.position, firePoint.rotation);
        //}
            
        //go.GetComponent<NetworkObject>().Spawn(true);

        Debug.Log("Spawned Spell on clients!");

        GameObject go = Instantiate(spellPrefab, firePoint.position, transform.rotation);
        Debug.Log("Casting Spell!");
    }
}
