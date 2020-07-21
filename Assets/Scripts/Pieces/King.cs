using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class King : Piece
{
    public bool check;
    public bool hasMoved;
    public bool checkDefended;

    public List<Vector2Int> line;
    private List<Vector2Int> castleMoveList = new List<Vector2Int>();

    public bool canCastleRight;
    public bool canCastleLeft;

    public override void FindLegalMoves()
    {
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

    private void RemoveKingMoves(List<Piece> opposingPieces)
    {
        RemoveAttackedTiles(opposingPieces);
        RemoveDefendedPieceTiles();
    }

    private void RemoveAttackedTiles(List<Piece> opposingPieces)
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
    }

    private void RemoveDefendedPieceTiles()
    {
        // Remove moves where the piece is defended.
        for (int i = 0; i < moves.Count; i++)
        {
            Piece piece = board.tiles[moves[i].x][moves[i].y].piece;
            moves.RemoveAll(x => piece != null && piece.defended);
        }
    }

    private bool CheckCastle(int direction)
    {
        bool canCastle = true;
        
        for (int i = 0; i < 4; i++)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(location.x + (i + 1) * direction, location.y);
            
            // skip adding the x + 1 tile because its not a castle tile
            if (i != 0)
            {
                castleMoveList.Add(boardCoordPoint);
            }
            
            if (IsPieceAtTile(boardCoordPoint) && !CheckForCastleRooks(boardCoordPoint))
            {
                return false;
            }
        }

        return canCastle;
    }

    private bool CheckForCastleRooks(Vector2Int boardCoordPoint)
    {
        bool canCastle = false;

        // If there is nothing between the rook and the king..
        if (board.tiles[boardCoordPoint.x][boardCoordPoint.y].piece is Rook rook && !rook.hasMoved)
        {
            // If the rook found hasn't moved, you can castle.
            canCastle = true;
            moves.AddRange(castleMoveList);
            moves = moves.Distinct().ToList();
        }

        return canCastle;

    }

    private Tile MoveAndCastleKing(Tile tile, int side)
    {
        // Choose the correct rook
        int rookPositionX = (side > 0) ? 7 : 0;
        int rookPositionY = (render.sharedMaterial == board.pieceWhite) ? 0 : 7;

        // Allocate the tiles that the pieces are supposed to switch to.
        tile = board.tiles[location.x + (2 * side)][rookPositionY];
        Tile castleRookTile = board.tiles[location.x + (1 * side)][rookPositionY];

        // Move the rook to the position
        board.tiles[rookPositionX][rookPositionY].piece.MoveTo(castleRookTile);

        return tile;
    }

    public override void MoveTo(Tile tile)
    {
        // Make sure the previous Tile no longer owns the piece
        board.tiles[location.x][location.y].piece = null;

        board.DestroyPieceAt(this, tile);

        if (canCastleRight && castleMoveList.Any(x => x == tile.coordinates && tile.coordinates.x > 4))
        {
            tile = MoveAndCastleKing(tile, 1);
        }
        else if (canCastleLeft && castleMoveList.Any(x => x == tile.coordinates && tile.coordinates.x < 4))
        {
            tile = MoveAndCastleKing(tile, -1);
        }

        // Move piece to new Tile
        tile.piece = this;
        transform.position = tile.transform.position + new Vector3(0, 0.5f, 0);
        location = tile.coordinates;  
       
        if (!hasMoved)
        {
            hasMoved = true;
            canCastleRight = false;
        }
    }
}
