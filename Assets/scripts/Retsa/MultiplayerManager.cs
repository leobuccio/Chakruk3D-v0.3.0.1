using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public static MultiplayerManager Instance;
    public enum Mode {Local,Online};

    [Header("Properties")]
    [SerializeField] private Team _localTeam;
    [SerializeField] private Mode _mode;

    [Header("References")]
    [SerializeField] private PhotonView PV;

    public static Action OnConnectedToPhoton = ()=> { };
    public static Action OnRoomCreated = () => { };
    public static Action OnPlayerFound = () => { };
    public static Action OnPlayerDisconnected = () => { };
    public static Action OnDisconnectedFromServer = () => { };
    public static Action OnLoadingScene = () => { };


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void StartLocalGame()
    {
        _mode = Mode.Local;
        //SceneManager.LoadScene(1);
        StartCoroutine(LoadScene(1));
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Conectado a photon!");
        OnConnectedToPhoton();
    }

    public void JoinRandomRoom()
    {
        _mode = Mode.Online;

        PhotonNetwork.JoinRandomRoom();
    }

    void CreateRoom()
    {
        int randomRoomName = UnityEngine.Random.Range(0, 99999);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 2
        };
        PhotonNetwork.CreateRoom(randomRoomName.ToString(), roomOptions);
        OnRoomCreated();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        Debug.Log("No se encontraron partidas, creando partida propia");
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.Log("Error creando partida");
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("Entre en una habitación!");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log("Entro un jugador!");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            OnPlayerFound();
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PV.RPC(nameof(RpcStartGame), RpcTarget.All);
        }

        
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        OnPlayerDisconnected();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        OnDisconnectedFromServer();
    }

    public Mode GetMode()
    {
        return _mode;
    }

    public Team GetLocalTeam()
    {
        return _localTeam;
    }

    public void MovePieceOnline(Piece piece, Checker checker)
    {
        Checker pieceChecker = piece.GetComponentInParent<Checker>();
        Vector2Int PieceCoordinate = new Vector2Int(pieceChecker.getXPosition(),pieceChecker.getYPosition());
        Vector2Int checkerCoordinate = new Vector2Int(checker.getXPosition(), checker.getYPosition());
        PV.RPC(nameof(RpcMovePiece), RpcTarget.Others, PieceCoordinate.x, PieceCoordinate.y, checkerCoordinate.x, checkerCoordinate.y);
    }

    public void ChangeCardOnline(Piece piece)
    {
        Checker pieceChecker = piece.GetComponentInParent<Checker>();
        Vector2Int PieceCoordinate = new Vector2Int(pieceChecker.getXPosition(), pieceChecker.getYPosition());
        PV.RPC(nameof(RpcChangeCard), RpcTarget.Others, PieceCoordinate.x, PieceCoordinate.y);
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        OnLoadingScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    [PunRPC]
    private void RpcStartGame()
    {
        foreach (var p in PhotonNetwork.PlayerList)
        {
            Debug.Log("Player:" + p + " isLocal: " + p.IsLocal + " ActorNumber: " + p.ActorNumber);
            if (p.IsLocal)
                _localTeam = (Team)p.ActorNumber - 1;
        }

        //SceneManager.LoadScene(1);
        StartCoroutine(LoadScene(1));
    }

    [PunRPC]
    private void RpcMovePiece(int PieceX, int PieceY, int CheckerX, int CheckerY)
    {
        Debug.Log("RPC MOVE PIECE!");
        Piece piece = CheckBoard.Instance.Checkers[PieceX, PieceY].GetComponentInChildren<Piece>();
        Checker checker = CheckBoard.Instance.Checkers[CheckerX, CheckerY];
        Debug.Log("Piece",piece);
        Debug.Log("Checker", checker);
        Commander.Instance.MovePiece(piece, checker);
    }

    [PunRPC]
    private void RpcChangeCard(int CheckerX, int CheckerY)
    {
        Debug.Log("RPC Change Card!");
        Piece piece = CheckBoard.Instance.Checkers[CheckerX, CheckerY].GetComponentInChildren<Piece>();
        CanvasReferences.Instance.ChangeCard(piece);
    }
}
