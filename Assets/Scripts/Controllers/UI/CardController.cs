using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class CardController : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, IDragHandler
{
    public Card card;
    public Image cardIllustration;
	public Image dropAreaImage;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardDescription;
	private Transform originalParent;

	private void Awake()
	{
		dropAreaImage = GetComponent<Image>();
	}

    public void Initialize(Card card)
	{
        this.card = card;
        cardIllustration.sprite = card.cardIllustration;
        cardName.text = card.cardName;
        cardDescription.text = card.cardDescription;
		originalParent = transform.parent;

		//"Verstecken" von ungewollten Kartenelementen. Ist nur ein Beispiel meine Karten haben keine Leben
		//if (card.health == 0) health.text = "";
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (TurnManager.instance.currentPlayerTurn == card.ownerID)
		{
			transform.SetParent(transform.root);
			dropAreaImage.raycastTarget = false;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		Debug.Log(eventData.pointerEnter);
		dropAreaImage.raycastTarget = true;

		if (eventData.pointerEnter != null && eventData.pointerEnter.name == "PlayerOneTableTopPlayArea" && TurnManager.instance.currentPlayerTurn == card.ownerID)
		{
			transform.SetParent(eventData.pointerEnter.transform);
			originalParent = eventData.pointerEnter.transform;
			PlayerManager.instance.LevelUpPlayer(card.ownerID, card.level);
			UIManager.instance.UpdateValues(PlayerManager.instance.players);
		}
		else
		{
			if (TurnManager.instance.currentPlayerTurn == card.ownerID)
			{
				transform.SetParent(originalParent);
				transform.localPosition = Vector3.zero;
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (TurnManager.instance.currentPlayerTurn == card.ownerID)
		{
			transform.position = eventData.position;
		}
	}
}
