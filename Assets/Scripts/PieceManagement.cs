using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManagement : MonoBehaviour
{
    public Piece[,] pieces = new Piece[8,8];
    public GameObject whitePiecePrefab;
    public GameObject blackPiecePrefab;

 
    void Start(){
        GenerateBoard();
    }

    void Update(){

    }
   private void GenerateBoard()
   {
        // generate white team
        for (int y = 0; y < 3; y++){
            for(int x = 0; x < 8; x+=2){
                //generate our piece
                GeneratePiece(x,y);
            }
        }
   }
   //skull
   private void GeneratePiece(int x, int y){
        
        // var newPiece = Instantiate(whitePiecePrefab, new Vector3(x, y));
        // newPiece.name = $"Piece {x} {y}";
        // Piece p = newPiece.GetComponent<Piece>();
        // pieces[x,y] = p;
      
        GameObject go = Instantiate(whitePiecePrefab) as GameObject;
        go.transform.SetParent(transform);
        Piece p = go.GetComponent<Piece>();
        go.name = $"White {x} {y}";
        pieces[x,y] = p;
   }
}
