using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceReina : Piece {
    public override PieceActionInfo findAvailableCheckers(){
        base.findAvailableCheckers();
        int x = findMyX();
		int y = findMyY();

        //Torre
		newCheckerStack();
        analyseMultipleChekers(x, y, new Vector2Int(1, 0));

        newCheckerStack();
		analyseMultipleChekers(x, y, new Vector2Int(-1, 0));

        newCheckerStack();
		analyseMultipleChekers(x, y, new Vector2Int(0, 1));

        newCheckerStack();
		analyseMultipleChekers(x, y, new Vector2Int(0, -1));

        //Alfil
        newCheckerStack();
        analyseMultipleChekers(x, y, new Vector2Int(1, 1));

        newCheckerStack();
		analyseMultipleChekers(x, y, new Vector2Int(-1, -1));

        newCheckerStack();
		analyseMultipleChekers(x, y, new Vector2Int(-1, 1));

        newCheckerStack();
		analyseMultipleChekers(x, y, new Vector2Int(1, -1));
		
        return result;
    }
}
