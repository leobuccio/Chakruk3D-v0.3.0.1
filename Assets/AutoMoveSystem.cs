using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveSystem : MonoBehaviour
{
    private bool BotHasMoved = false;

    private List<Checker> availableCheckers = new List<Checker>();
    // Update is called once per frame
    void FixedUpdate()
    {   
        BotHasMoved = Commander.instance.BotHasMoved;
        if(MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Bot)
        {
            if(Commander.instance.CanMove && !BotHasMoved && !Commander.instance._isChakrukMate)
            {
                int delayMoveTime = Random.Range(2, 8);
                
                Invoke("SetMove", delayMoveTime);
                Commander.instance.BotHasMoved = true;
            }
        }
    }

    private void SetMove()
    {
        List<Checker> checkerList = Util.getBotPieces(); //obtiene las casillas que contenga las piezas disponibles del bot

        Debug.Log("available pieces: " + checkerList.Count);
        int _selectedCheck = Random.Range(0, checkerList.Count);
        Checker checkSelection = checkerList.ElementAt(_selectedCheck); // selecciona aleatoriamente una de las casillas disponibles
        Commander.instance.actOnChecker(checkSelection); // realiza la seleccion sobre la casilla seleccionada

        Piece _piece = checkSelection.GetComponentInChildren<Piece>();  // obtiene la pieza que esta dentro de la casilla
        List<Checker> availableOptions = Util.getBotMovementsByPiece(checkSelection); // obtiene las posibles jugadas a partir de la pieza seleccionada

        Debug.Log("available movements: " + availableOptions.Count);

        int selectedElement = Random.Range(0, availableOptions.Count); 
        Checker optionSelected = availableOptions.ElementAt(selectedElement); // elige una de las posibles jugadas

        if(Util.isMoveIllegal(_piece, optionSelected) && checkerList.Count > 1) // valido si la jugada es posible chakruk y tenga mas de un movimiento disponible.
        {
            SetMove(); // de ser asi, el metodo se repite hasta que vuelva a salir ok.
        } 
        else
        {
            Commander.instance.actOnChecker(optionSelected); // en cambio si la movida no conduce a jaque o si es jaque y no tengo movimientos disponibles. juego normal
        }

        
    }
}
