using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject MenuItems;
    public GameObject CreateGameItems;
    public GameObject JoinGameItems;
	public GameObject OptionItems;
	public GameObject BackOption;

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
		MenuItems.SetActive(true);
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
        MenuItems.SetActive(false);
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
}
