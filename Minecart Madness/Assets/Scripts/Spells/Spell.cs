using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Spell : NetworkBehaviour
{
    [SerializeField] private string displayName;
    [TextArea()]
    [SerializeField] private string description;

    [SerializeField] protected GameObject[] projectilePrefabs;
    
    void Start()
    {
        Cast();
    }


    public abstract void Cast();

    /// <summary>
    /// Call when spell has finished casting to destroy it
    /// </summary>
    protected void Finished()
    {
        Destroy(gameObject);
    }
}
