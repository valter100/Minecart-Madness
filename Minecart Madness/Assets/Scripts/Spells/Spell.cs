using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [SerializeField] private string displayName;
    [TextArea()]
    [SerializeField] private string description;

    [SerializeField] protected GameObject[] projectilePrefabs;
    
    private void Start()
    {
        Cast();
    }

    protected abstract void Cast();

    /// <summary>
    /// Call when spell has finished casting to destroy it
    /// </summary>
    protected void Finished()
    {
        Destroy(gameObject);
    }
}
