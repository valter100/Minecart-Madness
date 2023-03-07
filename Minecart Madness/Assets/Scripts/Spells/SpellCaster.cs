using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SpellCaster : MonoBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Transform firePoint;
    [Range(0f, 20f)]
    [SerializeField] private float fireRate;
    [SerializeField] private CrosshairController crosshairController;

    private float cooldown;

    public void SetSpell(GameObject spellPrefab)
    {
        this.spellPrefab = spellPrefab;
    }

    public void TryCastSpell()
    {
        if (cooldown == 0f)
        {
            cooldown = 1f / fireRate;
            CastSpellServerRpc();
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

    [ServerRpc]
    public void CastSpellServerRpc()
    {
        //GameObject go = Instantiate(spellPrefab, firePoint.position, transform.rotation);
        //go.GetComponent<NetworkObject>().Spawn(true);

        //Debug.Log("Spawned Spell on server!");
        CastSpellClientRpc();
    }

    [ClientRpc]
    public void CastSpellClientRpc()
    {
        if (crosshairController && crosshairController.CrosshairVisible)
        {
            Instantiate(spellPrefab, firePoint.position, Quaternion.LookRotation(crosshairController.CrosshairPosition - firePoint.position));
        }
        else
        {
            Instantiate(spellPrefab, firePoint.position, firePoint.rotation);
        }
            
        //go.GetComponent<NetworkObject>().Spawn(true);

        Debug.Log("Spawned Spell on clients!");
    }
}
