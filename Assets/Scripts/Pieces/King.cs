using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class King : Piece
{
    public override void FindLegalMoves()
    {
        moves.Clear();
        
        // right
        CalculateMoves(1, 0, true);

        // left
        CalculateMoves(-1, 0, true);

        // forward
        CalculateMoves(0, 1, true);

        // backwards
        CalculateMoves(0, -1, true);

        // diagonal right
        CalculateMoves(1, 1, true);

        // diagonal left
        CalculateMoves(-1, 1, true);

        // diagonal backwards right
        CalculateMoves(1, -1, true);

        // diagonal backwards left
        CalculateMoves(-1, -1, true);

    }

    public override void RemoveIllegalMoves()
    {
        base.RemoveIllegalMoves();

        if (render.sharedMaterial == board.pieceBlack)
        {
            RemoveKingMoves(board.whitePieces);
        }
        else if (render.sharedMaterial == board.pieceWhite)
        {
            RemoveKingMoves(board.blackPieces);
        }
        
    }

    // TODO: further optimize if needed
    public void RemoveKingMoves(List<Piece> opposingMoves)
    {
        for (int i = 0; i < opposingMoves.Count; i++)
        {
            for (int j = 0; j < opposingMoves[i].moves.Count; j++)
            {
                if (moves.Contains(opposingMoves[i].moves[j]))
                {
                    moves.Remove(opposingMoves[i].moves[j]);
                }
            }
        }

    } 
}
