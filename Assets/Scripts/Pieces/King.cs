using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class King : Piece
{
    public bool check;
    public List<Vector2Int> line;
    public bool canCastle;
    public List<Vector2Int> castleMoveList;
    
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

        CheckCastle();
    }

    public override void RemoveIllegalMoves()
    {
        base.RemoveIllegalMoves();

        if (render.sharedMaterial == board.pieceBlack)
        {
            RemoveKingMoves(board.whitePieces);
        }
        else
        {
            RemoveKingMoves(board.blackPieces);
        }
        
    }

    public void RemoveKingMoves(List<Piece> opposingPieces)
    {
        // Remove tiles that are being attacked by opposing pieces.
        foreach (Piece piece in opposingPieces)
        {
            for (int i = 0; i < piece.moves.Count; i++)
            {
                if (piece != null && moves.Contains(piece.moves[i]))
                {
                    moves.Remove(piece.moves[i]);
                }
            }
        }       

        // Remove moves where the piece is defended.
        foreach (Vector2Int move in moves)
        {
            Piece piece = board.tiles[move.x][move.y].piece;

            if (piece != null && piece.defended)
            {
                moves.Remove(move);
            }
        }
    }

    public void CheckCastle()
    {
        int inc = 1;
        while (inc < 5)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + inc, currentCoordinates.y);

            if (IsInBoard(boardCoordPoint) && IsPieceAtTile(boardCoordPoint))
            {
                if (board.tiles[boardCoordPoint.x][boardCoordPoint.y].piece is Rook)
                {
                    canCastle = true;
                }
                break;
            }
            
            inc++;
        }
    }

}
