using System.Collections.Generic;

public class PiecePeon : Piece {
    

    public override PieceActionInfo findAvailableCheckers(){
        base.findAvailableCheckers();
        int x = findMyX();
		int y = findMyY();

        int multiplier;
		
		if (team == Team.Humanos)
            multiplier = -1;
		else
            multiplier = 1;

		newCheckerStack();
        if (analyseChecker(x - 1 * multiplier, y) && moveCount==0){
           // analyseChecker(x - 2 * multiplier, y);
        }

		newCheckerStack();
		if (analyseChecker(x, y + 1 * multiplier) && moveCount==0){
           // analyseChecker(x, y + 2 * multiplier);
        }
		
        return result;
    }

	
}
