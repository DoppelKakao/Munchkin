using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public LevelUpCard card;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(card.Name);
    }
}
