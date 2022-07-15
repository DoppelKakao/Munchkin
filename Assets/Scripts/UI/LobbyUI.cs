using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : NetworkBehaviour
{
	[SerializeField]
	private PlayerContainer[] lobbyPlayerContainer;
	[SerializeField]
	private Button startGameButton;

	private NetworkList<LobbyPlayerState> lobbyPlayers;

	private void Awake()
	{
		lobbyPlayers = new NetworkList<LobbyPlayerState>();
	}

	public override void OnNetworkSpawn()
	{
		if (IsClient)
		{
			lobbyPlayers.OnListChanged += HandleLobbyPlayersStateChanged;
		}

		if (IsServer)
		{
			startGameButton.gameObject.SetActive(true);

			NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
			NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

			foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
			{
				HandleClientConnected(client.ClientId);
			}
		}
	}

	public override void OnDestroy()
	{
		base.OnDestroy();

		lobbyPlayers.OnListChanged -= HandleLobbyPlayersStateChanged;

		if (NetworkManager.Singleton)
		{
			NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
			NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientConnected;
		}
	}

	private bool IsEveryoneReady()
	{
		if (lobbyPlayers.Count < 2)
		{
			return false;
		}

		foreach (var player in lobbyPlayers)
		{
			if (!player.IsReady)
			{
				return false;
			}
		}

		return true;
	}

	private void HandleClientConnected(ulong clientId)
	{
		var playerData = ServerGameNetPortal.Instance.GetPlayerData(clientId);

		if (!playerData.HasValue)
		{
			return;
		}

		lobbyPlayers.Add(new LobbyPlayerState(
			clientId,
			playerData.Value.PlayerName,
			false
		));
	}

	private void HandleClientDisconnect(ulong clientId)
	{
		for (int i = 0; i < lobbyPlayers.Count; i++)
		{
			if (lobbyPlayers[i].ClientId == clientId)
			{
				lobbyPlayers.RemoveAt(i);
				break;
			}
		}
	}

	private void HandleLobbyPlayersStateChanged(NetworkListEvent<LobbyPlayerState> lobbyState)
	{
		for (int i = 0; i < lobbyPlayerContainer.Length; i++)
		{
			if (lobbyPlayers.Count > i)
			{
				lobbyPlayerContainer[i].UpdatePlayerContainer(lobbyPlayers[i]);
			}
			else
			{
				lobbyPlayerContainer[i].DisableContainer();
			}
		}

		if (IsHost)
		{
			startGameButton.interactable = IsEveryoneReady();
		}
	}
}
