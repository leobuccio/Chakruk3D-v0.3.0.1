using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour {
	public static Commander instance;

    [Header("Properties")]
    public Team currentTeam = Team.Humanos;
    [SerializeField] private float pieceSpeed = 1;
    [SerializeField] private float checkerArrivalDistance = 0.2f;
    [SerializeField] private Vector3 destinationOffset = Vector3.zero;
    [SerializeField] private float pieceRotationMaxSpeed = 80;
    [SerializeField] private float pieceRotationAcceleration = 1;

    private Piece selectedPiece;
    private List<Checker> greenCheckers = new List<Checker>();
	private List<Checker> redCheckers = new List<Checker>();
    private bool freezed = false;
    public bool CanMove = false;
    public bool BotHasMoved = false;

    public bool _isChakrukMate = false;
    private Coroutine rotatePieceCoroutine = null;

    void Awake(){
        instance = this;
    }

	public void actOnChecker(Checker checker){
        if (freezed)
            return;
        
        
        if (rotatePieceCoroutine!=null)
            StopCoroutine(rotatePieceCoroutine);

        if (selectedPiece)
            this.selectedPiece.outline.enabled = false;
        CheckBoard.instance.turnOffAllCheckers();

        if (greenCheckers.Contains(checker))
        {
            greenCheckers = new List<Checker>();

            if (Util.isMoveIllegal(selectedPiece,checker))
            {
                showChakrukMessage();
            }
            else
            {
                movePiece(selectedPiece, checker);

                if (MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Online)
                {
                    MultiplayerManager.instance.MovePieceOnline(selectedPiece, checker);
                }
            }
        }
		else
        {
            Piece piece = checker.GetComponentInChildren<Piece>();


            if (!piece)
				return;
            
            if(!canIPickThePiece(piece))
                return;
            
            pickPiece(piece);
        }
	}

    public bool canIPickThePiece(Piece piece)
    {
        if (MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Local)
            return true;
        else if (MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Online && piece.GetTeam() != currentTeam)
        {
            return false;
        }
        else if(MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Online && piece.GetTeam() == currentTeam)
        {
            return true;
        }
        else if (MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Bot &&
                ((currentTeam == Team.Humanos && piece.GetTeam() == Team.Humanos) || (currentTeam == Team.Maquinas && piece.GetTeam() == Team.Maquinas)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

   

	
	public void movePiece(Piece piece, Checker checker){
        
        Vector3 localPosition = piece.transform.localPosition;

        freezed = true;
        StartCoroutine(
            lerpPieceToChecker(piece, checker,
                () => onPieceArrived(piece,checker,localPosition))
        );

        
    }

    void onPieceArrived(Piece piece, Checker checker, Vector3 localPosition)
    {
        if (checker.GetComponentInChildren<Piece>())
            Destroy(checker.GetComponentInChildren<Piece>().gameObject);
        piece.transform.parent = checker.transform;
        piece.transform.localPosition = localPosition;
        piece.addMove();

        //Coronacion
        if (piece.GetType() == typeof(PiecePeon) && checker.getCoronacion()){
            GameObject queen = Instantiate(QueenPrefabs.instance.getQueens()[(int)piece.GetTeam()], checker.transform);
            queen.transform.position = piece.transform.position;
            Destroy(piece.gameObject);
        }

        changeTeam();

        if (Util.isTeamInChakruk(currentTeam))
        {
            if (isChakrukMate())
            {
                showChakrukMateMessage();
                _isChakrukMate = true;
            }
            else{
                showChakrukMessage();
            }
            
        }
            

        freezed = false;
    }

	private void pickPiece(Piece pickedPiece){
        CanvasReferences.instance.ChangeCard(pickedPiece);

        if (MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Online)
            MultiplayerManager.instance.changeCardOnline(pickedPiece);
        

        this.selectedPiece = pickedPiece;
        this.selectedPiece.outline.enabled = true;

        if (rotatePieceCoroutine!=null)
            StopCoroutine(rotatePieceCoroutine);
        rotatePieceCoroutine = StartCoroutine(rotatePiece(pickedPiece));

        PieceActionInfo pieceActionInfo = pickedPiece.findAvailableCheckers();
		greenCheckers = pieceActionInfo.availableCheckers;
		redCheckers = pieceActionInfo.unavailableCheckers;
		foreach (var item in greenCheckers)
			item.turnOnGreenHighlight();
		foreach (var item in redCheckers)
			item.turnOnRedHighlight();
	}

    private void changeTeam(){
        CanvasReferences.instance.chackTimers[(int)currentTeam].SetIsRunning(false);

        currentTeam = Util.getOppositeTeam(currentTeam);
        if(currentTeam == Team.Humanos)
        {
            CanMove = true;
            BotHasMoved = false;
        }
        else
        {
            CanMove = false;
        }
        CanvasReferences.instance.chackTimers[(int)currentTeam].SetIsRunning(true);
    }

    private IEnumerator lerpPieceToChecker(Piece piece, Checker checker, System.Action onLerpEnds){
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

    private IEnumerator rotatePiece(Piece piece){
        float speed = 0;
        while(true){
            piece.transform.eulerAngles = new Vector3(
                piece.transform.eulerAngles.x,
                piece.transform.eulerAngles.y+speed,
                piece.transform.eulerAngles.z
            );
            if (speed < pieceRotationMaxSpeed)
                speed += Time.deltaTime * pieceRotationAcceleration;
            yield return null;
        }
    }

    void showChakrukMessage()
    {
        CanvasReferences.instance.txtChakruk.GetComponent<Animator>().SetTrigger("chakruk");
    }

    void showChakrukMateMessage()
    {
        CanvasReferences.instance.txtChakruk.GetComponent<Animator>().SetTrigger("chakrukMate");
    }

    bool isChakrukMate() {
        List<Piece> MyPieces = CheckBoard.instance.getPiecesByTeam(currentTeam);

        foreach (var piece in MyPieces)
        {
            List<Checker> checkers = piece.findAvailableCheckers().availableCheckers;

            foreach (var cheker in checkers)
            {
                if (!Util.isMoveIllegal(piece, cheker))
                    return false;
            }
        }

        return true;
    }
}