using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckersBoard : MonoBehaviour
{
    public Piece[,] pieces = new Piece[8, 8];
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;

    // declare the board's dimensions for when we need to check if the mouse is in the board
    // assumes each tile is 1x1
    public int boardWidth = 8;
    public int boardHeight = 8;

    private Vector2 boardOffset = new Vector2(-4f, -4f);
    private Vector2 pieceOffset = new Vector2(0.5f, 0.5f);

    private bool isWhite;
    private bool isWhiteTurn;

    private Piece selectedPiece;

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;

    private void Start()
    {
        GenerateBoard();
        isWhiteTurn = true;
    }

    private void Update()
    {
        UpdateMouseOver();

        // if my turn
        // this code is for drag and drop
        /*
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;

            if (Input.GetMouseButtonDown(0))
                SelectPiece(x, y);

            if (Input.GetMouseButtonUp(0))
                TryMove((int) startDrag.x, (int) startDrag.y, x, y);
        }
        */

        if (Input.GetMouseButtonDown(0))
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;

            // If a piece is selected, try to move it
            if (selectedPiece != null)
            {
                TryMove((int)startDrag.x, (int)startDrag.y, x, y);
            }
            else // Otherwise, select the piece at the current mouse position
            {
                SelectPiece(x, y);
            }
        }

    }

    private bool IsMouseOverBoard()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10; // place mouse above all else

        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // fix offset
        worldMousePos.x -= boardOffset.x;
        worldMousePos.y -= boardOffset.y;

        // Check if mouse is in bounds
        return worldMousePos.x >= 0 && worldMousePos.x < boardWidth && worldMousePos.y >= 0 && worldMousePos.y < boardHeight;
    }

    private void UpdateMouseOver()
    {
        if (IsMouseOverBoard())
        {
            // get mouse position, converts position to be relative to world, then floor that fucker
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseOver.x = Mathf.FloorToInt(mousePos.x - boardOffset.x);
            mouseOver.y = Mathf.FloorToInt(mousePos.y - boardOffset.y);
        }
        else
        {
            mouseOver = new Vector2(-1, -1);
        }
    }

    private void SelectPiece(int x, int y)
    {
        // out of bounds check
        if (x < 0 || x >= pieces.Length || y < 0 || y >= pieces.Length)
        {
            return;
        }

        Piece p = pieces[x, y];
        if (p != null)
        {
            selectedPiece = p;

            //*****DELETE*ME*WHEN*DONE******
            Debug.Log(selectedPiece.name);
            //******************************

            startDrag = mouseOver;
        }

    }

    // TODO: clean up debug statements in this function
    private void TryMove(int x1, int y1, int x2, int y2)
    {
        // multiplayer support
        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);

        selectedPiece = pieces[x1, y1];

        // check if move is out of bounds
        if (x2 < 0 || x2 >= pieces.Length || y2 < 0 || y2 >= pieces.Length)
        {
            Debug.Log("1");
            if (selectedPiece != null)
                MovePiece(selectedPiece, x1, y1);

            startDrag = Vector2.zero;
            selectedPiece = null;
            return;
        }

        if (selectedPiece != null)
        {
            Debug.Log("2");
            // piece has not moved from original spot
            if (endDrag == startDrag)
            {
                Debug.Log("3");
                MovePiece(selectedPiece, x1, y1);

                startDrag = Vector2.zero;
                selectedPiece = null;
                return;
            }

            // check validity of move
            if (selectedPiece.ValidMove(pieces, x1, y1, x2, y2))
            {
                Debug.Log("4");
                // check for kills
                // are we sure this works here ... FIXME
                if (Mathf.Abs(x2 - x2) == 2)
                {
                    Debug.Log("5");
                    Piece p = pieces[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null)
                    {
                        Debug.Log("6");
                        pieces[(x1 + x2) / 2, (y1 + y2) / 2] = null;
                        Destroy(p);
                    }
                }

                Debug.Log("7");

                pieces[x2, y2] = selectedPiece;
                pieces[x1, y1] = null;
                MovePiece(selectedPiece, x2, y2);

                EndTurn();

            }
            else
            {
                Debug.Log("8");
                MovePiece(selectedPiece, x1, y1);

                startDrag = Vector2.zero;
                selectedPiece = null;
                return; // this return statement is redundant
            }
        }
    }

    private void EndTurn()
    {
        selectedPiece = null;
        startDrag = Vector2.zero;

        isWhiteTurn = !isWhiteTurn;
        CheckVictory();
    }

    private bool CheckVictory()
    {
        return false;
    }

    private void GenerateBoard()
    {
        // Generate white team
        for (int y = 0; y < 3; y++)
        {
            bool oddRow = (y % 2 == 0); 
            for (int x = 0; x < 8; x+=2)
            {
                // Generate piece
                GeneratePiece( (oddRow) ? x : x + 1, y);
            }
        }

        // Generate black team
        for (int y = 7; y > 4; y--)
        {
            bool oddRow = (y % 2 == 0);
            for (int x = 0; x < 8; x += 2)
            {
                // Generate piece
                GeneratePiece((oddRow) ? x : x + 1, y);
            }
        }
    }

    private void GeneratePiece(int x, int y)
    {
        bool isPieceWhite = (y > 3) ? false : true;
        GameObject go = Instantiate((isPieceWhite) ? whitePiecePrefab : blackPiecePrefab) as GameObject;
        go.transform.SetParent(transform);
        Piece p = go.GetComponent<Piece>();
        pieces[x, y] = p;
        MovePiece(p, x, y);
    }

    private void MovePiece(Piece p, int x, int y)
    {
        p.transform.position = new Vector3(x + boardOffset.x + pieceOffset.x, y + boardOffset.y + pieceOffset.y, -3f);

    }
}
