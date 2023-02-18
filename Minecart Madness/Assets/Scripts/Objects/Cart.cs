using ChaseMacMillan.CurveDesigner;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Cart : NetworkBehaviour
{
    [SerializeField] private CurveFollower curveFollower;
    [SerializeField] private Transform[] spawnPointLayouts;
    [SerializeField] private Transform[] currentSpawnPoints;
    [SerializeField] private float speed;
    [SerializeField] private float startPosition;

    [SerializeField] private NetworkVariable<List<Transform>> playerTransforms;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public void AddPlayer(NetworkPlayer networkPlayer)
    {
        playerTransforms.Value.Add(networkPlayer.transform);
        UpdateSpawnPoints();
    }

    public void RemovePlayer(NetworkPlayer networkPlayer)
    {
        playerTransforms.Value.Remove(networkPlayer.transform);
        UpdateSpawnPoints();
    }

    private void UpdateSpawnPoints()
    {
        currentSpawnPoints = new Transform[playerTransforms.Value.Count];

        int i = 0;
        foreach (Transform spawnPoint in spawnPointLayouts[currentSpawnPoints.Length - 1])
            currentSpawnPoints[i++] = spawnPoint;
    }

    public void MoveToStartPosition()
    {
        curveFollower.SetDistanceAlongCurve(startPosition);
    }

    public void StartMoving()
    {
        curveFollower.speed = speed;
    }

    private void Update()
    {
        for (int i = 0; i < playerTransforms.Value.Count; ++i)
        {
            playerTransforms.Value[i].position = currentSpawnPoints[i].position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;

        foreach (Transform spawnPointLayout in spawnPointLayouts)
            foreach (Transform spawnPosition in spawnPointLayout)
                Gizmos.DrawSphere(spawnPosition.position, 0.05f);
    }
}
