using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Rook : Piece
{
    public bool hasMoved;

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
    }

    public override void MoveTo(Tile tile)
    {
        base.MoveTo(tile);
        
        if (!hasMoved)
        {
            hasMoved = true;
        }
    }

    [PunRPC]
    public override void MoveTo(Vector2 tile)
    {
        base.MoveTo(tile);
        
        if (!hasMoved)
        {
            hasMoved = true;
        }
    }

}
