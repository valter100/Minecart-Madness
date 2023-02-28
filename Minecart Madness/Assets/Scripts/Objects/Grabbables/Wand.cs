using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Wand : NetworkBehaviour
{
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] private Transform firePoint;

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
        GameObject go = Instantiate(spellPrefab, firePoint.position, transform.rotation);
        //go.GetComponent<NetworkObject>().Spawn(true);

        Debug.Log("Spawned Spell on clients!");
    }

}
