using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameplayUIController : MonoBehaviour
{
	public static GameplayUIController instance;
    public TextMeshProUGUI currentPlayerTurn;
	public Button endTurnButton;

	private void Awake()
	{
		instance = this;
		SetupButtons();
	}

	private void SetupButtons()
	{
		endTurnButton.onClick.AddListener(() => { TurnManager.instance.EndTurn(); });
	}

	public void UpdateCurrentPlayerTurn(int ID)
	{
		currentPlayerTurn.gameObject.SetActive(true);
		currentPlayerTurn.text = $"Player turn: {ID}";
	}
}
