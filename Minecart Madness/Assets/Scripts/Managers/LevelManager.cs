using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelManager : NetworkBehaviour
{
    public void StartLevel()
    {
        Cart cart = GameObject.Find("Cart").GetComponent<Cart>();
        cart.StartMoving();
    }
}
