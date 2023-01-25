using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasReferences : MonoBehaviour {
	public static CanvasReferences instance;

    [Header("References")]
    public GameObject txtChakruk;
    public ChackTimer[] chackTimers;
    [SerializeField] moves[] TeamCards;
    [SerializeField] Button btnReset;
    [SerializeField] Button btnSurrender;
    [SerializeField] Button btnGoToMenu;
    [SerializeField] GameObject panelDisconnected;

    void Awake(){
        instance = this;
    }

    private void OnEnable()
    {
        MultiplayerManager.OnDisconnectedFromServer += OnDisconnected;
        MultiplayerManager.OnPlayerDisconnected += OnDisconnected;
    }

    private void OnDisable()
    {
        MultiplayerManager.OnDisconnectedFromServer += OnDisconnected;
        MultiplayerManager.OnPlayerDisconnected += OnDisconnected;
    }

    private void Start()
    {
        btnReset.onClick.AddListener(OnBtnResetScene);
        btnSurrender.onClick.AddListener(OnBtnSurrender);
        btnGoToMenu.onClick.AddListener(OnBtnGoToMenu);
    }

    public void ChangeCard(Piece piece)
    {
        TeamCards[(int)piece.GetTeam()].changeSprite(piece.getPieceID());
    }

    void OnBtnResetScene() {
        if (MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Online)
            return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
    }

    public void OnBtnSurrender(){
        Debug.Log("SURRENDER!");
        if (MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Online)
        {
            PhotonNetwork.Disconnect();
        }
        else {
            Destroy(MultiplayerManager.instance.gameObject);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    public void OnBtnGoToMenu()
    {
        Destroy(MultiplayerManager.instance.gameObject);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    void OnDisconnected()
    {
        panelDisconnected.SetActive(true);
    }


}
