using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using System.Linq;

public class CanvasReferences : MonoBehaviour 
{
	public static CanvasReferences Instance;

    [Header("References")]
    public GameObject txtChakruk;
    public ChackTimer[] chackTimers;
    [SerializeField] private UDictionary<Team, Moves> _teamCards = new UDictionary<Team, Moves>();
    [SerializeField] private Button btnReset;
    [SerializeField] private Button btnSurrender;
    [SerializeField] private Button btnGoToMenu;
    [SerializeField] private GameObject panelDisconnected;

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
        _teamCards.FirstOrDefault(pair => pair.Key == piece.GetTeam()).Value.ChangeSprite(piece.GetCheckerType());
    }

    public void OnBtnResetScene()
    {
        if (MultiplayerManager.Instance.GetMode() == MultiplayerManager.Mode.Online) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void OnBtnSurrender()
    {
        Debug.Log("SURRENDER!");
        if (MultiplayerManager.Instance.GetMode() == MultiplayerManager.Mode.Online)
        {
            PhotonNetwork.Disconnect();
        }
        else 
        {
            Destroy(MultiplayerManager.Instance.gameObject);
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
    public void OnBtnGoToMenu()
    {
        Destroy(MultiplayerManager.Instance.gameObject);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    private void OnDisconnected()
    {
        panelDisconnected.SetActive(true);
    }
}
