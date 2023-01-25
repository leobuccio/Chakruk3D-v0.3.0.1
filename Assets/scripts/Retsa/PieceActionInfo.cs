using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceActionInfo{
    public List<Checker> availableCheckers;
	public List<Checker> unavailableCheckers;

    public  PieceActionInfo(){
        availableCheckers = new List<Checker>();
        unavailableCheckers = new List<Checker>();
    }
}
