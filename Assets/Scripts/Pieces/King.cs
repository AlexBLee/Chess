using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class King : Piece
{
    public List<Vector2Int> line;
    public List<Vector2Int> castleMoveList;
    public bool check;
    public bool hasMoved;
    public bool canCastle;
    
    public override void FindLegalMoves()
    {
        moves.Clear();
        castleMoveList.Clear();
        
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

        if (!hasMoved)
        {
            CheckCastle();
        }
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
        for (int i = 1; i < 5; i++)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + i, currentCoordinates.y);

            if (IsInBoard(boardCoordPoint))
            {
                castleMoveList.Add(boardCoordPoint);
                
                if (IsPieceAtTile(boardCoordPoint))
                {
                    // If there is nothing between the rook and the king..
                    if (board.tiles[boardCoordPoint.x][boardCoordPoint.y].piece is Rook rook)
                    {
                        // If the rook found hasn't moved, you can castle.
                        if (!rook.hasMoved)
                        {
                            canCastle = true;
                            break;
                        }
                        else
                        {
                            canCastle = false;
                        }
                    }
                    else
                    {
                        canCastle = false;
                        break;
                    }
                }
            }
        }
    }

    public override void MoveTo(Tile tile)
    {
        base.MoveTo(tile);
        
        if (!hasMoved)
        {
            hasMoved = true;
            canCastle = false;
        }
    }
}
