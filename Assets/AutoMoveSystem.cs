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
                int delayMoveTime = Random.Range(2, 7);
                
                Invoke("SetMove", delayMoveTime);
                Commander.instance.BotHasMoved = true;
            }
        }
    }

    private void SetMove()
    {
        List<Checker> checkerList = Util.getBotPieces();

        Debug.Log("available pieces: " + checkerList.Count);
        Checker checkSelection = checkerList.ElementAt(Random.Range(0, checkerList.Count));
        Commander.instance.actOnChecker(checkSelection);
        
        List<Checker> availableOptions = Util.getBotMovementsByPiece(checkSelection);
        Debug.Log("available movements: " + availableOptions.Count);
        Checker optionSelected = availableOptions.ElementAt(Random.Range(0, availableOptions.Count));
        Commander.instance.actOnChecker(optionSelected);
        
        
    }
}
