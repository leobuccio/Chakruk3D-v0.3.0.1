using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    public static MultiplayerManager instance;
    public enum Mode {Local,Online,Bot};

    [Header("Properties")]
    [SerializeField] Team localTeam;
    [SerializeField] Mode mode;

    [Header("References")]
    [SerializeField] PhotonView PV;

    public static System.Action OnConnectedToPhoton = ()=> { };
    public static System.Action OnRoomCreated = () => { };
    public static System.Action OnPlayerFound = () => { };
    public static System.Action OnPlayerDisconnected = () => { };
    public static System.Action OnDisconnectedFromServer = () => { };
    public static System.Action OnLoadingScene = () => { };


    void Awake()
    {
        if (instance && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLocalGame()
    {
        mode = Mode.Local;
        //SceneManager.LoadScene(1);
        StartCoroutine(LoadScene(1));
    }

    public void StartBotGame()
    {
        mode = Mode.Bot;
        localTeam = Team.Maquinas;
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
        mode = Mode.Online;

        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateCustomRoom()
    {
        mode = Mode.Online;

        PhotonNetwork.JoinLobby();
    }

    void CreateRoom(bool publicMatch)
    {

        int randomRoomName = Random.Range(1000, 9999);
        RoomOptions roomOptions = new RoomOptions()
        {
            IsVisible = publicMatch,
            IsOpen = true,
            MaxPlayers = 2
        };
        PhotonNetwork.CreateRoom(randomRoomName.ToString(), roomOptions);
        
        PlayerPrefs.SetString("RoomName", randomRoomName.ToString());
        OnRoomCreated();
    }

    public void JoinCustomRoom(string code)
    {
        mode = Mode.Online;
        PhotonNetwork.JoinRoom(code);
    }
    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.Log("No se encontró partida con el numero especificado");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        Debug.Log("No se encontraron partidas, creando partida propia");
        CreateRoom(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.Log("Error creando partida");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        CreateRoom(false);
        Debug.Log("Entered Lobby");
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
            PV.RPC("RpcStartGame", RpcTarget.All);
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

    public Mode getMode()
    {
        return mode;
    }

    public Team getLocalTeam()
    {
        return localTeam;
    }

    public void MovePieceOnline(Piece piece, Checker checker)
    {
        Checker pieceChecker = piece.GetComponentInParent<Checker>();
        Vector2Int PieceCoordinate = new Vector2Int(pieceChecker.getXPosition(),pieceChecker.getYPosition());
        Vector2Int checkerCoordinate = new Vector2Int(checker.getXPosition(), checker.getYPosition());
        PV.RPC("RpcMovePiece", RpcTarget.Others, PieceCoordinate.x, PieceCoordinate.y, checkerCoordinate.x, checkerCoordinate.y);
    }

    public void changeCardOnline(Piece piece)
    {
        Checker pieceChecker = piece.GetComponentInParent<Checker>();
        Vector2Int PieceCoordinate = new Vector2Int(pieceChecker.getXPosition(), pieceChecker.getYPosition());
        PV.RPC("RpcChangeCard", RpcTarget.Others, PieceCoordinate.x, PieceCoordinate.y);
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
    void RpcStartGame()
    {
        foreach (var p in PhotonNetwork.PlayerList)
        {
            Debug.Log("Player:" + p + " isLocal: " + p.IsLocal + " ActorNumber: " + p.ActorNumber);
            if (p.IsLocal)
                localTeam = (Team)p.ActorNumber - 1;
        }

        //SceneManager.LoadScene(1);
        StartCoroutine(LoadScene(1));
    }

    [PunRPC]
    void RpcMovePiece(int PieceX, int PieceY, int CheckerX, int CheckerY)
    {
        Debug.Log("RPC MOVE PIECE!");
        Piece piece = CheckBoard.instance.Checkers[PieceX, PieceY].GetComponentInChildren<Piece>();
        Checker checker = CheckBoard.instance.Checkers[CheckerX, CheckerY];
        Debug.Log("Piece",piece);
        Debug.Log("Checker", checker);
        Commander.instance.movePiece(piece, checker);
    }

    [PunRPC]
    void RpcChangeCard(int CheckerX, int CheckerY)
    {
        Debug.Log("RPC Change Card!");
        Piece piece = CheckBoard.instance.Checkers[CheckerX, CheckerY].GetComponentInChildren<Piece>();
        CanvasReferences.instance.ChangeCard(piece);
    }
}
