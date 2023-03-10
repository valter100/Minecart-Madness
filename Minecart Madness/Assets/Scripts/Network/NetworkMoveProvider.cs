using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkMoveProvider : ActionBasedContinuousMoveProvider
{
    [SerializeField] bool enableInputActions;

    protected override Vector2 ReadInput()
    {
        if (!enableInputActions)
            return Vector2.zero;

        return base.ReadInput();
    }

    public void EnableInputActions(bool state)
    {
        enableInputActions = state;
    }
}
