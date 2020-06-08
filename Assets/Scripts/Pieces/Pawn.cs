using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    bool firstMove = true;

    // todo: this function is ugly and should be fixed
    public override void FindLegalMoves()
    {
        moves.Clear();

        if (firstMove)
        {
            // Does not use the CalculateMove because of its first move edge case.
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

            firstMove = false;
        }
        else
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x, currentCoordinates.y + (1 * forwardDirection));

            if (!IsPieceAtTile(boardCoordPoint))
            {
                moves.Add(boardCoordPoint);
            }

            Vector2Int boardCoordPoint2 = 
            new Vector2Int(currentCoordinates.x + 1, currentCoordinates.y + (1 * forwardDirection));

            if (IsPieceAtTile(boardCoordPoint2) && IsEnemyPiece(boardCoordPoint2))
            {
                moves.Add(boardCoordPoint2);
            }

            Vector2Int boardCoordPoint3 = 
            new Vector2Int(currentCoordinates.x - 1, currentCoordinates.y + (1 * forwardDirection));

            if (IsPieceAtTile(boardCoordPoint3) && IsEnemyPiece(boardCoordPoint3))
            {
                moves.Add(boardCoordPoint3);
            }
        }
        
    }
}
