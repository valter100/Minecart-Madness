using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    [SerializeField] Slider healthbar;

    [ClientRpc]
    public void UpdateHealthBarClientRpc(float healthPercentage)
    {
        Debug.Log("Health bar percentage: " + healthPercentage);
        healthbar.value = healthPercentage;
    }
}
