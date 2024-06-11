using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCaballo : Piece 
{
    private void Awake()
    {
        _checkerType = CheckerType.CABALLO;
    }
    public override PieceActionInfo FindAvailableCheckers(){
        base.FindAvailableCheckers();
        int x = FindMyX();
		int y = FindMyY();

        //Arriba
        NewCheckerStack();
        AnalyseChecker(x-1, y+1);
		NewCheckerStack();
        AnalyseChecker(x-2, y-1);
        NewCheckerStack();
        AnalyseChecker(x+1, y+2);
        NewCheckerStack();
        //analyseChecker(x+2, y+4);
        //newCheckerStack();
        //analyseChecker(x-4, y-2);
        //newCheckerStack();
        //analyseChecker(x+3, y+6);
        //newCheckerStack();
        //analyseChecker(x-6, y-3);
        
   
        

        //Abajo
        NewCheckerStack();
        AnalyseChecker(x+1, y-1);
        NewCheckerStack();
        AnalyseChecker(x+2, y+1);
        NewCheckerStack();
        AnalyseChecker(x-1, y-2);
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
