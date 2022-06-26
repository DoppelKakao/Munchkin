using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : NetworkBehaviour
{
    // Singleton
    public static GameManager _instance;
    public static GameManager Instance => _instance;

	public GameState state;
    public UnityAction<GameState> OnGameStateChange;

    private string _lobbyId;
    private string _lobbyCode;

    private string _joinCode;

    private RelayHostData _hostData;
    private RelayJoinData _joinData;

    private Lobby lobby;

    // Setup events

    // Notify state update
    public UnityAction<string> UpdateLobbyState;

    // Notify Lobby found
    public UnityAction LobbyJoined;

    // Notify Match found
    public UnityAction MatchFound;

    [SerializeField]
    private TMP_InputField joinCodeInputField;
    [SerializeField]
    private TMP_InputField joinPlayerNameInputField;
    [SerializeField]
    private TMP_InputField createPlayerNameInputField;

    private void Awake()
    {
        // Just a basic singleton
        if (_instance is null)
        {
            _instance = this;
            return;
        }

        Destroy(this);
    }

    async void Start()
    {
        UpdateGameState(GameState.Menu);

        // Initialize unity services
        await UnityServices.InitializeAsync();

        // Setup events listeners
        SetupEvents();

        // Unity Login
        await SignInAnonymouslyAsync();

        // Subscribe to NetworkManager events
        NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
    }

    #region Network events

    private void ClientConnected(ulong id)
    {
        // Player with id connected to our session

        Debug.Log("Connected player with id: " + id);

        UpdateLobbyState?.Invoke("Player found!");

        MenuController.instance.setLobbyPlayerData(lobby.Players.Count, id);
        //MatchFound?.Invoke();
    }

    #endregion

    #region UnityLogin

    void SetupEvents()
    {
        AuthenticationService.Instance.SignedIn += () => {
            // Shows how to get a playerID
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

            // Shows how to get an access token
            Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
        };

        AuthenticationService.Instance.SignInFailed += (err) => {
            Debug.LogError(err);
        };

        AuthenticationService.Instance.SignedOut += () => {
            Debug.Log($"Player { AuthenticationService.Instance.PlayerId } signed out.");
        };
    }

    async Task SignInAnonymouslyAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");
        }
        catch (Exception ex)
        {
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    #endregion

    #region Lobby

    public async void FindMatch()
    {
		if (joinCodeInputField.text == "" || joinPlayerNameInputField.text == "")
		{
            return;
		}

        string playerName = joinPlayerNameInputField.text;

        UpdateGameState(GameState.ConnectingToLobby);

        Debug.Log("Looking for a lobby...");

        UpdateLobbyState?.Invoke("Looking for a lobby...");

        try
        {
            // Looking for a lobby

            // Add options to the matchmaking (mode, rank, etc..)
            JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();

            // Safe lobbyCode
			_lobbyCode = joinCodeInputField.text;

            // Join lobby by lobby ID
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(_lobbyCode);

            UpdateGameState(GameState.InLobby);

            // Debugging
            Debug.Log("Joined lobby: " + lobby.Id);
            Debug.Log("Lobby Players: " + lobby.Players.Count);

            LobbyJoined?.Invoke();

            UpdateLobbyState?.Invoke("Waiting for Players!");

            // Trigger events (Move this to when Host presses start)
            //UpdateLobbyState?.Invoke("Match found!");
            //MatchFound?.Invoke();
        }
        catch (LobbyServiceException e)
        {
            // If we don't find any lobby, let's create a new one
            Debug.Log("Cannot find a lobby: " + e);
        }
    }

    public async void CreateMatch()
    {
		if (createPlayerNameInputField.text == "")
		{
            return;
		}

        string playerName = createPlayerNameInputField.text;

        Debug.Log("Creating a new lobby...");

        UpdateLobbyState?.Invoke("Creating a new match...");

        try
        {
            string lobbyName = "game_lobby";
            int maxPlayers = 8;
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = true;

            //// Put the JoinCode in the lobby data, visible by every member
            //options.Data = new Dictionary<string, DataObject>()
            //{
            //    {
            //        "joinCode", new DataObject(
            //            visibility: DataObject.VisibilityOptions.Member,
            //            value: _hostData.JoinCode)
            //    },
            //};
            options.Player = new Unity.Services.Lobbies.Models.Player(
                id: AuthenticationService.Instance.PlayerId,
                data: new Dictionary<string, PlayerDataObject>()
                {
					{
                        "playerData", new PlayerDataObject(
                            visibility: PlayerDataObject.VisibilityOptions.Member,
                            value: playerName)
					}
                });

            // Create the lobby
            lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxPlayers, options);

            // Save Lobby ID for later uses
            _lobbyId = lobby.Id;

            // Save Lobby Code for later uses
            _lobbyCode = lobby.LobbyCode;

            Debug.Log("Created lobby: " + lobby.LobbyCode);

            UpdateGameState(GameState.InLobby);

            // Heartbeat the lobby every 5 seconds.
            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 5)); // Duration needs to be lower then 9000 ms

            // Finally start host // TODO: Move this (lobby logic)
            //NetworkManager.Singleton.StartHost();
            //UpdateGameState(GameState.InGame);

            LobbyJoined?.Invoke();

            UpdateLobbyState?.Invoke($"Waiting for players... {_lobbyCode}");
        }
        catch (LobbyServiceException e)
        {
            Console.WriteLine(e);
            UpdateGameState(GameState.CantFindGame);
            throw;
        }
    }

    IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            Debug.Log("Lobby Heartbit");
            yield return delay;
        }
    }

    private void OnDestroy()
    {
        // We need to delete the lobby when we're not using it
        Lobbies.Instance.DeleteLobbyAsync(_lobbyId);
    }

    #endregion

    #region GameState
    public void UpdateGameState(GameState newState)
	{
        state = newState;

		switch (state)
		{
			case GameState.Menu:
				break;
			case GameState.ConnectingToLobby:
				break;
			case GameState.InLobby:
                MenuController.instance.disabelMenu();
                MenuController.instance.enableLobby();
                break;
			case GameState.ConnectingToGame:
				break;
			case GameState.StartGame:
				break;
			case GameState.InGame:
				break;
			case GameState.Combat:
				break;
			case GameState.CombatEnd:
				break;
			case GameState.RoundOver:
				break;
            case GameState.CantFindGame:
                break;
			default:
				throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
		}

        OnGameStateChange?.Invoke(newState);
	}

	public enum GameState
    {
        Menu,
        ConnectingToLobby,
        InLobby,
        ConnectingToGame,
        StartGame,
        InGame,
        Combat,
        CombatEnd,
        RoundOver,
        CantFindGame
    }
	#endregion

    async private void CreateRelay()
	{
        // External connections
        int maxConnections = 1;

        // Create RELAY object
        Allocation allocation = await Relay.Instance.CreateAllocationAsync(maxConnections);
        _hostData = new RelayHostData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            IPv4Address = allocation.RelayServer.IpV4,
        };

        // Retrieve JoinCode
        _hostData.JoinCode = await Relay.Instance.GetJoinCodeAsync(allocation.AllocationId);
        MenuController.instance.setJoinCode(_hostData.JoinCode);

        // Set Transports data
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
            _hostData.IPv4Address,
            _hostData.Port,
            _hostData.AllocationIDBytes,
            _hostData.Key,
            _hostData.ConnectionData);
    }

    async private void JoinRelay()
    {
        JoinAllocation allocation = await Relay.Instance.JoinAllocationAsync(_joinCode);

        // Create Object
        _joinData = new RelayJoinData
        {
            Key = allocation.Key,
            Port = (ushort)allocation.RelayServer.Port,
            AllocationID = allocation.AllocationId,
            AllocationIDBytes = allocation.AllocationIdBytes,
            ConnectionData = allocation.ConnectionData,
            HostConnectionData = allocation.HostConnectionData,
            IPv4Address = allocation.RelayServer.IpV4
        };

        // Set transport data
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(
            _joinData.IPv4Address,
            _joinData.Port,
            _joinData.AllocationIDBytes,
            _joinData.Key,
            _joinData.ConnectionData,
            _joinData.HostConnectionData);
    }

	#region RelayData
	/// <summary>
	/// RelayHostData represents the necessary informations
	/// for a Host to host a game on a Relay
	/// </summary>
	public struct RelayHostData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] Key;
    }

    /// <summary>
    /// RelayHostData represents the necessary informations
    /// for a Host to host a game on a Relay
    /// </summary>
    public struct RelayJoinData
    {
        public string JoinCode;
        public string IPv4Address;
        public ushort Port;
        public Guid AllocationID;
        public byte[] AllocationIDBytes;
        public byte[] ConnectionData;
        public byte[] HostConnectionData;
        public byte[] Key;
    }
	#endregion
}
