using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	public TextMeshProUGUI playerOneLevel;
	public TextMeshProUGUI playerOneStrength;

	private void Awake()
	{
		instance = this;
	}

	public void UpdateValues(List<Player> players)
	{
		foreach (Player player in players)
		{
			ShowLevel(player.ID);
			ShowStrength(player.ID);
		}
	}

	public void ShowLevel(int ID)
	{
		playerOneLevel.text = $"Level: {PlayerManager.instance.FindPlayerByID(ID).level}";
	}

	public void ShowStrength(int ID)
	{
		playerOneStrength.text = $"Strength: {PlayerManager.instance.FindPlayerByID(ID).strength}";
	}
}
