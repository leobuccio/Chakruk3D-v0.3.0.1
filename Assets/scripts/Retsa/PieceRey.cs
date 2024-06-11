using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceRey : Piece 
{
    private void Awake()
    {
        _checkerType = CheckerType.REY;
    }
    public override PieceActionInfo FindAvailableCheckers(){
        base.FindAvailableCheckers();
        int x = FindMyX();
		int y = FindMyY();

        //Torre, solo 1 casilla contigua
		NewCheckerStack();
        AnalyseChecker(x+1, y);

        NewCheckerStack();
		AnalyseChecker(x-1, y);

        NewCheckerStack();
		AnalyseChecker(x, y+1);

        NewCheckerStack();
		AnalyseChecker(x, y-1);

        //Alfil, solo 1 casilla contigua
        NewCheckerStack();
        AnalyseChecker(x+1, y+1);

        NewCheckerStack();
		AnalyseChecker(x-1, y-1);

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
