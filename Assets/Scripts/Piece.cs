using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject highlight;

    public bool isWhite;
    public bool isKing;

    public bool ValidMove(Piece[,] board, int x1, int y1, int x2, int y2)
    {
        // colliding with existing piece
        if (board[x2, y2] != null)
            return false;


        int deltaMove = Mathf.Abs(x1 - x2);
        // reversed deltaMoveY because the piece names are also reversed. we are adhering to the names, not the code :)
        int deltaMoveY = y2 - y1;

        if (isWhite || isKing)
        {
            if (deltaMoveY == 1)
            {
                if (deltaMove == 1)
                    return true;
            }
            else if (deltaMove == 2)
            {
                if (deltaMoveY == 2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null && p.isWhite != isWhite)
                        return true;
                }
            }
        }

        if (!isWhite || isKing)
        {
            if (deltaMoveY == -1)
            {
                if (deltaMove == 1)
                    return true;
            }
            else if (deltaMove == 2)
            {
                if (deltaMoveY == -2)
                {
                    Piece p = board[(x1 + x2) / 2, (y1 + y2) / 2];
                    if (p != null && p.isWhite != isWhite)
                        return true;
                }
            }
        }

        return false;

    }

    public void SetHighlightStatus(bool status)
    {
        highlight.SetActive(status);
    }

}
