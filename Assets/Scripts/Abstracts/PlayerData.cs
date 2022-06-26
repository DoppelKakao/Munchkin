using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
    public string PlayerName { get; private set; }
    public ulong ClientId { get; private set; }
    public int LobbyPosition { get; private set; }

    public PlayerData(string playerName, ulong clientId, int lobbyPosition)
	{
        PlayerName = playerName;
        ClientId = clientId;
        LobbyPosition = lobbyPosition;
	}
}
