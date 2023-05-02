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
    [SerializeField] NetworkPlayer player;

    private float cooldown;

    public override void OnNetworkSpawn()
    {
        player = GetComponentInParent<NetworkPlayer>();
    }

    public void SetSpell(GameObject spellPrefab)
    {
        this.spellPrefab = spellPrefab;
    }

    public void TryCastSpell()
    {
        if (cooldown == 0f && !player.Stunned())
        {
            cooldown = 1f / fireRate;
            CastSpell();
        }
    }

    private void Update()
    {
        if (cooldown != 0f)
        {
            cooldown -= Time.deltaTime;

            if (cooldown < 0f)
                cooldown = 0f;
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
