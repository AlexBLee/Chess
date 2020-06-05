using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override void FindLegalMoves()
    {
        moves.Clear();
        moves.Add(new Vector2(currentCoordinates.x, currentCoordinates.y + forwardDirection));
    }
}
