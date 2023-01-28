using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;
using Unity.XR.CoreUtils;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] Transform cartTransform;

    [SerializeField] private XROrigin origin;
    [SerializeField] private XRController leftController;
    [SerializeField] private XRController rightController;

    public override void OnNetworkSpawn()
    {
        
        base.OnNetworkSpawn();
    }

    public void DisableClientInput()
    {
        if (IsClient && !IsOwner)
        {
            var clientMoveProvider = GetComponent<NetworkMoveProvider>();
            var clientControllers = GetComponentsInChildren<ActionBasedController>();
            var clientTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();

            clientCamera.enabled = false;
            clientMoveProvider.EnableInputActions(false);
            clientTurnProvider.enableTurnLeftRight = false;
            clientTurnProvider.enableTurnAround = false;
            clientHead.enabled = false;

            foreach(var controller in clientControllers)
            {
                controller.enableInputActions = false;
                controller.enableInputTracking = false;
            }
        }
    }

    private void Start()
    {
        cartTransform = GameObject.FindGameObjectWithTag("Cart").transform;

        if(IsClient && IsOwner)
        {
            transform.parent = cartTransform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    //public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    //{
    //    if(IsClient && IsOwner)
    //    {

    //    }
    //}
}
