using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Board board;
    public bool playerTurn = true;

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
}
