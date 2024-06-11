using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    [SerializeField] protected Team team;
    [SerializeField] public Outline outline;
    [SerializeField] protected CheckerType _checkerType;

    protected PieceActionInfo result;
	protected List<Checker> actualList;
    protected int moveCount = 0;
    

    public virtual PieceActionInfo FindAvailableCheckers(){
        result = new PieceActionInfo();
        return result;
    }

    public Team GetTeam() { return team; }
    public CheckerType GetCheckerType() { return _checkerType; }

    protected bool AnalyseChecker(int x, int y){
		CheckerAvailability cAvailability = CheckBoard.Instance.IsCheckerAvailable(x, y, team);
        bool CheckerExists = (cAvailability != CheckerAvailability.nonExistent);
        if (CheckerExists) {
			if (cAvailability == CheckerAvailability.unavailable)
                actualList = result.unavailableCheckers;

            actualList.Add(CheckBoard.Instance.Checkers[x, y]);

            if (cAvailability == CheckerAvailability.pieceKill)
                actualList = result.unavailableCheckers;
		}

        return CheckerExists;
    }

    protected void AnalyseMultipleChekers(int x,int y,Vector2Int factor)
    {
        int multiplier = 1;
        while (AnalyseChecker(
            x + factor.x * multiplier,
            y + factor.y * multiplier))
        {
            multiplier++;
        }
    }

    protected int FindMyX(){
        return GetComponentInParent<Checker>().getXPosition();
    }

    protected int FindMyY(){
        return GetComponentInParent<Checker>().getYPosition();
    }

    protected void NewCheckerStack(){
        actualList = result.availableCheckers;
    }

    public void AddMove(){
        moveCount++;
    }
}
