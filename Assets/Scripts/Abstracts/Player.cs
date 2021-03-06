using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
	public int ID;
	public int level;
	public int strength;
	public bool isPlaying;

	public Player(int ID, int level, int strength, bool isPlaying)
	{
		this.ID = ID;
		this.level = level;
		this.strength = strength;
		this.isPlaying = isPlaying;
	}
}
