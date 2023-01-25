using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {
    [SerializeField] protected Team team;
    [SerializeField] public Outline outline;
    [SerializeField] int pieceID;

    protected PieceActionInfo result;
	protected List<Checker> actualList;
    protected int moveCount = 0;
    

    public virtual PieceActionInfo findAvailableCheckers(){
        result = new PieceActionInfo();
        return result;
    }

    public Team GetTeam(){
        return team;
    }

    public int getPieceID() {
        return pieceID;
    }

    protected bool analyseChecker(int x, int y){
		CheckerAvailability cAvailability = CheckBoard.instance.isCheckerAvailable(x, y, team);
        bool CheckerExists = (cAvailability != CheckerAvailability.nonExistent);
        if (CheckerExists) {
			if (cAvailability == CheckerAvailability.unavailable)
                actualList = result.unavailableCheckers;

            actualList.Add(CheckBoard.instance.Checkers[x, y]);

            if (cAvailability == CheckerAvailability.pieceKill)
                actualList = result.unavailableCheckers;
		}

        return CheckerExists;
    }

    protected void analyseMultipleChekers(int x,int y,Vector2Int factor){
        int multiplier = 1;
        while(analyseChecker(
            x +factor.x*multiplier,
            y +factor.y*multiplier)){
            multiplier++;
        }
    }

    protected int findMyX(){
        return GetComponentInParent<Checker>().getXPosition();
    }

    protected int findMyY(){
        return GetComponentInParent<Checker>().getYPosition();
    }

    protected void newCheckerStack(){
        actualList = result.availableCheckers;
    }

    public void addMove(){
        moveCount++;
    }
}
