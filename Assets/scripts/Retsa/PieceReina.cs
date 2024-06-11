using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceReina : Piece 
{
    private void Awake()
    {
        _checkerType = CheckerType.REINA;
    }
    public override PieceActionInfo FindAvailableCheckers(){
        base.FindAvailableCheckers();
        int x = FindMyX();
		int y = FindMyY();

        //Torre
		NewCheckerStack();
        AnalyseMultipleChekers(x, y, new Vector2Int(1, 0));

        NewCheckerStack();
		AnalyseMultipleChekers(x, y, new Vector2Int(-1, 0));

        NewCheckerStack();
		AnalyseMultipleChekers(x, y, new Vector2Int(0, 1));

        NewCheckerStack();
		AnalyseMultipleChekers(x, y, new Vector2Int(0, -1));

        //Alfil
        NewCheckerStack();
        AnalyseMultipleChekers(x, y, new Vector2Int(1, 1));

        NewCheckerStack();
		AnalyseMultipleChekers(x, y, new Vector2Int(-1, -1));

        NewCheckerStack();
		AnalyseMultipleChekers(x, y, new Vector2Int(-1, 1));

        NewCheckerStack();
		AnalyseMultipleChekers(x, y, new Vector2Int(1, -1));
		
        return result;
    }
}
