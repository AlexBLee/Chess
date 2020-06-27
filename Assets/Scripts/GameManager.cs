using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Board board;
    public bool playerTurn = true;
    public bool check;
    public King kingInCheck;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void FindWhiteMoves()
    {
        foreach (Piece piece in board.whitePieces)
        {
            piece.FindMoveSet();
        }
    }

    public void FindBlackMoves()
    {
        foreach (Piece piece in board.blackPieces)
        {
            piece.FindMoveSet();
        }
    }

    public void FindAllPossibleMoves()
    {
        FindWhiteMoves();
        FindBlackMoves();
    }

    public void SwitchSides()
    {
        // Resets checks and defended statuses as well.

        if (kingInCheck != null)
        {
            kingInCheck.check = false;
            kingInCheck = null;
        }

        foreach (Piece piece in board.whitePieces)
        {
            piece.interactable = (piece.interactable == true) ? false : true;
            piece.defended = false;
        }

        foreach (Piece piece in board.blackPieces)
        {
            piece.interactable = (piece.interactable == true) ? false : true;
            piece.defended = false;
        }
    }

    public void CheckKingCheck()
    {
        // Assign the correct list
        List<Piece> pieceList = 
        (kingInCheck.render.sharedMaterial == board.pieceBlack) ? board.blackPieces : board.whitePieces;

        for (int i = 0; i < pieceList.Count; i++)
        {
            List<Vector2Int> tempCoors = new List<Vector2Int>();

            foreach (Vector2Int move in kingInCheck.line)
            {
                // If any moves that intersect with the piece are found, add them to the list
                if (pieceList[i].moves.Any(x => x == move))
                {
                    tempCoors.Add(move);
                }
            }

            // Change the move list for the pieces that found any intersecting moves
            // If there's nothing, it will give an empty list, as any piece that cant move in the way shouldn't be able to move.
            if (!(pieceList[i] is King))
            {
                pieceList[i].moves = tempCoors;
            }
            
        }
    }
}
