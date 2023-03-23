using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlasmaShot : Spell
{
    public override void Cast()
    {
        // Instantiate projectile at spell position and rotation
        GameObject spell = Instantiate(projectilePrefabs[0], transform.position, transform.rotation);
        spell.GetComponent<NetworkObject>().Spawn(true);

        // Spell finished
        Finished();
    }
}
