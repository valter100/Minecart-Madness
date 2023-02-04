using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Cart : NetworkBehaviour
{
    [SerializeField] Transform[] spawnPositions;
    
    [SerializeField] NetworkVariable<int> playerAmount = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public Transform GetSpawnPosition() => spawnPositions[playerAmount.Value++];
}
