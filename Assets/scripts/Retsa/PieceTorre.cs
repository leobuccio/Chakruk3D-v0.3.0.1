using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceTorre : Piece {
    public override PieceActionInfo findAvailableCheckers(){
        base.findAvailableCheckers();
        int x = findMyX();
		int y = findMyY();

		newCheckerStack();
        analyseMultipleChekers(x, y, new Vector2Int(1, 0));

        newCheckerStack();
		analyseMultipleChekers(x, y, new Vector2Int(-1, 0));

        newCheckerStack();
		analyseMultipleChekers(x, y, new Vector2Int(0, 1));

        newCheckerStack();
		analyseMultipleChekers(x, y, new Vector2Int(0, -1));
		
        return result;
    }
}


