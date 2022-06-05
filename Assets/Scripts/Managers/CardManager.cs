using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardManager : MonoBehaviour
{
    public ICard card;

    public new TextMeshProUGUI name;
    public TextMeshProUGUI description;

    public Image background;
    public Image artwork;

    // Start is called before the first frame update
    void Start()
    {
        name.text = card.Name;
        description.text = card.Description;
        background.sprite = card.Background;
        artwork.sprite = card.Illustration;
    }
}
