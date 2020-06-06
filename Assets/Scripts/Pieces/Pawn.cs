using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public override void FindLegalMoves()
    {
        moves.Clear();

        if (!IsPieceAtTile(new Vector2Int(currentCoordinates.x, currentCoordinates.y + forwardDirection)))
        {
            moves.Add(new Vector2Int(currentCoordinates.x, currentCoordinates.y + forwardDirection));
        }
    }
}
