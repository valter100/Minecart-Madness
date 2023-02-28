using ChaseMacMillan.CurveDesigner;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Cart : NetworkBehaviour
{
    [SerializeField] private CurveFollower curveFollower;
    [SerializeField] private float speed;
    [SerializeField] private float startPosition;

    [SerializeField] private NetworkVariable<int> playerAmount;
    [SerializeField] private NetworkVariable<int> positionValue;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] spawnPointLayouts;

    public Transform GetTransform()
    {
        int numberOfSpawnpoints = spawnPointLayouts[playerAmount.Value].childCount;
        Debug.Log("There are: " + numberOfSpawnpoints + " spawnpoints in the cart");

        spawnPoints[playerAmount.Value].position = spawnPointLayouts[playerAmount.Value].GetChild(positionValue.Value).position;
        IncreasePlayerAmountServerRpc();
        return spawnPoints[playerAmount.Value];
    }

    [ServerRpc]
    public void IncreasePlayerAmountServerRpc()
    {
        playerAmount.Value++;
    }

    [ServerRpc]
    public void ResetSpawnPositionsServerRpc()
    {
        if (!IsOwner)
            return;

        playerAmount.Value = 0;
        positionValue.Value = 0;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public void MoveToStartPosition()
    {
        curveFollower.SetDistanceAlongCurve(startPosition);
    }

    public void StartMoving()
    {
        curveFollower.speed = speed;
    }
}
