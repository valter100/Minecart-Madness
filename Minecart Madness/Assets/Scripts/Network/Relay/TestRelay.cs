using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class TestRelay : MonoBehaviour
{
    //[SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text playerText;
    [SerializeField] TouchScreenKeyboard keyboard;
    [SerializeField] string joinCode;

    public string JoinCode() => joinCode;

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Join code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();


        }
        catch (RelayServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    /// <summary>
    /// Is called by join button
    /// </summary>
    public async void JoinRelay(GameObject inputField)
    {
        string input = inputField.GetComponent<InputField>().text;

        if (string.IsNullOrEmpty(input))
            return;

        try
        {
            Debug.Log("Joining Relay with code: " + input);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(input);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();

            SceneTransitioner.Instance.FadeLoad("Level 1");
        }
        catch (RelayServiceException ex)
        {
            playerText.text = ex.Message;
            Debug.Log(ex);
        }
    }

    public void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("");
        keyboard.active = true;
    }
}
