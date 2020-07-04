using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class King : Piece
{
    public bool check;
    public bool hasMoved;

    public List<Vector2Int> line;
    public List<Vector2Int> castleMoveList;

    public bool canCastleRight;
    public bool canCastleLeft;

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
            // Check both directions for castling
            canCastleRight = CheckCastle(1);
            canCastleLeft = CheckCastle(-1);
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

    public bool CheckCastle(int direction)
    {
        bool canCastle = false;
        
        for (int i = 0; i < 4; i++)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + (i + 1) * direction, currentCoordinates.y);

            if (IsInBoard(boardCoordPoint))
            {
                // skip adding the x + 1 tile because its not a castle tile
                if (i != 0)
                {
                    castleMoveList.Add(boardCoordPoint);
                }
                
                if (IsPieceAtTile(boardCoordPoint))
                {
                    // If there is nothing between the rook and the king..
                    if (board.tiles[boardCoordPoint.x][boardCoordPoint.y].piece is Rook rook)
                    {
                        // If the rook found hasn't moved, you can castle.
                        if (!rook.hasMoved)
                        {
                            canCastle = true;
                            moves.AddRange(castleMoveList);
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

        return canCastle;
    }

    public void MoveAndCastleKing(Tile tile)
    {
        // Allocate the tiles that the pieces are supposed to switch to.
        tile = board.tiles[currentCoordinates.x + 2][0];
        Tile rookTile = board.tiles[currentCoordinates.x + 1][0];

        // Move the rook to the position
        board.tiles[7][0].piece.MoveTo(rookTile);
    }

    public override void MoveTo(Tile tile)
    {
        // Make sure the previous Tile no longer owns the piece
        board.tiles[currentCoordinates.x][currentCoordinates.y].piece = null;

        // Scuffed way to castle.. but a way to castle it is..
        if (canCastleRight && castleMoveList.Any(x => x == tile.coordinates))
        {
            MoveAndCastleKing(tile);
        }

        // Move piece to new Tile
        tile.piece = this;
        transform.position = tile.transform.position + new Vector3(0, 0.5f, 0);
        currentCoordinates = tile.coordinates;  
       
        if (!hasMoved)
        {
            hasMoved = true;
            canCastleRight = false;
        }
    }
}
