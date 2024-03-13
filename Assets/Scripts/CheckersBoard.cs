using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Vector2 mouseOver;

    private void Start()
    {
        GenerateBoard();
    }

    private void Update()
    {
        UpdateMouseOver();

        Debug.Log(mouseOver);
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
