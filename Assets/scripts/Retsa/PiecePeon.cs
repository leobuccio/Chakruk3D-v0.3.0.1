using System.Collections.Generic;

public class PiecePeon : Piece 
{
    private void Awake()
    {
        _checkerType = CheckerType.PEON;
    }
    public override PieceActionInfo FindAvailableCheckers(){
        base.FindAvailableCheckers();
        int x = FindMyX();
		int y = FindMyY();

        int multiplier;
		
		if (team == Team.HUMANOS)
            multiplier = -1;
		else
            multiplier = 1;

		NewCheckerStack();
        if (AnalyseChecker(x - 1 * multiplier, y) && moveCount==0){
           // analyseChecker(x - 2 * multiplier, y);
        }

		NewCheckerStack();
		if (AnalyseChecker(x, y + 1 * multiplier) && moveCount==0){
           // analyseChecker(x, y + 2 * multiplier);
        }
		
        return result;
    }
	
}
