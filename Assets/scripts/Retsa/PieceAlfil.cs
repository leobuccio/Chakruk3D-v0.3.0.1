using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAlfil : Piece 
{
    private void Awake()
    {
        _checkerType = CheckerType.ALFIL;
    }
    public override PieceActionInfo FindAvailableCheckers(){
        base.FindAvailableCheckers();
        int x = FindMyX();
		int y = FindMyY();

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
