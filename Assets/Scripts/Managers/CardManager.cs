using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Card> cards = new List<Card>();
	public Transform playerOneHand;
	public Transform playerTwoHand;
	public CardController cardControllerPrefab;

	private void Awake()
	{
		instance = this;
	}

	public void Start()
	{
		GenerateCards();
	}

	private void GenerateCards()
	{
		foreach (Card card in cards)
		{
			CardController newCard = Instantiate(cardControllerPrefab, playerOneHand);
			newCard.transform.position = Vector3.zero;
			newCard.Initialize(card);
		}
	}
}
