using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour {
	public static Commander Instance;

    [Header("Properties")]
    [SerializeField]private Team currentTeam = Team.HUMANOS;
    [SerializeField] private float pieceSpeed = 1;
    [SerializeField] private float checkerArrivalDistance = 0.2f;
    [SerializeField] private Vector3 destinationOffset = Vector3.zero;
    [SerializeField] private float pieceRotationMaxSpeed = 10f;
    [SerializeField] private float pieceRotationAcceleration = 5f;

    private Piece selectedPiece;
    private List<Checker> greenCheckers = new List<Checker>();
	private List<Checker> redCheckers = new List<Checker>();
    private bool freezed = false;

    private Coroutine rotatePieceCoroutine = null;
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

    public void ActOnChecker(Checker checker)
    {
        if (freezed) return;

        if (rotatePieceCoroutine!=null) StopCoroutine(rotatePieceCoroutine);

        if (selectedPiece) this.selectedPiece.outline.enabled = false;

        CheckBoard.Instance.TurnOffAllCheckers();

        if (greenCheckers.Contains(checker))
        {
            greenCheckers = new List<Checker>();

            if (Util.isMoveIllegal(selectedPiece,checker))
            {
                ShowChakrukMessage();
            }
            else
            {
                MovePiece(selectedPiece, checker);

                if (MultiplayerManager.Instance.GetMode() == MultiplayerManager.Mode.Online)
                {
                    MultiplayerManager.Instance.MovePieceOnline(selectedPiece, checker);
                }
            }
        }
		else
        {
            Piece piece = checker.GetComponentInChildren<Piece>();

            if (!piece) return;

            if (!CanIPickThePiece(piece)) return;

            PickPiece(piece);
        }
	}

    public bool CanIPickThePiece(Piece piece)
    {
        if (piece.GetTeam() != currentTeam)
        {
            return false;
        }
        else if (MultiplayerManager.Instance.GetMode() == MultiplayerManager.Mode.Local)
        {
            return true;
        }
        else if (piece.GetTeam() == MultiplayerManager.Instance.GetLocalTeam())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
	
	public void MovePiece(Piece piece, Checker checker)
    {
        
        Vector3 localPosition = piece.transform.localPosition;
        freezed = true;
        StartCoroutine(LerpPieceToChecker(piece, checker, () => OnPieceArrived(piece,checker,localPosition)));        
    }

    void OnPieceArrived(Piece piece, Checker checker, Vector3 localPosition)
    {
        if (checker.GetComponentInChildren<Piece>()) { Destroy(checker.GetComponentInChildren<Piece>().gameObject); }

        piece.transform.parent = checker.transform;
        piece.transform.localPosition = localPosition;
        piece.AddMove();

        //Coronacion
        if (piece.GetType() == typeof(PiecePeon) && checker.getCoronacion())
        {
            GameObject queen = Instantiate(QueenPrefabs.Instance.getQueens()[(int)piece.GetTeam()], checker.transform);
            queen.transform.position = piece.transform.position;
            Destroy(piece.gameObject);
        }

        ChangeTeam();

        if (Util.IsTeamInChakruk(currentTeam))
        {
            if (IsChakrukMate())
            {
                ShowChakrukMateMessage();
            }
            else
            {
                ShowChakrukMessage();
            }            
        }            

        freezed = false;
    }

	private void PickPiece(Piece pickedPiece)
    {
        CanvasReferences.Instance.ChangeCard(pickedPiece);

        if (MultiplayerManager.Instance.GetMode() == MultiplayerManager.Mode.Online)
        {
            MultiplayerManager.Instance.ChangeCardOnline(pickedPiece); 
        }        

        this.selectedPiece = pickedPiece;
        this.selectedPiece.outline.enabled = true;

        if (rotatePieceCoroutine != null)
        {
            StopCoroutine(rotatePieceCoroutine); 
        }

        rotatePieceCoroutine = StartCoroutine(RotatePiece(pickedPiece));

        PieceActionInfo pieceActionInfo = pickedPiece.FindAvailableCheckers();
		greenCheckers = pieceActionInfo.availableCheckers;
		redCheckers = pieceActionInfo.unavailableCheckers;

		foreach (var item in greenCheckers)
        {
            item.turnOnGreenHighlight();
        }

		foreach (var item in redCheckers)
        {
            item.turnOnRedHighlight();
        }
	}

    private void ChangeTeam(){
        CanvasReferences.Instance.chackTimers[(int)currentTeam].SetIsRunning(false);

        currentTeam = Util.getOppositeTeam(currentTeam);

        CanvasReferences.Instance.chackTimers[(int)currentTeam].SetIsRunning(true);
    }

    private IEnumerator LerpPieceToChecker(Piece piece, Checker checker, System.Action onLerpEnds)
    {
        Vector3 destination = checker.transform.position + destinationOffset;

        while (Vector3.Distance(piece.transform.position,destination)>checkerArrivalDistance){
            piece.transform.position = 
                Vector3.Lerp(piece.transform.position,
                destination,
                Time.deltaTime * pieceSpeed);
            yield return null;
        }
        onLerpEnds();
    }

    private IEnumerator RotatePiece(Piece piece)
    {
        float speed = 0;

        while(true)
        {
            piece.transform.eulerAngles = new Vector3(
                piece.transform.eulerAngles.x,
                piece.transform.eulerAngles.y + speed,
                piece.transform.eulerAngles.z );

            if (speed < pieceRotationMaxSpeed)
            {
                speed += Time.deltaTime * pieceRotationAcceleration;
            }

            yield return null;
        }
    }

    private void ShowChakrukMessage()
    {
        CanvasReferences.Instance.txtChakruk.GetComponent<Animator>().SetTrigger("chakruk");
    }

    private void ShowChakrukMateMessage()
    {
        CanvasReferences.Instance.txtChakruk.GetComponent<Animator>().SetTrigger("chakrukMate");
    }

    private bool IsChakrukMate()
    {
        List<Piece> MyPieces = CheckBoard.Instance.GetPiecesByTeam(currentTeam);

        foreach (var piece in MyPieces)
        {
            List<Checker> checkers = piece.FindAvailableCheckers().availableCheckers;

            foreach (var cheker in checkers)
            {
                if (!Util.isMoveIllegal(piece, cheker))
                    return false;
            }
        }

        return true;
    }
}
