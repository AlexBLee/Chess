using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    bool firstMove = true;

    public override void FindLegalMoves()
    {
        // Does not use the CalculateMove function because of the special movement patterns of the pawn.

        moves.Clear();

        if (firstMove)
        {
            for (int i = 1; i < 3; i++)
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(currentCoordinates.x, currentCoordinates.y + (i * forwardDirection));
                
                Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

                if (!IsPieceAtTile(boardCoordPoint))
                {
                    ColourAvailableTiles(currentTile, board.pieceWhite);
                    moves.Add(boardCoordPoint);
                }
                else
                {
                    break;
                }
            }

            firstMove = false;
        }
        else
        {
            for (int i = -1; i < 2; i++)
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(currentCoordinates.x + i, currentCoordinates.y + (1 * forwardDirection));

                Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

                if (i == 0 && !IsPieceAtTile(boardCoordPoint))
                {
                    ColourAvailableTiles(currentTile, board.pieceWhite);
                    moves.Add(boardCoordPoint);
                }
                else if (i != 0 && IsPieceAtTile(boardCoordPoint) && IsEnemyPiece(boardCoordPoint))
                {
                    ColourAvailableTiles(currentTile, board.pieceAttack);
                    moves.Add(boardCoordPoint);
                }

            }
        }
        
    }
}
