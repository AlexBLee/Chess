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

        // First move can move 2 tiles
        if (firstMove)
        {
            for (int i = 1; i < 3; i++)
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(currentCoordinates.x, currentCoordinates.y + (i * forwardDirection));
                
                Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

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
        else
        {
            for (int i = -1; i < 2; i++)
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(currentCoordinates.x + i, currentCoordinates.y + (1 * forwardDirection));
                
                // Moving normally
                if (IsInBoard(boardCoordPoint))
                {
                    Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

                    if (i == 0 && !IsPieceAtTile(boardCoordPoint))
                    {
                        moves.Add(boardCoordPoint);
                    }
                    // Attacking pieces on its diagonals
                    else if (i != 0 && IsPieceAtTile(boardCoordPoint) && IsEnemyPiece(boardCoordPoint))
                    {
                        moves.Add(boardCoordPoint);
                    }
                }
            }
        }
    }
}
