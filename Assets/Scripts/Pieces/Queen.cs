using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override void FindLegalMoves()
    {
        for (int i = 1; i < 8; i++)
        {
            moves.Add(new Vector2Int(currentCoordinates.x - i, currentCoordinates.y));
        }

        for (int i = 1; i < 8; i++)
        {
            moves.Add(new Vector2Int(currentCoordinates.x + i, currentCoordinates.y));
        }

        for (int i = 1; i < 8; i++)
        {
            moves.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y + (i * forwardDirection)));
        }


    }
}
