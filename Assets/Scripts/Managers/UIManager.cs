using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject playButton;
    [SerializeField] private TextMeshProUGUI stateText;

    private void Start()
    {
        // Subscribe to events
        GameManager.Instance.MatchFound += MatchFound;
        GameManager.Instance.UpdateState += UpdateState;
    }

    private void UpdateState(string newState)
    {
        stateText.text = newState;
    }

    private void MatchFound()
    {
        // Disable LobbyUI
        //playButton.SetActive(false);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        GameManager.Instance.MatchFound -= MatchFound;
        GameManager.Instance.UpdateState -= UpdateState;
    }
}