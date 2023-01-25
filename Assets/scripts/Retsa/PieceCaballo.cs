using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCaballo : Piece {
    public override PieceActionInfo findAvailableCheckers(){
        base.findAvailableCheckers();
        int x = findMyX();
		int y = findMyY();

        //Arriba
        newCheckerStack();
        analyseChecker(x-1, y+1);
		newCheckerStack();
        analyseChecker(x-2, y-1);
        newCheckerStack();
        analyseChecker(x+1, y+2);
        newCheckerStack();
        //analyseChecker(x+2, y+4);
        //newCheckerStack();
        //analyseChecker(x-4, y-2);
        //newCheckerStack();
        //analyseChecker(x+3, y+6);
        //newCheckerStack();
        //analyseChecker(x-6, y-3);
        
   
        

        //Abajo
        newCheckerStack();
        analyseChecker(x+1, y-1);
        newCheckerStack();
        analyseChecker(x+2, y+1);
        newCheckerStack();
        analyseChecker(x-1, y-2);
        //newCheckerStack();
        //analyseChecker(x-2, y-4);
        //newCheckerStack();
        //analyseChecker(x+4, y+2);
        //newCheckerStack();
        //analyseChecker(x-3, y-6);
        //newCheckerStack();
        //analyseChecker(x+6, y+3);
        
        
		
        return result;
    }
}
