using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Obstacle : NetworkBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    [SerializeField] HealthBar healthBar;
    [SerializeField] float slowPercentage;
    [SerializeField] float slowDuration;

    [SerializeField] ParticleSystem pSystem;
    [SerializeField] Transform pSystemSpawnPoint;

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        currentHealth.Value = maxHealth;
    }

    [ServerRpc]
    public void TakeDamageServerRPC(int damage)
    {
        if(currentHealth.Value == maxHealth)
        {
            healthBar.gameObject.SetActive(true);
        }

        currentHealth.Value -= damage;
        healthBar.UpdateHealthBar((float)currentHealth.Value / (float)maxHealth);

        if (currentHealth.Value <= 0 && IsHost)
        {
            DieServerRPC();
        }
    }

    [ServerRpc]
    public void DieServerRPC()
    {
        if (!IsServer)
            return;

        GameObject spawnedEffect = Instantiate(pSystem.gameObject, pSystemSpawnPoint);
        spawnedEffect.transform.parent = null;
        GetComponent<NetworkObject>().Despawn();
        //Play death animation for all clients
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Cart")
        {
            other.GetComponent<Cart>().SlowCartByPercentage(slowPercentage, slowDuration);
            TakeDamageServerRPC(currentHealth.Value);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Cart")
    //    {
    //        collision.gameObject.GetComponent<Cart>().SlowCartByPercentage(slowPercentage, slowDuration);
    //        TakeDamageServerRPC(currentHealth.Value);
    //    }
    //}
}
