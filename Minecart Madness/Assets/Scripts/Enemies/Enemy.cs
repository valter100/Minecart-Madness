using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] NetworkVariable<int>  currentHealth = new NetworkVariable<int>();
    [SerializeField] HealthBar healthBar;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamageServerRPC(5);
        }
    }

    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        currentHealth.Value = maxHealth;
    }

    [ServerRpc]
    public void TakeDamageServerRPC(int damage)
    {
        if (currentHealth.Value == maxHealth)
        {
            healthBar.gameObject.SetActive(true);
        }

        currentHealth.Value -= damage;
        healthBar.UpdateHealthBarClientRpc((float)currentHealth.Value / (float)maxHealth);
        Debug.Log("Enemy Health: " + currentHealth.Value);
        if(currentHealth.Value <= 0 && IsHost)
        {
            DieServerRPC();
        }
    }

    [ServerRpc]
    public void DieServerRPC()
    {
        if (!IsServer)
            return;

        GetComponent<NetworkObject>().Despawn();
        //Play death animation for all clients
    }

    public void Stun(float seconds)
    {

    }
}
