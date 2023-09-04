using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks 
{
    public int army;

	void Start () 
	{
		if (photonView.IsMine)
		{
			PlayerManager.instance.localPlayer = this.gameObject;
			army = 1;
		}
		else
		{
			PlayerManager.instance.remotePlayer = this.gameObject;
			army = 2;
		}   
    }
	
	// Update is called once per frame
	void Update () 
	{
		if(photonView.IsMine)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
				RpcMoverseArriva();
			}
		}
		
			
	}

	[PunRPC]
	private void RpcMoverseArriva()
	{
		if (!photonView.IsMine)
		{
			transform.position = new Vector3( transform.position.x, transform.position.y + 2, transform.position.z);
		}
	}

	/**MOVER PIEZA */
	public void movePiece(int a1, int b1, int a2, int b2){
		RpcMovePiece(a1,b1,a2,b2);
	}

	// [Command]
	// private void CmdMovePiece(int a1, int b1, int a2, int b2){
	// 	RpcMovePiece(a1,b1,a2,b2);
	// }

    [PunRPC]
    private void RpcMovePiece(int a1, int b1, int a2, int b2)
    {
        if (photonView.IsMine)
            return;

        GameObject pieceToMove = tablero.instance.cuadrosTablero[a1, b1]
			.GetComponent<board>().MiPieza;

        tablero.instance.movePieceOnline(pieceToMove, a2, b2);
    }

}
