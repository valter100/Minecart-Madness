using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaShot : Spell
{
    protected override void Cast()
    {
        // Instantiate projectile at spell position and rotation
        Instantiate(projectilePrefabs[0], transform.position, transform.rotation);

        // Spell finished
        Finished();
    }
}
