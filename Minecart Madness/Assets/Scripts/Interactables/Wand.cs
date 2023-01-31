using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : MonoBehaviour
{
    [SerializeField] private Spell spellPrefab;
    [SerializeField] private Transform firePoint;

    public void CastSpell()
    {
        Instantiate(spellPrefab, firePoint.position, transform.rotation, transform);
    }

}
