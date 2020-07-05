using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override void FindLegalMoves()
    {
        // forward right side
        CalculateMoves(1, 2, true);
        CalculateMoves(2, 1, true);

        // forward left side
        CalculateMoves(-1, 2, true);
        CalculateMoves(-2, 1, true);

        // backward left side
        CalculateMoves(-2, -1, true);
        CalculateMoves(-1, -2, true);

        // backward right side
        CalculateMoves(1, -2, true);
        CalculateMoves(2, -1, true);
    }
}
