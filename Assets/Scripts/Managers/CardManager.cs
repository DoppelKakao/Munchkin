using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardManager : MonoBehaviour
{
    public LevelUpCard card;

    public TextMeshProUGUI name;
    public TextMeshProUGUI description;

    public Image background;
    public Image artwork;

    // Start is called before the first frame update
    void Start()
    {
        name.text = card.name;
        description.text = card.description;
        background.sprite = card.background;
        artwork.sprite = card.artwork;
    }
}
