using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public bool firstMove = true;

    public override void FindLegalMoves()
    {
        // Does not use the CalculateMove function because of the special movement patterns of the pawn.
        moves.Clear();

        // TODO: very crap solution need to refactor
        if (interactable)
        {
            // First move can move 2 tiles
            if (firstMove)
            {
                for (int i = 1; i < 3; i++)
                {
                    Vector2Int boardCoordPoint = 
                    new Vector2Int(currentCoordinates.x, currentCoordinates.y + (i * forwardDirection));
                    
                    if (!IsPieceAtTile(boardCoordPoint))
                    {
                        moves.Add(boardCoordPoint);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            // Normal movement
            else
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(currentCoordinates.x, currentCoordinates.y + (1 * forwardDirection));
                    
                // Moving normally
                if (IsInBoard(boardCoordPoint))
                {
                    if (!IsPieceAtTile(boardCoordPoint))
                    {
                        moves.Add(boardCoordPoint);
                    }
                }
            }

            AttackDiagonals();

        }
        else
        {
            // If it's not the pawn's turn, add only the attacking squares into the move list.
            for (int i = -1; i < 2; i += 2)
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(currentCoordinates.x + i, currentCoordinates.y + (1 * forwardDirection));

                if (IsInBoard(boardCoordPoint))
                {
                    if (IsPieceAtTile(boardCoordPoint))
                    {
                        if (IsFriendlyPiece(boardCoordPoint))
                        {
                            board.tiles[boardCoordPoint.x][boardCoordPoint.y].piece.defended = true;
                        }
                    }

                    moves.Add(boardCoordPoint);
                }
            }
        }
    }

    public void AttackDiagonals()
    {
        for (int i = -1; i < 2; i += 2)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + i, currentCoordinates.y + (1 * forwardDirection));

            // Moving normally
            if (IsInBoard(boardCoordPoint))
            {
                Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

                if (IsPieceAtTile(boardCoordPoint))
                {
                    if (IsFriendlyPiece(boardCoordPoint))
                    {
                        currentTile.piece.defended = true;
                    }
                    // Attacking pieces on its diagonals
                    else if (IsEnemyPiece(boardCoordPoint))
                    {
                        if (currentTile.piece is King)
                        {
                            ApplyCheck((King)currentTile.piece, boardCoordPoint);
                        }
                        else
                        {
                            moves.Add(boardCoordPoint);
                        }
                    }
                }          
            }
        }
    }
}
