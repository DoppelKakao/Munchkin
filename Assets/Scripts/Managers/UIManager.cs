using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using System.Linq;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject menuItems;
    [SerializeField]
    private GameObject playerHUDItems;
    [SerializeField]
    private TextMeshProUGUI gameStateItem;

    [SerializeField] 
    private TextMeshProUGUI LobbyStateItem;

    private void Start()
    {
        // Subscribe to events
        GameManager.Instance.MatchFound += MatchFound;
        GameManager.Instance.UpdateLobbyState += UpdateLobbyState;
        GameManager.Instance.OnGameStateChange += UpdateGameState;
    }

    private void UpdateGameState(GameManager.GameState gameState)
	{
        gameStateItem.text = gameState.ToString();
	}

    private void UpdateLobbyState(string newState)
    {
        LobbyStateItem.text = newState;
    }

    private void MatchFound()
    {
        // To attach the camera to the camera mount point of the local PlayerPrefab
        //Camera.main.GetComponent<CameraController>().AttachCameraToCameraMountPoint(NetworkManager.Singleton.LocalClient.PlayerObject.transform.GetChild(0).GetChild(0).GetChild(0).transform);
    }

    private void OnDestroy()
    {
        // Unsubscribe from events
        GameManager.Instance.MatchFound -= MatchFound;
        GameManager.Instance.UpdateLobbyState -= UpdateLobbyState;
        GameManager.Instance.OnGameStateChange -= UpdateGameState;
    }
}