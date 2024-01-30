using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveSystem : MonoBehaviour
{
    [SerializeField] private bool BotHasMoved = false;

    private List<Checker> availableCheckers = new List<Checker>();
    // Update is called once per frame
    void FixedUpdate()
    {   
        BotHasMoved = Commander.instance.BotHasMoved;
        if(MultiplayerManager.instance.getMode() == MultiplayerManager.Mode.Bot)
        {
            if(Commander.instance.CanMove && !BotHasMoved)
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

        Checker checkSelection = checkerList.ElementAt(Random.Range(0, checkerList.Count)); // selecciona aleatoriamente una de las casillas disponibles
        Commander.instance.actOnChecker(checkSelection); // realiza la seleccion sobre la casilla seleccionada

        Piece _piece = checkSelection.GetComponentInChildren<Piece>();  // obtiene la pieza que esta dentro de la casilla
        List<Checker> availableOptions = Util.getBotMovementsByPiece(checkSelection); // obtiene las posibles jugadas a partir de la pieza seleccionada

        Debug.Log("available movements: " + availableOptions.Count);
        
        int selectedElement = Random.Range(0, availableOptions.Count); 
        Checker optionSelected = availableOptions.ElementAt(selectedElement); // elige una de las posibles jugadas

        if(Util.isMoveIllegal(_piece, optionSelected) && availableOptions.Count > 1) // valido si la jugada es posible chakruk y tenga mas de un movimiento disponible.
        {
            int newSelection = Random.Range(0, availableOptions.Count); // si se cumple la condicion. vuelve a elegir una de las posibles jugadas

            while(newSelection == selectedElement) // valido que la nueva seleccion no sea la misma que la anterior. 
            {
                newSelection = Random.Range(0, availableOptions.Count); // de ser asi, se realiza un bucle hasta que haya un valor diferente.
            }

            optionSelected = availableOptions.ElementAt(newSelection); // selecciono la jugada nueva y ejecuto.
            Commander.instance.actOnChecker(optionSelected);
        }
        else
        {
            Commander.instance.actOnChecker(optionSelected);
        }

        
    }
}
