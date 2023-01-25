using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {
    public int army;

	void Start () {
		if (isLocalPlayer)
			PlayerManager.instance.localPlayer = this.gameObject;
		else{
			PlayerManager.instance.remotePlayer = this.gameObject;
		}

		if (isServer && isLocalPlayer)
			army = 1;
		else
            army = 2;
    }
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer)
			return;
		
		if (Input.GetKeyDown(KeyCode.Space)){
			transform.position = new Vector3(
				transform.position.x,
				transform.position.y + 2,
				transform.position.z
			);
			moverseArrivaOnline();
		}
			
	}

	/**METODOS ONLINE */

	/**EJEMPLO**/
	public void moverseArrivaOnline(){
		CmdMoverseArriva();
	}

	[Command]
	private void CmdMoverseArriva(){
		RpcMoverseArriva();
	}

	[ClientRpc]
	private void RpcMoverseArriva(){
		if (isLocalPlayer)
			return;
			
		transform.position = new Vector3(
				transform.position.x,
				transform.position.y + 2,
				transform.position.z
			);
	}

	/**MOVER PIEZA */
	public void movePiece(int a1, int b1, int a2, int b2){
		CmdMovePiece(a1,b1,a2,b2);
	}

	[Command]
	private void CmdMovePiece(int a1, int b1, int a2, int b2){
		RpcMovePiece(a1,b1,a2,b2);
	}

    [ClientRpc]
    private void RpcMovePiece(int a1, int b1, int a2, int b2)
    {
        if (isLocalPlayer)
            return;

        GameObject pieceToMove = tablero.instance.cuadrosTablero[a1, b1]
			.GetComponent<board>().MiPieza;

        tablero.instance.movePieceOnline(pieceToMove, a2, b2);
    }

}
