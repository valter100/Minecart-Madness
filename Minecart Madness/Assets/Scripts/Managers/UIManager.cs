using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    [SerializeField] Button startServerButton;
    [SerializeField] Button startHostButton;
    [SerializeField] Button startClientButton;

    [SerializeField] TMP_InputField joinCodeInput;
    [SerializeField] TextMeshProUGUI playersInGameText;

    bool hasServerStarted;

    private void Awake()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        //playersInGameText.text = "Players in game: "
    }

    private void Start()
    {
        //Start server
        startServerButton?.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.StartServer())
                Debug.Log("Server started.");
            else
                Debug.Log("Unable to start server");
        });

        //Start host
        startHostButton?.onClick.AddListener(async () =>
        {
            if (RelayManager.IsRelayEnabled)
            {
                await RelayManager.SetupRelay();
            }

            if (NetworkManager.Singleton.StartHost())
                Debug.Log("Host started");
            else
                Debug.Log("Unable to start host");
        });

        //Start client
        startClientButton?.onClick.AddListener(async() =>
        {
            if (RelayManager.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
                await RelayManager.joinRelay(joinCodeInput.text);
            else
                Debug.Log("Unable to join Relay");

            if (NetworkManager.Singleton.StartClient())
                Debug.Log("Client started");
            else
                Debug.Log("Unable to start client");
        });
    }
}
