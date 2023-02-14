using ChaseMacMillan.CurveDesigner;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Cart : NetworkBehaviour
{
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] CurveFollower follower;
    [SerializeField] float cartSpeed;
    [SerializeField] NetworkVariable<int> playerAmount = new NetworkVariable<int>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public Transform GetSpawnPosition() => spawnPositions[playerAmount.Value++];

    public void ActivateCart()
    {
        follower.speed = cartSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (Transform spawnPosition in spawnPositions)
            Gizmos.DrawSphere(spawnPosition.position, 0.1f);
    }
}
