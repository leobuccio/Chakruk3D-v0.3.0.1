using System.Collections.Generic;
using UnityEngine;

public class CheckBoard : MonoBehaviour 
{
    public static CheckBoard Instance;

    const int size = 11;
    public Checker[,] Checkers = new Checker[size,size];

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        AddCheckers();
    }

	private void AddCheckers()
    {
		foreach (var c in GetComponentsInChildren<Checker>())
		{
            Checkers[c.getXPosition(), c.getYPosition()] = c;
        }
	}

	public CheckerAvailability IsCheckerAvailable(int x, int y, Team team){
		if (x<0 || y<0)
            return CheckerAvailability.nonExistent;
		else if (x>=size || y>=size)
            return CheckerAvailability.nonExistent;
		else if (Checkers[x,y]==null)
            return CheckerAvailability.nonExistent;
		else if (!Checkers[x,y].GetComponentInChildren<Piece>())
            return CheckerAvailability.available;
        else if (Checkers[x,y].GetComponentInChildren<Piece>().GetTeam()!=team)
            return CheckerAvailability.pieceKill;
        else
            return CheckerAvailability.unavailable;
    }

	public void TurnOffAllCheckers()
    {
		foreach (var c in GetComponentsInChildren<Checker>())
		{
            c.turnOffGreenHighlight();
            c.turnOffRedHighlight();
        }
	}

    public List<Piece> GetPiecesByTeam(Team team)
    {
        List<Piece> result = new List<Piece>();

        foreach (var piece in GetComponentsInChildren<Piece>())
        {
            if (piece.GetTeam() == team)
                result.Add(piece);
        }
        return result;
    }
}
