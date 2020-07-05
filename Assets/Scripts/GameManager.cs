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
            if (!piece.pinned)
            {
                piece.FindMoveSet();
            }
            else if (piece.pinned && piece.interactable)
            {
                piece.moves = piece.pinnedMoveList;
            }
        }
    }

    public void FindBlackMoves()
    {
        foreach (Piece piece in board.blackPieces)
        {
            if (!piece.pinned)
            {
                piece.FindMoveSet();
            }
            else if (piece.pinned && piece.interactable)
            {
                piece.moves = piece.pinnedMoveList;
            }
        }
    }

    public void FindAllPossibleMoves()
    {
        FindWhiteMoves();
        FindBlackMoves();

        // King movesets are found at the end because if one king is called first in FindXMoves without the opposite
        // side piece's to have been updated, the king will only take the previous moves into consideration.
        // PS: may need to find a better way to write this.
        King k = (King)board.whitePieces.Find(x => x is King);
        k.FindMoveSet();

        King kx = (King)board.blackPieces.Find(x => x is King);
        kx.FindMoveSet();
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
            piece.pinned = false;
        }

        foreach (Piece piece in board.blackPieces)
        {
            piece.interactable = (piece.interactable == true) ? false : true;
            piece.defended = false;
            piece.pinned = false;

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
