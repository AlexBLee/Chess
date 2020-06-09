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
            for (int i = -1; i < 2; i++)
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(currentCoordinates.x + i, currentCoordinates.y + (1 * forwardDirection));

                if (i == 0 && !IsPieceAtTile(boardCoordPoint))
                {
                    moves.Add(boardCoordPoint);
                }
                else if (i != 0 && IsPieceAtTile(boardCoordPoint) && IsEnemyPiece(boardCoordPoint))
                {
                    moves.Add(boardCoordPoint);
                }

            }
        }
        
    }
}
