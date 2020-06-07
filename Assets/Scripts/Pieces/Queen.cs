using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override void FindLegalMoves()
    {
        // right
        CalculateMoves(1, 0, false);

        // left
        CalculateMoves(-1, 0, false);

        // forward
        CalculateMoves(0, 1, false);

        // backwards
        CalculateMoves(0, -1, false);

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
