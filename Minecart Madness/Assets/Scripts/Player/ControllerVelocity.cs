using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVelocity : MonoBehaviour
{
    [SerializeField] private InputActionProperty velocityProperty;

    public Vector3 Velocity { get; private set; } 

    private void Update()
    {
        Velocity = velocityProperty.action.ReadValue<Vector3>();
    }
}
