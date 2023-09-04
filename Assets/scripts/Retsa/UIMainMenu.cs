using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI txtState;
    [SerializeField] TMP_InputField txtServerInput;
    [SerializeField] Toggle ChkBox_privateMatch;
    [SerializeField] GameObject pantallaCarga;
    [SerializeField] GameObject panel;

    private bool privateMatch;

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

        if(ChkBox_privateMatch.isOn)
        {
            MultiplayerManager.instance.CreateRoom(false);
            Debug.Log("creando partida privada...");
        }
        else
        {
            MultiplayerManager.instance.JoinRandomRoom();
        }
        
    }

    public void ShowPanel()
    {
        panel.SetActive((panel.activeSelf) ? false : true);
    }

    public void StartCustomRoom() 
    {
        txtState.text = "joining custom Room...";
        MultiplayerManager.instance.JoinCustomRoom(txtServerInput.text);
    }

    

    void OnConnectedToPhoton()
    {
        txtState.text = "Connected To Photon!";
    }

    void OnRoomCreated()
    {
        if(ChkBox_privateMatch.isOn)
        {
            txtState.text = "New room created! share this code to join: " + PlayerPrefs.GetString("RoomName");
        }
        else
        {
            txtState.text = "New room created! waiting for players to join";
        }
        
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
