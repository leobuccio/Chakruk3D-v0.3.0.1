using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI txtState;
    [SerializeField] GameObject pantallaCarga;

    private void OnEnable()
    {
        MultiplayerManager.OnConnectedToPhoton += OnConnectedToPhoton;
        MultiplayerManager.OnRoomCreated += OnRoomCreated;
        MultiplayerManager.OnPlayerFound += OnPlayerFound;
        MultiplayerManager.OnLoadingScene += OnLoadingScene;
    }

    private void OnDisable()
    {
        MultiplayerManager.OnConnectedToPhoton -= OnConnectedToPhoton;
        MultiplayerManager.OnRoomCreated -= OnRoomCreated;
        MultiplayerManager.OnPlayerFound -= OnPlayerFound;
        MultiplayerManager.OnLoadingScene -= OnLoadingScene;
    }

    public void StartLocal() {
        MultiplayerManager.instance.StartLocalGame();
    }

    public void StartMultiplayer() {
        txtState.text = "Searching for players...";
        MultiplayerManager.instance.JoinRandomRoom();
    }

    void OnConnectedToPhoton()
    {
        txtState.text = "Connected To Photon!";
    }

    void OnRoomCreated()
    {
        txtState.text = "New room created! waiting for players to join";
    }

    void OnPlayerFound()
    {
        txtState.text = "Player Found!, starting game...";
    }

    void OnLoadingScene()
    {
        pantallaCarga.SetActive(true);
    }
}
