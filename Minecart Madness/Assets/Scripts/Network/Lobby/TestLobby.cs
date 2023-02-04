using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class TestLobby : MonoBehaviour
{
    Lobby hostLobby;
    Lobby joinedLobby;
    float heartbeatTimer = 15;
    [SerializeField] float heartbeatTimerMax;
    string playerName;
    [SerializeField] GameObject image;
    [SerializeField] TMP_Text lobbyText;

    async void Start()
    {
        await UnityServices.InitializeAsync();

        Debug.Log(UnityServices.State);
        SetupEvents();

        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        playerName = "Valter" + UnityEngine.Random.Range(10, 99);
        Debug.Log(playerName);
    }

    private void Update()
    {
        HandleLobbyHeartbeat();
    }

    async void HandleLobbyHeartbeat()
    {
        if (hostLobby == null)
            return;

        heartbeatTimer -= Time.deltaTime;

        if (heartbeatTimer < 0)
        {
            heartbeatTimer = heartbeatTimerMax;

            await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        }
    }

    public async void CreateLobby()
    {
        try
        {
            string lobbyName = "My Lobby";
            int maxPlayer = 4;

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions()
            {
                IsPrivate = false,
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag" , DataObject.IndexOptions.S1) }
                    //{KEY_START_GAME, new DataObject(DataObject.VisibilityOptions.Member, "0") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayer, lobbyOptions);

            hostLobby = lobby;
            joinedLobby = lobby;

            PrintPlayers(hostLobby);

            Debug.Log("Created Lobby: " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
            image.SetActive(true);
            lobbyText.text = lobby.LobbyCode;
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in lobby " + lobby.Name);
        foreach (Player player in lobby.Players)
        {
            Debug.Log(player.Id + " " + player.Data["PlayerName"].Value);
        }
    }

    public async void ListLobbies()
    {
        try
        {
            //QueryLobbiesOptions options = new QueryLobbiesOptions
            //{
            //    Count = 25,
            //    Filters = new List<QueryFilter> { 
            //        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            //},
            //Order = new List<QueryOrder>{
            //    new QueryOrder(false, QueryOrder.FieldOptions.Created)
            //}
            //};

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach (Lobby lobby in queryResponse.Results)
            {
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
        }
    }

    public async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions()
            {
                Player = GetPlayer()
            };
            joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, options);
            image.SetActive(true);

            Debug.Log("Joined lobby with code: " + lobbyCode);

            PrintPlayers(joinedLobby);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async void QuickJoinLobby()
    {
        try
        {
            await LobbyService.Instance.QuickJoinLobbyAsync();
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Player ID: " + AuthenticationService.Instance.PlayerId);

            Debug.Log("Access Token: " + AuthenticationService.Instance.AccessToken);
        };

        AuthenticationService.Instance.SignInFailed += (err) =>
        {
            Debug.Log(err);
        };

        AuthenticationService.Instance.SignedOut += () =>
        {
            Debug.Log("Player signed out");
        };

        AuthenticationService.Instance.Expired += () =>
        {
            Debug.Log("Player session could not be refreshed and expired");
        };
    }

    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
                    {
                        {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                    }
        };
    }

    async void UpdatePlayerName(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                     {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)}
                }
            });
        }
        catch(LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            image.SetActive(false);
        }
        catch(LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async void KickPlayer()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, joinedLobby.Players[1].Id);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    private async void MigrateLobbyHost()
    {
        try
        {
            hostLobby = await Lobbies.Instance.UpdateLobbyAsync(hostLobby.Id, new UpdateLobbyOptions
            {
                HostId = joinedLobby.Players[1].Id
            });
            joinedLobby = hostLobby;
        }
        catch(LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    public async void DeleteLobby()
    {
        try
        {
            await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

    //public async void StartGame()
    //{
    //    if (!IsLobbyHost())
    //        return;

    //    try
    //    {
    //        Debug.Log("Start Game");

    //        string relayCode = await TestRelay.Instance.CreateRelay();

    //        Lobby lobby = await Lobbies.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
    //        {
    //            Data = new Dictionary<string, DataObject>
    //            {
    //                Key_Start_Game, new DataObject(DataObject.VisibilityOptions.Member, relayCode)
    //            }
    //        });

    //        joinedLobby = lobby;
    //    }
    //    catch(LobbyServiceException ex)
    //    {
    //        Debug.Log(ex);
    //    }

    //}
}
