using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;
using Unity.XR.CoreUtils;
using TMPro;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] Transform cartTransform;
    [SerializeField] private XROrigin origin;
    [SerializeField] GameObject cameraObject;
    Cart cart;
    GameObject canvasObject;

    public override void OnNetworkSpawn()
    {
        DisableClientInput();

        cart = GameObject.Find("Rail").transform.Find("Cart").GetComponent<Cart>();

        SetCartTransform();
        canvasObject = GameObject.Find("Join Sign").transform.Find("UI").gameObject;
        TMP_Text joinCodeText = canvasObject.transform.Find("JoinCodeText").GetComponent<TMP_Text>();
        joinCodeText.text = "Join Code: " + FindObjectOfType<TestRelay>().JoinCode();
    }

    public override void OnNetworkDespawn()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        cart.ResetSpawnPositionsServerRpc();

        foreach(GameObject player in players)
        {
            if (player == gameObject)
                continue;

            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();

            networkPlayer.SetCartTransform();
        }
    }



    public void DisableClientInput()
    {
        if (IsClient && !IsOwner)
        {
            var clientMoveProvider = GetComponent<NetworkMoveProvider>();
            var clientControllers = GetComponentsInChildren<ActionBasedController>();
            var clientHandAnimators = GetComponentsInChildren<HandAnimator>();
            var clientHandControllers = GetComponentsInChildren<HandController>();
            var clientTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();
            var clientAudioListener = GetComponentInChildren<AudioListener>();

            clientCamera.enabled = false;
            clientMoveProvider.EnableInputActions(false);
            clientTurnProvider.enableTurnLeftRight = false;
            clientTurnProvider.enableTurnAround = false;
            clientHead.enabled = false;
            clientAudioListener.enabled = false;

            foreach (var controller in clientControllers)
            {
                controller.enableInputActions = false;
                controller.enableInputTracking = false;
            }

            foreach(var hand in clientHandControllers)
            {
                hand.enabled = false;
            }

            foreach(var hand in clientHandAnimators)
            {
                hand.enabled = false;
            }
        }
    }
    
    public void SetCartTransform()
    {
        cartTransform = cart.GetTransform();
    }

    private void Update()
    {
        transform.position = cartTransform.position;
    }
}
