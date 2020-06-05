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
            moves.Add(new Vector2(currentCoordinates.x + i, currentCoordinates.y + i));
        }

        for (int i = 1; i < 8; i++)
        {
            moves.Add(new Vector2(currentCoordinates.x - i, currentCoordinates.y + i));
        }

        moves.RemoveAll(tile => tile.x < 1 || tile.x > 8 || tile.y < 1 || tile.y > 8);
    }
}
