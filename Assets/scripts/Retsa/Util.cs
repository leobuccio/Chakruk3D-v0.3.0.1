using System.Collections.Generic;
using System.Linq;

public static class Util{

	public static Team getOppositeTeam(Team team){
		if (team==Team.Humanos)
            return Team.Maquinas;
        else
            return Team.Humanos;
	}

	public static bool isMoveIllegal(Piece piece, Checker checker){
        bool result;
        Checker originalChecker = piece.GetComponentInParent<Checker>();
        Piece killedPiece = checker.GetComponentInChildren<Piece>();
        piece.transform.SetParent(checker.transform);
        if (killedPiece)
            killedPiece.transform.SetParent(null);
        
        if (isTeamInChakruk(piece.GetTeam())){
            result = true;
        }
        else{
            result = false;
        }

        //Volver todo a la normalidad
        piece.transform.SetParent(originalChecker.transform);
        if (killedPiece)
            killedPiece.transform.SetParent(checker.transform);

        return result;
    }

    public static bool isTeamInChakruk(Team team){
        PieceRey pieceRey = null;
        foreach (var rey in CheckBoard.instance.GetComponentsInChildren<PieceRey>())
        {
            if (rey.GetTeam()==team){
                pieceRey = rey;
                break;
            }
        }
        Checker checkerRey = pieceRey.GetComponentInParent<Checker>();
        Team oppositeTeam = Util.getOppositeTeam(team);

        if (getAllTeamPossibleMovements(oppositeTeam).Contains(checkerRey)){
            return true;
        }
        else{
            return false;
        }
    }

	public static List<Checker> getAllTeamPossibleMovements(Team team){
        List<Checker> result = new List<Checker>();
        foreach (var piece in CheckBoard.instance.GetComponentsInChildren<Piece>())
        {
            if (piece.GetTeam()!=team)
                continue;
            result.AddRange(piece.findAvailableCheckers().availableCheckers);
        }
        return result;
    }

    public static List<Checker> getBotPieces()
    {
        List<Checker> result = new List<Checker>();
        foreach (var piece in CheckBoard.instance.GetComponentsInChildren<Piece>())
        {
            if (piece.GetTeam() != Team.Humanos)
                continue;

            if(piece.findAvailableCheckers().availableCheckers.Count > 0)
            {
                result.Add(piece.GetComponentInParent<Checker>());
            }
            
        }
        return result;

    }

    public static List<Checker> getBotMovementsByPiece(Checker _selectedChecker)
    {
        List<Checker> result = new List<Checker>();
        Piece _piece = _selectedChecker.GetComponentInChildren<Piece>();
        if(_piece != null)
        {
            result.AddRange(_piece.findAvailableCheckers().availableCheckers);
        }

        return result;
    }



}
