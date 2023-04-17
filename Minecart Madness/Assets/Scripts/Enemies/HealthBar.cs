using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    [SerializeField] Slider healthbar;
    [SerializeField] Image fillImage;
    [SerializeField] List<Color> healthbarColors = new List<Color>();

    //[ClientRpc]
    public void UpdateHealthBar(float healthPercentage)
    {
        Debug.Log("Health bar percentage: " + healthPercentage);
        healthbar.value = healthPercentage;

        if(healthPercentage > 0.677f)
        {
            fillImage.color = healthbarColors[2];
        }
        else if (healthPercentage > 0.33f)
        {
            fillImage.color = healthbarColors[1];
        }
        else
        {
            fillImage.color = healthbarColors[0];
        }
        
    }
}
