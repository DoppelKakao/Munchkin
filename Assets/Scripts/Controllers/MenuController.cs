using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
	public static MenuController instance;

	[SerializeField]
	private GameObject MainMenuItems;
	[SerializeField]
	private GameObject MenuItems;
	[SerializeField]
	private GameObject CreateGameItems;
	[SerializeField]
	private GameObject JoinGameItems;
	[SerializeField]
	private GameObject OptionItems;
	[SerializeField]
	private GameObject LobbyItems;
	[SerializeField]
	private GameObject BackOption;

	[SerializeField]
	private GameObject PlayerContainer;

	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		DontDestroyOnLoad(instance);
	}

	public void onHostGameButtonPressed()
	{
        disableMenuObjects();
        enableCreateLobbyObjects();
	}

    public void onJoinGameButtonPressed()
	{
        disableMenuObjects();
        enableJoinLobbyObjects();
	}

    public void onOptionButtonPressed()
	{
		disableMenuObjects();
		enableOptionObjects();
	}

	public void onBackButtonPressed()
	{
		disableEveryOtherObjects();
		enableMenuObjects();
	}

	private void enableMenuObjects()
	{
		MainMenuItems.SetActive(true);
		BackOption.SetActive(false);
	}

	private void disableEveryOtherObjects()
	{
		CreateGameItems.SetActive(false);
		JoinGameItems.SetActive(false);
		OptionItems.SetActive(false);
	}

	private void disableMenuObjects()
	{
        MainMenuItems.SetActive(false);
	}

	private void enableCreateLobbyObjects()
	{
        CreateGameItems.SetActive(true);
		BackOption.SetActive(true);

    }

	private void enableJoinLobbyObjects()
	{
        JoinGameItems.SetActive(true);
		BackOption.SetActive(true);
	}

	private void enableOptionObjects()
	{
		OptionItems.SetActive(true);
		BackOption.SetActive(true);
	}

	public void disabelMenu()
	{
		MenuItems.SetActive(false);
	}

	public void enableLobby()
	{
		LobbyItems.SetActive(true);
	}

	public void setLobbyPlayerData(int playerNumber, string id)
	{
		var playerLobbyProfile = PlayerContainer.transform.GetChild(playerNumber);
		playerLobbyProfile.GetChild(1).GetComponent<TextMeshProUGUI>().text = id;
	}
}
