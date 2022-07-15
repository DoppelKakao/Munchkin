using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContainer : MonoBehaviour
{
	[SerializeField]
	private GameObject playerContainer;

	[SerializeField]
	private TMP_Text playerNameText;

	public void UpdatePlayerContainer(LobbyPlayerState lobbyPlayerState)
	{
		playerNameText.text = lobbyPlayerState.PlayerName.ToString();

		playerContainer.SetActive(true);
	}

	public void DisableContainer()
	{
		playerContainer.SetActive(false);
	}
}
