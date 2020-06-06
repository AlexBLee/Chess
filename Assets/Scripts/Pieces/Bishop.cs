﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override void FindLegalMoves()
    {
        moves.Clear();

        for (int i = 1; i < 8; i++)
        {
            moves.Add(new Vector2Int(currentCoordinates.x + i, currentCoordinates.y + (i * forwardDirection)));
        }

        for (int i = 1; i < 8; i++)
        {
            moves.Add(new Vector2Int(currentCoordinates.x - i, currentCoordinates.y + (i * forwardDirection)));
        }

    }
}
