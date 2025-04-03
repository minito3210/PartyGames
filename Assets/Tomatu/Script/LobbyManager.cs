/*
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobby;
using Unity.Services.Lobby.Models;
using System.Collections;
using System.Threading.Tasks;

public class LobbyManager : MonoBehaviour
{
    private Lobby currentLobby;
    private string playerId;
    private const string LOBBY_NAME = "GameLobby";
    
    async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticatePlayer();
    }

    private async Task AuthenticatePlayer()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log("Player Authenticated: " + playerId);
        }
    }

    public async void CreateLobby()
    {
        try
        {
            currentLobby = await LobbyService.Instance.CreateLobbyAsync(LOBBY_NAME, 2);
            Debug.Log("Lobby Created: " + currentLobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }

    public async void JoinLobby()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions { Filters = new System.Collections.Generic.List<QueryFilter> { new QueryFilter(QueryFilter.FieldOptions.Name, LOBBY_NAME, QueryFilter.OpOptions.EQ) } };
            var lobbies = await LobbyService.Instance.QueryLobbiesAsync(options);
            
            if (lobbies.Results.Count > 0)
            {
                currentLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbies.Results[0].Id);
                Debug.Log("Joined Lobby: " + currentLobby.Id);
            }
            else
            {
                CreateLobby();
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError(e.Message);
        }
    }
}
*/