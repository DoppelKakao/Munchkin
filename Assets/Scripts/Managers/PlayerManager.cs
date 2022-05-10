using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance;
    public List<Player> players = new List<Player>();

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		UIManager.instance.UpdateValues(players);
	}

	internal void AssignTurn(int currentPlayerTurn)
	{
		//Player player = players.Find(x => x.ID == currentPlayerTurn);
		//player.isPlaying = true;

		foreach (Player player in players)
		{
			player.isPlaying = player.ID == currentPlayerTurn;
		}
	}

	public Player FindPlayerByID(int ID)
	{
		Player player = players.Find(x => x.ID == ID);

		return player;
	}

	public void LevelDownPlayer(int ID, int amount)
	{
		Player player = FindPlayerByID(ID);
		if (player.level > 1)
		{
			if (player.level - amount < 1)
			{
				player.level = 1;
			}
			else
			{
				player.level -= amount;
			}
		}
	}

	public void LevelUpPlayer(int ID, int level)
	{
		FindPlayerByID(ID).level += level;
	}

	public void EnhancePlayerStrength(int ID, int strength)
	{
		FindPlayerByID(ID).strength += strength;
	}
}
