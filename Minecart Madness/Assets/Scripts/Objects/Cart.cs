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

    private void Start()
    {
        StartMoving(); //REMOVE
    }

    private void Update()
    {
        if (!started)
            return;

        if(slowed)
        {
            HandleSlow();
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
        Debug.Log("Trying to hit track!");
        Debug.DrawRay(transform.position + new Vector3(0, 5, 0), Vector3.down*100, Color.yellow);
        if (Physics.Raycast(transform.position + new Vector3(0, 5, 0), Vector3.down, out hit, Mathf.Infinity, trackLayer))
        {
            Debug.Log("Hitting track!");
            curveFollower.speed = Mathf.Clamp(curveFollower.speed += hit.normal.normalized.x * Time.deltaTime * speedShift, -maxSpeed, maxSpeed);
        }
    }

    public void SlowCartByPercentage(float slowPercentage, float duration)
    {
        GetComponent<Animator>().Play("Crash");

        slowed = true;
        speedBeforeSlow = curveFollower.speed;

        curveFollower.speed /= (1 + slowPercentage);
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

}
