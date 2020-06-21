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
        foreach (Piece piece in board.whitePieces)
        {
            piece.interactable = (piece.interactable == true) ? false : true;
        }

        foreach (Piece piece in board.blackPieces)
        {
            piece.interactable = (piece.interactable == true) ? false : true;
        }
    }

    // Right now checks both directions, 
    // should only check the direction relative to the king and the piece that is checking it.
    public void CheckCheck()
    {
        for (int i = 0; i < board.blackPieces.Count; i++)
        {
            List<Vector2Int> tempCoors = new List<Vector2Int>();

            foreach (Vector2Int move in kingInCheck.line)
            {
                // If any moves that intersect with the piece are found, add them to the list
                if (board.blackPieces[i].moves.Any(x => x == move))
                {
                    tempCoors.Add(move);
                }
            }

            // Change the move list for the pieces that found any intersecting moves
            // If there's nothing, it will give an empty list, as any piece that cant move in the way shouldn't be able to move.
            if (!(board.blackPieces[i] is King))
            {
                board.blackPieces[i].moves = tempCoors;
            }
            
        }
    }
}
