using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
	public static TurnManager instance;
	public int currentPlayerTurn;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		StartGame(1);
	}

	public void StartGame(int playerID)
	{
		currentPlayerTurn = playerID;
		StartTurn();
	}

	public void StartTurn()
	{
		GameplayUIController.instance.UpdateCurrentPlayerTurn(currentPlayerTurn);
		PlayerManager.instance.AssignTurn(currentPlayerTurn);
	}

	public void EndTurn()
	{
		currentPlayerTurn = currentPlayerTurn == 1 ? 2 : 1;
		StartTurn();
	}
}
