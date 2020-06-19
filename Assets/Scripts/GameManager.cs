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
            foreach (Vector2Int move in kingInCheck.checkPiece.moves)
            {
                for (int j = 0; j < board.blackPieces[i].moves.Count; j++)
                {
                    if (board.blackPieces[i].moves[j] != move)
                    {
                        Debug.Log("Found move: " + move + " From: " + board.blackPieces[i] + " at " + board.blackPieces[i].currentCoordinates);
                        // board.blackPieces[i].moves.Remove(board.blackPieces[i].moves[j]);
                    }
                }
            }
            
        }
    }
}
