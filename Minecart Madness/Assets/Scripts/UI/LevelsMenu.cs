using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsMenu : MonoBehaviour
{
    [SerializeField] private Toggle multiplayerToggle;

    public bool IsMultiplayer()
    {
        return multiplayerToggle.isOn;
    }
}
