using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceRey : Piece {
    public override PieceActionInfo findAvailableCheckers(){
        base.findAvailableCheckers();
        int x = findMyX();
		int y = findMyY();

        //Torre, solo 1 casilla contigua
		newCheckerStack();
        analyseChecker(x+1, y);

        newCheckerStack();
		analyseChecker(x-1, y);

        newCheckerStack();
		analyseChecker(x, y+1);

        newCheckerStack();
		analyseChecker(x, y-1);

        //Alfil, solo 1 casilla contigua
        newCheckerStack();
        analyseChecker(x+1, y+1);

        newCheckerStack();
		analyseChecker(x-1, y-1);

        //newCheckerStack();
		//analyseChecker(x-1, y+1);

        //newCheckerStack();
		//analyseChecker(x+1, y-1);

        //Caballo
		//newCheckerStack();
        //analyseChecker(x-2, y-1);
        //newCheckerStack();
        //analyseChecker(x+1, y+2);

        //newCheckerStack();
        //analyseChecker(x+2, y+1);
        //newCheckerStack();
        //analyseChecker(x-1, y-2);
		
        return result;
    }
}
