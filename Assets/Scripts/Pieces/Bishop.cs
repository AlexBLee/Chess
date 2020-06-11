﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override void FindLegalMoves()
    {
        moves.Clear();

        // diagonal right
        CalculateMoves(1, 1, false);

        // diagonal left
        CalculateMoves(-1, 1, false);

        // diagonal backwards right
        CalculateMoves(1, -1, false);

        // diagonal backwards left
        CalculateMoves(-1, -1, false);

    }
}