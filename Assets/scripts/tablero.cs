using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tablero : MonoBehaviour {
    public static tablero instance;

    public GameObject[,] cuadrosTablero;
    public GameObject piezaSostenida;
    public GameObject Clocks;
    private Vector3 destino;
    private GameObject piezaSostenidaHelp;
    private GameObject bottomCard, topCard;
    public int actualArmy;
    private int lastClickedX, lastClickedY;
    //public int King1X, King1Y, King2X, King2Y;
    public GameObject[] piezasArriba, piezasAbajo;
    public GameObject King1, King2;
    public GameObject textChakruk;
    private int previousX, previousY;
    private GameObject pieceHolder01,pieceHolder02;
  


    private void Awake()
    {
        instance = this;

        cuadrosTablero = new GameObject[12, 12];
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                if (GameObject.Find("Cubo-" + i + "-" + j))
                {
                    cuadrosTablero[i , j] = GameObject.Find("Cubo-" + i + "-" + j);
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
        bottomCard = GameObject.Find("bottomCard");
        topCard = GameObject.Find("topCard");
        actualArmy = 1;
	}

    // Update is called once per frame
    public void Update()
    {
    }


    public void Selection(int a, int b) {
        if (!piezaSostenida) //El jugador no tiene pieza sostenida :P
        {
            if (cuadrosTablero[a, b].GetComponent<board>().MiPieza)
            {
                takePiece(a, b);
            }
        }
        else //El jugador tiene una pieza
        {
            moveAPiece(a, b);

        }
        lastClickedX = a;
        lastClickedY = b;
        if (cuadrosTablero[a, b].GetComponent<board>().MiPieza&&cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<coronar>())
        {
            cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<coronar>().checkCoronar();
        }

    }

    public void moveAPiece(int a, int b) {
        ResetVisualHelp();
        piezaSostenida.GetComponent<Outline>().enabled = false;
        previousX = piezaSostenida.GetComponent<piece>().posX;
        previousY = piezaSostenida.GetComponent<piece>().posY;

        piezaSostenida.GetComponent<piece>().posX = a;
        piezaSostenida.GetComponent<piece>().posY = b;
        destino= cuadrosTablero[a, b].transform.position+Vector3.up/2;
        pieceHolder01 = cuadrosTablero[a, b].GetComponent<board>().MiPieza; // guardar la pieza del cuadro clickeado

        if (lastClickedX != a || lastClickedY != b) //cambiar de turno si la pieza se movió de su lugar
        {
            if (cuadrosTablero[a, b].GetComponent<board>().MiPieza) {
                cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().checkThisPiece = false; //no chequear la pieza que está en el cuadro clickeado
            }
            cuadrosTablero[a, b].GetComponent<board>().MiPieza = piezaSostenida;

            if (!CheckCheck())// si no queda en jaque se mueve
            {

                if (pieceHolder01)
                {
                    cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().checkThisPiece = true;
                    cuadrosTablero[a, b].GetComponent<board>().MiPieza = pieceHolder01;
                    
                }
                else
                {
                    cuadrosTablero[a, b].GetComponent<board>().MiPieza = null;
                }
                

                if (cuadrosTablero[a, b].GetComponent<board>().MiPieza)// si hay una pieza en el casillero que caigo
                {
                    cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().morir();
                    Destroy(cuadrosTablero[a, b].GetComponent<board>().MiPieza);

                }

                cambiarTurno();
                //MOVER ONLINE
                PlayerManager.instance.localPlayer.GetComponent<Player>()
                    .movePiece(previousX,previousY, a, b);

                Clocks.GetComponent<timer>().playerOneTurn = !Clocks.GetComponent<timer>().playerOneTurn;
                piezaSostenida.GetComponent<piece>().played = true;
            }
            else //si queda en jaque vuelve a su lugar
            {
                textChakruk.GetComponent<Animator>().SetTrigger("chakruk");
                if (pieceHolder01)
                {
                    cuadrosTablero[a, b].GetComponent<board>().MiPieza = pieceHolder01;
                }
                else { cuadrosTablero[a, b].GetComponent<board>().MiPieza = null; }
                destino = cuadrosTablero[previousX, previousY].transform.position + Vector3.up / 2;
                piezaSostenida.GetComponent<piece>().posX = previousX;
                piezaSostenida.GetComponent<piece>().posY = previousY;
                a = previousX;
                b = previousY;
            }

        }
        cuadrosTablero[a, b].GetComponent<board>().MiPieza = piezaSostenida;
        cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().destino = destino;
        cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().move = true;

        piezaSostenida = null;
    }

    public void movePieceOnline(GameObject pieceToMove,int a, int b){
        pieceToMove.GetComponent<piece>().posX = a;
        pieceToMove.GetComponent<piece>().posY = b;

        destino = cuadrosTablero[a, b].transform.position+Vector3.up/2;
        pieceHolder01 = cuadrosTablero[a, b].GetComponent<board>().MiPieza; // guardar la pieza del cuadro clickeado

        if (cuadrosTablero[a, b].GetComponent<board>().MiPieza) {
            cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().checkThisPiece = false; //no chequear la pieza que está en el cuadro clickeado
        }

        cuadrosTablero[a, b].GetComponent<board>().MiPieza = pieceToMove;

        
        if (pieceHolder01)
        {
            cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().checkThisPiece = true;
            cuadrosTablero[a, b].GetComponent<board>().MiPieza = pieceHolder01;
        }
        else
        {
            cuadrosTablero[a, b].GetComponent<board>().MiPieza = null;
        }
        

        if (cuadrosTablero[a, b].GetComponent<board>().MiPieza)// si hay una pieza en el casillero que caigo
        {
            cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().morir();
            Destroy(cuadrosTablero[a, b].GetComponent<board>().MiPieza);

        }

        cambiarTurno();

        Clocks.GetComponent<timer>().playerOneTurn = !Clocks.GetComponent<timer>().playerOneTurn;
        pieceToMove.GetComponent<piece>().played = true;

        cuadrosTablero[a, b].GetComponent<board>().MiPieza = pieceToMove;
        cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().destino = destino;
        cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().move = true;
    }

    public void takePiece(int a, int b) {
        GameObject piezaActual = cuadrosTablero[a, b].GetComponent<board>().MiPieza;
        int localPlayerArmy = PlayerManager.instance.localPlayer.GetComponent<Player>().army;
        if( piezaActual && piezaActual.GetComponent<piece>().myArmy == localPlayerArmy && piezaActual.GetComponent<piece>().myArmy == actualArmy) {
            sostenerPieza(a, b);
        }
    }

    public void sostenerPieza(int a, int b) {
        Debug.Log("Estoy sosteniendo una pieza");
        piezaSostenida = cuadrosTablero[a, b].GetComponent<board>().MiPieza;
        piezaSostenida.GetComponent<Outline>().enabled = true;
        switch (piezaSostenida.GetComponent<piece>().myType)
        {
            case 6:
                Peon(a, b);
                break;
            case 5:
                Alfil(a, b);
                break;
            case 4:
                Caballo(a, b);
                break;
            case 3:
                Torre(a, b);
                break;
            case 2:
                Rey(a, b);
                break;
            case 1:
                Reina(a, b);
                break;
            default:
                break;
        }
        cuadrosTablero[a, b].GetComponent<board>().MiPieza = null;
    }

    public void changePiece(int a, int b) {
        //release current piece
        ResetVisualHelp();
        piezaSostenida.GetComponent<Outline>().enabled = false;
        cuadrosTablero[piezaSostenida.GetComponent<piece>().posX, piezaSostenida.GetComponent<piece>().posY].GetComponent<board>().MiPieza=piezaSostenida;
        //take next piece
        takePiece(a, b);


    }

    
    public bool ChequearIsOnBoard(int a, int b)
    {
        if (a >= 0 && b >= 0 && a < 11 && b < 11)
        {
            if (cuadrosTablero[a, b])
            {
               return true;
            }
            else
                return false;

        }
        else
            return false;
    }
    public bool ChequearSameTeam(int a, int b)
    {
        if (a >= 0 && b >= 0 && a < 11 && b < 11)
        {
            if (cuadrosTablero[a, b])
            {
                if (cuadrosTablero[a, b].GetComponent<board>().MiPieza)
                {
                    if (cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().myArmy == piezaSostenida.GetComponent<piece>().myArmy)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
                //return true;
            }
            else
                return false;

        }
        else
            return false;
    }
    public bool ChequearIsEmpty(int a, int b)
    {
        if (a >= 0 && b >= 0 && a < 11 && b < 11)
        {
            if (cuadrosTablero[a, b])
            {
                if (cuadrosTablero[a, b].GetComponent<board>().MiPieza)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
                return false;
        }
        else
            return false;
    }


    public bool ChequearEnemy(int a, int b)
    {
        if ((10 * a + b) >= 0 && (10 * a + b) < 121)
        {
            if (cuadrosTablero[a, b])
            {
                if (cuadrosTablero[a, b].GetComponent<board>().MiPieza)
                {
                    if (cuadrosTablero[a, b].GetComponent<board>().MiPieza.GetComponent<piece>().myArmy != piezaSostenida.GetComponent<piece>().myArmy)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
                return false;

        }
        else
            return false;
    }


    public void PrenderAyudaVisual(int a, int b)
    {
        cuadrosTablero[a, b].GetComponent<board>().VisualHelpON();
    }
    public void PrenderAyudaVisual2(int a, int b)
    {
        cuadrosTablero[a, b].GetComponent<board>().VisualHelp2ON();
    }
    public void ResetVisualHelp()
    {
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 11; j++)
            {
                if (cuadrosTablero[i, j])
                {
                    cuadrosTablero[i, j].GetComponent<board>().VisualHelpOFF();
                }
            }
        }
    }
    private void Peon(int a, int b)
    {
        if (piezaSostenida.gameObject.transform.parent.name == "PiezasAbajo")
        {
            //Arriba Izquierda VistaISO
            if (ChequearSameTeam(a - 1, b))
            {
                PrenderAyudaVisual(a - 1, b);
            }

            //ayuda roja
            if (ChequearIsOnBoard(a - 1, b))
            {
                PrenderAyudaVisual2(a - 1, b);
            }
            //fin ayuda roja

            //Arriba Derecha Vista ISO
            if (ChequearSameTeam(a, b + 1))
            {
                PrenderAyudaVisual(a, b + 1);
            }
            //ayuda roja
            if (ChequearIsOnBoard(a, b + 1))
            {
                PrenderAyudaVisual2(a, b + 1);
            }
            //fin ayuda roja

            if (piezaSostenida.GetComponent<piece>().played == false)
            {
                if (ChequearSameTeam(a - 2, b) && !cuadrosTablero[a - 1, b].GetComponent<board>().MiPieza)
                {
                    PrenderAyudaVisual(a - 2, b);
                }

                if (ChequearSameTeam(a, b + 2) && !cuadrosTablero[a, b + 1].GetComponent<board>().MiPieza)
                {
                    PrenderAyudaVisual(a, b + 2);
                }
            }
            //ayuda roja doble
            if (piezaSostenida.GetComponent<piece>().played == false)
            {
                if (ChequearIsOnBoard(a - 2, b) )
                {
                    PrenderAyudaVisual2(a - 2, b);
                }

                if (ChequearIsOnBoard(a, b + 2))
                {
                    PrenderAyudaVisual2(a, b + 2);
                }
            }
            //fin ayuda roja doble


            bottomCard.GetComponent<moves>().changeSprite(6);
        }
        else
        {
            //Abajo Izquierda VistaISO

            if (ChequearSameTeam(a, b - 1))
            {
                PrenderAyudaVisual(a, b - 1);
            }

            //ayuda roja
            if (ChequearIsOnBoard(a, b - 1))
            {
                PrenderAyudaVisual2(a, b - 1);
            }
            //fin ayuda roja

            //Abajo Derecha VistaISO
            if (ChequearSameTeam(a + 1, b))
            {
                PrenderAyudaVisual(a + 1, b);
            }
            //ayuda roja
            if (ChequearIsOnBoard(a + 1, b))
            {
                PrenderAyudaVisual2(a + 1, b);
            }
            //fin ayuda roja
            if (piezaSostenida.GetComponent<piece>().played == false)
            {
                if (ChequearSameTeam(a + 2, b) && !cuadrosTablero[a + 1, b].GetComponent<board>().MiPieza)
                {
                    PrenderAyudaVisual(a + 2, b);
                }

                if (ChequearSameTeam(a, b - 2) && !cuadrosTablero[a, b-1].GetComponent<board>().MiPieza)
                {
                    PrenderAyudaVisual(a, b - 2);
                }
            }
            //ayuda roja doble
            if (piezaSostenida.GetComponent<piece>().played == false)
            {
                if (ChequearIsOnBoard(a + 2, b))
                {
                    PrenderAyudaVisual2(a + 2, b);
                }

                if (ChequearIsOnBoard(a, b - 2))
                {
                    PrenderAyudaVisual2(a, b - 2);
                }
            }
            //fin ayuda roja doble
            topCard.GetComponent<moves>().changeSprite(6);

        }
    }
    private void Alfil(int a, int b) //Esto es horrible pero funciona, WET a morir acá...
    {
        //Izquieda VistaISO
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a - dummy >= 0 && b - dummy >= 0)
            {
                if (ChequearSameTeam(a - dummy, b - dummy))
                {
                    PrenderAyudaVisual(a - dummy, b - dummy);
                }
                else break;
                if (ChequearEnemy(a - dummy, b - dummy))
                {
                    break;
                }
            }
        }
        //ayuda visual roja
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a - dummy >= 0 && b - dummy >= 0)
            {
                if (ChequearIsOnBoard(a - dummy, b - dummy))
                {
                    PrenderAyudaVisual2(a - dummy, b - dummy);
                }
            }
        }
        //fin ayuda visual roja

        //arriba VistaISO
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a - dummy >= 0 && b + dummy <= 10)
            {
                if (ChequearSameTeam(a - dummy, b + dummy))
                {
                    PrenderAyudaVisual(a - dummy, b + dummy);
                }
                else break;
                if (ChequearEnemy(a - dummy, b + dummy))
                {
                    break;
                }
            }
        }

        //ayuda visual roja
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a - dummy >= 0 && b + dummy <= 10)
            {
                if (ChequearIsOnBoard(a - dummy, b + dummy))
                {
                    PrenderAyudaVisual2(a - dummy, b + dummy);
                }
            }
        }

        //fin ayuda visual roja

        //Abajo VistaISO
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a + dummy <= 10 && b - dummy >= 0)
            {
                if (ChequearSameTeam(a + dummy, b - dummy))
                {
                    PrenderAyudaVisual(a + dummy, b - dummy);
                }
                else break;
                if (ChequearEnemy(a + dummy, b - dummy))
                {
                    break;
                }
            }
        }

        //ayuda visual roja
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a + dummy <= 10 && b - dummy >= 0)
            {
                if (ChequearIsOnBoard(a + dummy, b - dummy))
                {
                    PrenderAyudaVisual2(a + dummy, b - dummy);
                }
            }
        }

        //fin ayuda visual roja

        //Derecha VistaISO
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a + dummy <= 10 && b + dummy <= 10)
            {
                if (ChequearSameTeam(a + dummy, b + dummy))
                {
                    PrenderAyudaVisual(a + dummy, b + dummy);
                }
                else break;
                if (ChequearEnemy(a + dummy, b + dummy))
                {
                    break;
                }
            }
        }

        //ayuda visual roja
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a + dummy <= 10 && b + dummy <= 10)
            {
                if (ChequearIsOnBoard(a + dummy, b + dummy))
                {
                    PrenderAyudaVisual2(a + dummy, b + dummy);
                }
            }
        }

        //fin ayuda visual roja

        if (piezaSostenida.gameObject.transform.parent.name == "PiezasAbajo")
        {
            bottomCard.GetComponent<moves>().changeSprite(5);
        }
        else
        {
            topCard.GetComponent<moves>().changeSprite(5);
        }
    }

    private void Caballo(int a, int b)
    {
        //habilita a jugar en la casilla
        if (ChequearSameTeam(a - 2, b - 1))
        {
            PrenderAyudaVisual(a - 2, b - 1);
        }
        if (ChequearSameTeam(a - 1, b - 2))
        {
            PrenderAyudaVisual(a - 1, b - 2);
        }
        if (ChequearSameTeam(a - 1, b + 1))
        {
            PrenderAyudaVisual(a - 1, b + 1);
        }
        if (ChequearSameTeam(a + 1, b - 1))
        {
            PrenderAyudaVisual(a + 1, b - 1);
        }
        if (ChequearSameTeam(a + 1, b + 2))
        {
            PrenderAyudaVisual(a + 1, b + 2);
        }
        if (ChequearSameTeam(a + 2, b + 1))
        {
            PrenderAyudaVisual(a + 2, b + 1);
        }
        //prende un cuadro en lugares donde se podría jugar
        if (ChequearIsOnBoard(a - 2, b - 1))
        {
            PrenderAyudaVisual2(a - 2, b - 1);
        }
        if (ChequearIsOnBoard(a - 1, b - 2))
        {
            PrenderAyudaVisual2(a - 1, b - 2);
        }
        if (ChequearIsOnBoard(a - 1, b + 1))
        {
            PrenderAyudaVisual2(a - 1, b + 1);
        }
        if (ChequearIsOnBoard(a + 1, b - 1))
        {
            PrenderAyudaVisual2(a + 1, b - 1);
        }
        if (ChequearIsOnBoard(a + 1, b + 2))
        {
            PrenderAyudaVisual2(a + 1, b + 2);
        }
        if (ChequearIsOnBoard(a + 2, b + 1))
        {
            PrenderAyudaVisual2(a + 2, b + 1);
        }


        if (piezaSostenida.gameObject.transform.parent.name == "PiezasAbajo")
        {
            bottomCard.GetComponent<moves>().changeSprite(4);
        }
        else
        {
            topCard.GetComponent<moves>().changeSprite(4);
        }
    }
    private void Torre(int a, int b)
    {
        //abajo izquierda VistaISO
        for (int dummy = 1; dummy < 9; dummy++)
        {
            if (b - dummy >= 0)
            {
                if (ChequearSameTeam(a, b - dummy))
                {
                    PrenderAyudaVisual(a, b - dummy);
                }
                else break;
                if (ChequearEnemy(a, b - dummy))
                {
                    break;
                }

            }
        }
        //Esta parte prende la ayuda roja, debería escribirse mejor
        for (int dummy = 1; dummy < 9; dummy++)
        {
            if (b - dummy >= 0)
            {
                if (ChequearIsOnBoard(a, b - dummy))
                {
                    PrenderAyudaVisual2(a, b - dummy);
                }
            }
        }

        // fin ayuda roja

        //arriba izquierda VistaISO
        for (int dummy = 1; dummy < 9; dummy++)
        {
            if (a - dummy >= 0)
            {
                if (ChequearSameTeam(a - dummy, b))
                {
                    PrenderAyudaVisual(a - dummy, b);
                }
                else break;
                if (ChequearEnemy(a - dummy, b))
                {
                    break;
                }

            }
        }
        //Esta parte prende la ayuda roja, debería escribirse mejor
        for (int dummy = 1; dummy < 9; dummy++)
        {
            if (a - dummy >= 0)
            {
                if (ChequearIsOnBoard(a - dummy, b))
                {
                    PrenderAyudaVisual2(a - dummy, b);
                }
            }
        }
         // fin ayuda roja
        
        //abajo derecha VistaISO
        for (int dummy = 1; dummy < 9; dummy++)
        {
            if (a + dummy <= 10)
            {
                if (ChequearSameTeam(a + dummy, b))
                {
                    PrenderAyudaVisual(a + dummy, b);
                }
                else break;
                if (ChequearEnemy(a + dummy, b))
                {
                    break;
                }

            }
        }
        //Esta parte prende la ayuda roja, debería escribirse mejor
        for (int dummy = 1; dummy < 9; dummy++)
        {
            if (a + dummy <= 10)
            {
                if (ChequearIsOnBoard(a + dummy, b))
                {
                    PrenderAyudaVisual2(a + dummy, b);
                }
            }
        }

        // fin ayuda roja

        //arriba derecha VistaISO
        for (int dummy = 1; dummy < 9; dummy++)
        {
            if (b + dummy <= 10)
            {
                if (ChequearSameTeam(a, b + dummy))
                {
                    PrenderAyudaVisual(a, b + dummy);
                }
                else break;
                if (ChequearEnemy(a, b + dummy))
                {
                    break;
                }

            }
        }
        //Esta parte prende la ayuda roja, debería escribirse mejor
        for (int dummy = 1; dummy < 9; dummy++)
        {
            if (b + dummy <= 10)
            {
                if (ChequearIsOnBoard(a, b + dummy))
                {
                    PrenderAyudaVisual2(a, b + dummy);
                }
            }
        }

        // fin ayuda roja
        if (piezaSostenida.gameObject.transform.parent.name == "PiezasAbajo")
        {
            bottomCard.GetComponent<moves>().changeSprite(3);
        }
        else
        {
            topCard.GetComponent<moves>().changeSprite(3);
        }
    }
    private void Rey(int a, int b)
    {
        Caballo(a, b);
        if (ChequearSameTeam(a - 1, b - 1))
        {
            PrenderAyudaVisual(a - 1, b - 1);
        }
        if (ChequearSameTeam(a - 1, b))
        {
            PrenderAyudaVisual(a - 1, b);
        }
        if (ChequearSameTeam(a, b - 1))
        {
            PrenderAyudaVisual(a, b - 1);
        }
        if (ChequearSameTeam(a, b + 1))
        {
            PrenderAyudaVisual(a, b + 1);
        }
        if (ChequearSameTeam(a + 1, b))
        {
            PrenderAyudaVisual(a + 1, b);
        }
        if (ChequearSameTeam(a + 1, b + 1))
        {
            PrenderAyudaVisual(a + 1, b + 1);
        }


        if (ChequearIsOnBoard(a - 1, b - 1))
        {
            PrenderAyudaVisual2(a - 1, b - 1);
        }
        if (ChequearIsOnBoard(a - 1, b))
        {
            PrenderAyudaVisual2(a - 1, b);
        }
        if (ChequearIsOnBoard(a, b - 1))
        {
            PrenderAyudaVisual2(a, b - 1);
        }
        if (ChequearIsOnBoard(a, b + 1))
        {
            PrenderAyudaVisual2(a, b + 1);
        }
        if (ChequearIsOnBoard(a + 1, b))
        {
            PrenderAyudaVisual2(a + 1, b);
        }
        if (ChequearIsOnBoard(a + 1, b + 1))
        {
            PrenderAyudaVisual2(a + 1, b + 1);
        }


        if (piezaSostenida.gameObject.transform.parent.name == "PiezasAbajo")
        {
            bottomCard.GetComponent<moves>().changeSprite(2);
        }
        else
        {
            topCard.GetComponent<moves>().changeSprite(2);
        }
    }
    private void Reina(int a, int b)
    {
        Torre(a, b);
        Alfil(a, b);
        if (piezaSostenida.gameObject.transform.parent.name == "PiezasAbajo")
        {
            bottomCard.GetComponent<moves>().changeSprite(1);
        }
        else
        {
            topCard.GetComponent<moves>().changeSprite(1);
        }
    }
    private bool CheckCheck()
    { //para todos los elementos del tablero chequear si están jaqueando al rey del otro
        bool check = false;
        //método WET
        if (actualArmy == 1)
        {
            foreach (GameObject piezaSuperior in piezasArriba)
            {
                if (piezaSuperior && piezaSuperior!= pieceHolder01 && piezaSuperior.GetComponent<piece>().checkThisPiece && piezaSuperior.GetComponent<piece>().myType == 6)
                {
                    check = check || Peon2Check(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY,piezaSuperior.GetComponent<piece>().played);
                    if (Peon2Check(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY, piezaSuperior.GetComponent<piece>().played))
                    {
                        piezaSuperior.GetComponent<piece>().prenderOutline();
                    }
                }
                if (piezaSuperior && piezaSuperior != pieceHolder01 && piezaSuperior.GetComponent<piece>().checkThisPiece && piezaSuperior.GetComponent<piece>().myType == 5)
                {
                    check = check || AlfilCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY);
                    if (AlfilCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY))
                    {
                        piezaSuperior.GetComponent<piece>().prenderOutline();
                    }
                }
                if (piezaSuperior && piezaSuperior != pieceHolder01 && piezaSuperior.GetComponent<piece>().checkThisPiece && piezaSuperior.GetComponent<piece>().myType == 4)
                {
                    check = check || CaballoCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY);
                    if (CaballoCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY))
                    {
                        piezaSuperior.GetComponent<piece>().prenderOutline();
                    }

                }
                if (piezaSuperior && piezaSuperior != pieceHolder01 && piezaSuperior.GetComponent<piece>().checkThisPiece && piezaSuperior.GetComponent<piece>().myType == 3)
                {
                    check = check || TorreCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY);
                    if (TorreCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY))
                    {
                        piezaSuperior.GetComponent<piece>().prenderOutline();
                    }
                }
                if (piezaSuperior && piezaSuperior != pieceHolder01 && piezaSuperior.GetComponent<piece>().checkThisPiece && piezaSuperior.GetComponent<piece>().myType == 2)
                {
                    check = check || ReyCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY);
                    if (ReyCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY))
                    {
                        piezaSuperior.GetComponent<piece>().prenderOutline();
                    }
                }
                if (piezaSuperior && piezaSuperior != pieceHolder01 && piezaSuperior.GetComponent<piece>().checkThisPiece && piezaSuperior.GetComponent<piece>().myType == 1)
                {
                    check = check || ReinaCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY);
                    if (ReinaCheck(piezaSuperior.GetComponent<piece>().posX, piezaSuperior.GetComponent<piece>().posY, King1.GetComponent<piece>().posX, King1.GetComponent<piece>().posY))
                    {
                        piezaSuperior.GetComponent<piece>().prenderOutline();
                    }
                }

            }
            return check;
        }
        else
        {
            foreach (GameObject piezaInferior in piezasAbajo)
            {
                if (piezaInferior && piezaInferior != pieceHolder01 && piezaInferior.GetComponent<piece>().myType == 6)
                {
                    check = check || Peon1Check(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY, piezaInferior.GetComponent<piece>().played);
                    if (Peon1Check(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY, piezaInferior.GetComponent<piece>().played))
                    {
                        piezaInferior.GetComponent<piece>().prenderOutline();
                    }
                }
                if (piezaInferior && piezaInferior != pieceHolder01 && piezaInferior.GetComponent<piece>().myType == 5)
                {
                    check = check || AlfilCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY);
                    if (AlfilCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY))
                    {
                        piezaInferior.GetComponent<piece>().prenderOutline();
                    }
                }
                if (piezaInferior && piezaInferior != pieceHolder01 && piezaInferior.GetComponent<piece>().myType == 4)
                {
                    check = check || CaballoCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY);
                    if (CaballoCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY))
                    {
                        piezaInferior.GetComponent<piece>().prenderOutline();
                    }
                }
                if (piezaInferior && piezaInferior != pieceHolder01 && piezaInferior.GetComponent<piece>().myType == 3)
                {
                    check = check || TorreCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY);
                    if (TorreCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY))
                    {
                        piezaInferior.GetComponent<piece>().prenderOutline();
                    }
                }
                if (piezaInferior && piezaInferior != pieceHolder01 && piezaInferior.GetComponent<piece>().myType == 2)
                {
                    check = check || ReyCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY);
                    if (ReyCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY))
                    {
                        piezaInferior.GetComponent<piece>().prenderOutline();
                    }
                }
                if (piezaInferior && piezaInferior != pieceHolder01 && piezaInferior.GetComponent<piece>().myType == 1)
                {
                    check = check || ReinaCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY);
                    if (ReinaCheck(piezaInferior.GetComponent<piece>().posX, piezaInferior.GetComponent<piece>().posY, King2.GetComponent<piece>().posX, King2.GetComponent<piece>().posY))
                    {
                        piezaInferior.GetComponent<piece>().prenderOutline();
                    }
                }
            }
            return check;
        }

    }
    // Estos métodos tienen que desaparecer y juntarse con los otros
    private bool Peon1Check(int a, int b, int reyX, int reyY,bool wasPlayed)
    {
        if ((reyX == a - 1 && reyY == b) || (reyX == a && reyY == b + 1))
        {
            Debug.Log("jaquePeonCorto");

            return true;
        }
        else if (!wasPlayed && ((reyX == a - 2 && reyY == b) || (reyX == a && reyY == b + 2)))
        {
            Debug.Log("jaquePeonLargo");
            return true;
        }
        else return false;
    }
    private bool Peon2Check(int a, int b, int reyX, int reyY,bool wasPlayed)
    {
        if ((reyX == a + 1 && reyY == b) || (reyX == a && reyY == b - 1))
        {
            Debug.Log("jaquePeonCorto");
            return true;
        }
        else if (!wasPlayed && ((reyX == a + 2 && reyY == b) || (reyX == a && reyY == b - 2)))
        {
            Debug.Log("jaquePeonLargo");

            return true;
        }
        else return false;
    }
    private bool AlfilCheck(int a, int b, int reyX, int reyY)
    {
        //Izquierda
        if ((a == reyX + 1 || a == reyX - 1) && (b == reyY + 1 || b == reyY - 1)) {
            Debug.Log("jaqueAlfil");
            return true;
        }
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a - dummy >= 0 && b - dummy >= 0)
            {
                if (ChequearIsEmpty(a - dummy, b - dummy))
                {
                    if (a - dummy - 1 == reyX && b - dummy - 1 == reyY)
                    {
                        Debug.Log("jaqueIzquierdaAlfil");
                        return true;
                    }
                }
                else break;
            }
            else break;
        }
        //Arriba
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a - dummy >= 0 && b - dummy <= 10)
            {
                if (ChequearIsEmpty(a - dummy, b + dummy))
                {
                    if (a - dummy - 1 == reyX && b + dummy + 1 == reyY)
                    {
                        Debug.Log("jaqueArribaAlfil");
                        return true;
                    }
                }
                else break;
            }
            else break;
        }
        //Abajo
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a - dummy <= 10 && b - dummy >= 0)
            {
                if (ChequearIsEmpty(a + dummy, b - dummy))
                {
                    if (a + dummy + 1 == reyX && b - dummy - 1 == reyY)
                    {
                        Debug.Log("jaqueAbajoAlfil");
                        return true;
                    }
                }
                else break;
            }
            else break;
        }
        //Derecha
        for (int dummy = 1; dummy < 11; dummy++)
        {
            if (a - dummy <= 10 && b - dummy <= 10)
            {
                if (ChequearIsEmpty(a + dummy, b + dummy))
                {
                    if (a + dummy + 1 == reyX && b + dummy + 1 == reyY)
                    {
                        Debug.Log("jaqueDerechaAlfil");
                        return true;
                    }
                }
                else break;
            }
            else break;
        }
        return false;
    }
    private bool CaballoCheck(int a, int b, int reyX, int reyY)
    {
        if (a - 2 == reyX && b - 1 == reyY)
        {
            return true;
        }
        else if (a - 1 == reyX && b - 2 == reyY)
        {
                return true;
        }
        else if (a - 1 == reyX && b + 1 == reyY)
        {
            return true;
        }
        else if (a + 1 == reyX && b - 1 == reyY)
        {
            return true;
        }
        else if (a + 1 == reyX && b + 2 == reyY)
        {
            return true;
        }
        else if (a + 2 == reyX && b + 1 == reyY)
        {
            return true;
        }
        else return false;
    }
    private bool TorreCheck(int a, int b, int reyX, int reyY)
    {
        if (a == reyX || b == reyY)
        {
            if (a == reyX && b < reyY)
            {
                if (b == reyY - 1)
                {
                    Debug.Log("jaqueTorre");
                    return true;
                }
                for (int dummy = b+1; dummy < reyY; dummy++)
                {
                    if (ChequearIsEmpty(a, dummy))
                    {
                        if (dummy == reyY-1)
                        {
                            Debug.Log("jaqueTorre");
                            return true;
                        }
                        Debug.Log(a+" " +dummy);
                    }
                    else break;

                }
            }
            if (a == reyX && b > reyY)
            {
                if (b == reyY + 1)
                {
                    Debug.Log("jaqueTorre");
                    return true;
                }
                for (int dummy = reyY+1; dummy < b; dummy++)
                {
                    if (ChequearIsEmpty(a, dummy))
                    {
                        if (dummy == b - 1)
                        {
                            Debug.Log("jaqueTorre");
                            return true;
                        }
                    }
                    else break;

                }
            }
            if (a < reyX && b == reyY)
            {
                if (a == reyX - 1)
                {
                    Debug.Log("jaqueTorre");
                    return true;
                }
                for (int dummy = a+1; dummy < reyX; dummy++)
                {
                    if (ChequearIsEmpty(dummy, b))
                    {
                        if (dummy == reyX - 1)
                        {
                            Debug.Log("jaqueTorre");
                            return true;
                        }
                    }
                    else break;

                }
            }
            if (a > reyX && b == reyY)
            {
                if (a == reyX + 1)
                {
                    Debug.Log("jaqueTorre");
                    return true;
                }
                for (int dummy = reyX+1; dummy < a; dummy++)
                {
                    if (ChequearIsEmpty(dummy,b))
                    {
                        if (dummy == a - 1)
                        {
                            Debug.Log("jaqueTorre");
                            return true;
                        }
                    }
                    else break;

                }
            }
        }
        return false;
    }
    private bool ReyCheck(int a, int b, int reyX, int reyY)
    {
        bool dummyBool=false;
        if (a - 1 == reyX && b - 1 == reyY)
        {
            dummyBool = true;
        }
        else if (a - 1 == reyX && b == reyY)

        {
            dummyBool = true;
        }
        else if (a == reyX && b - 1 == reyY)
        {
            dummyBool = true;
        }
        else if (a == reyX && b + 1 == reyY)
        {
            dummyBool = true;
        }
        else if (a + 1 == reyX && b  == reyY)
        {
            dummyBool = true;
        }
        else if (a + 1 == reyX && b + 1 == reyY)
        {
            dummyBool = true;
        }
        return ((CaballoCheck(a, b, reyX, reyY)) || dummyBool);

    }
    private bool ReinaCheck(int a, int b, int reyX, int reyY)
    {
        return (AlfilCheck(a, b, reyX, reyY) || TorreCheck(a, b, reyX, reyY));
    }

    private void cambiarTurno(){
        Debug.Log("SE VA A CAMBIAR EL TURNO, EL TURNO ES:" + actualArmy);
        if (actualArmy == 2) {
            actualArmy = 1;
        }else {
            actualArmy = 2;
        }

        Debug.Log("SE CAMBIO EL TURNO! EL TURNO ES: " + actualArmy);
    }

}
