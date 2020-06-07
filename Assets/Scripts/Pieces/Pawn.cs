using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    bool firstMove = true;

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
            CalculateMoves(0, 1, true);
        }
        
    }
}
