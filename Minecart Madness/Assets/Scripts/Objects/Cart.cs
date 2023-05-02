using ChaseMacMillan.CurveDesigner;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Cart : NetworkBehaviour
{
    [SerializeField] private CurveFollower curveFollower;
    [SerializeField] private float startSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedShift;
    [SerializeField] private float startPosition;
    [SerializeField] bool started;
    bool slowed;
    float slowDuration;
    float speedBeforeSlow;

    [SerializeField] private NetworkVariable<int> playerAmount;
    [SerializeField] private NetworkVariable<int> positionValue;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] spawnPointLayouts;

    [SerializeField] private LayerMask trackLayer;

    [SerializeField] GameObject enemyObject;
    [SerializeField] float enemySpawnInterval;
    [SerializeField] float minimumSpawnRange;
    [SerializeField] float maximumSpawnRange;
    float timeSinceLastEnemySpawn;

    private void Start()
    {
        //StartMoving(); //REMOVE
    }

    private void Update()
    {
        if (!started)
            return;

        if(slowed)
        {
            HandleSlow();
        }

        timeSinceLastEnemySpawn += Time.deltaTime;
        if(timeSinceLastEnemySpawn >= enemySpawnInterval)
        {
            timeSinceLastEnemySpawn = 0;
            SpawnEnemy();
        }
        //RelegateSpeed();
    }

    public Transform GetTransform()
    {
        int numberOfSpawnpoints = spawnPointLayouts[playerAmount.Value].childCount;
        Debug.Log("There are: " + numberOfSpawnpoints + " spawnpoints in the cart");

        spawnPoints[playerAmount.Value].position = spawnPointLayouts[playerAmount.Value].GetChild(positionValue.Value).position;

        if (IsServer)
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
        curveFollower.speed = startSpeed;
        started = true;
    }

    public void RelegateSpeed()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, 5, 0), Vector3.down*100, Color.yellow);
        if (Physics.Raycast(transform.position + new Vector3(0, 5, 0), Vector3.down, out hit, Mathf.Infinity, trackLayer))
        {
            curveFollower.speed = Mathf.Clamp(curveFollower.speed += hit.normal.normalized.x * Time.deltaTime * speedShift, -maxSpeed, maxSpeed);
        }
    }

    public void SlowCartByPercentage(float slowPercentage, float duration)
    {
        if (slowed)
            return;

        GetComponent<Animator>().Play("Crash");

        slowed = true;
        speedBeforeSlow = curveFollower.speed;

        curveFollower.speed /= (1 + slowPercentage / 100);
        slowDuration = duration;
    }

    public void HandleSlow()
    {
        slowDuration -= Time.deltaTime;

        if (slowDuration <= 0)
        {
            slowDuration = 0;
            slowed = false;

            curveFollower.speed = speedBeforeSlow;
        }
    }

    public void SpawnEnemy()
    {
        Vector2 spawnAroundCart = Random.insideUnitCircle * Random.Range(minimumSpawnRange, maximumSpawnRange);

        Vector3 enemySpawnLocation = new Vector3(spawnAroundCart.x, transform.position.y + Random.Range(1, 10), spawnAroundCart.y);
        GameObject spawnedEnemy = Instantiate(enemyObject, enemySpawnLocation, Quaternion.identity);
        spawnedEnemy.GetComponent<NetworkObject>().Spawn(true);
    }
}
