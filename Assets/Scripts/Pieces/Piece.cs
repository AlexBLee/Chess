using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // to tell between white and black
    public static Board board;
    public bool interactable;
    public Renderer render;
    public List<Vector2Int> moves;
    public List<Vector2Int> pinnedMoveList;
    public Vector2Int location;
    public int forwardDirection;
    public bool defended;
    public bool pinned;
    
    private void Awake() 
    {
        board = FindObjectOfType<Board>();
    }

    public void SetPieceColor(Material color)
    {
        render.material = color;
    }

    public void FindMoveSet()
    {
        moves.Clear();
        FindLegalMoves();
        RemoveIllegalMoves();
    }

    public virtual void FindLegalMoves()
    {
    }

    // TODO: clean this up.. as usual.
    public void CalculateMoves(int xIncrement, int yIncrement, bool singleJump)
    {
        // Movement
        int xStep = xIncrement;
        int yStep = yIncrement;
        int maxJump = singleJump == true ? 1 : 7;

        // For marking the first enemy piece found in the moveset.
        Piece enemyPieceFound = null;

        // List for if a king is checked in the same line
        List<Vector2Int> line = new List<Vector2Int>();

        for (int i = 0; i < maxJump; i++)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(location.x + xStep, location.y + (yStep * forwardDirection));

            Tile currentTile = board.GetTile(boardCoordPoint);
            if (currentTile == null) { return; }

            if (!IsPieceAtTile(boardCoordPoint) && enemyPieceFound == null)
            {
                // If an enemy piece hasn't already appeared, keep adding moves.
                line.Add(boardCoordPoint);
                moves.Add(boardCoordPoint);
            }
            else if (IsFriendlyPiece(boardCoordPoint) && enemyPieceFound == null)
            {
                currentTile.piece.defended = true;
                return;
            }
            else if (IsEnemyPiece(boardCoordPoint) && currentTile.piece is King king)
            {
                if (enemyPieceFound == null)
                {
                    king.line = new List<Vector2Int>(line);
                    ApplyCheck(king);
                }
                else
                {
                    PinPiece(enemyPieceFound, king, line, currentTile);
                }
            }
            else
            {
                // If an enemy piece has already been found and the next piece is anything but a king, do nothing and break.
                if (enemyPieceFound != null)
                {
                    return;
                }
                // If a piece has not yet been set, set it and begin looking at the tiles behind the piece
                enemyPieceFound = currentTile.piece;
                moves.Add(boardCoordPoint);
            }          
            
            xStep = (xStep < 0) ? xStep -= Mathf.Abs(xIncrement) : xStep += xIncrement;
            yStep = (yStep < 0) ? yStep -= Mathf.Abs(yIncrement) : yStep += yIncrement; 
        }
    }

    private void PinPiece(Piece enemyPieceFound, King kingRef, List<Vector2Int> line, Tile currentTile)
    {

        // If the 2ND ENEMY PIECE found is the King AFTER the 1ST ENEMY PIECE is found, then the first piece
        // will only be allowed to move within the line that the king is on as the king will be in 
        // check otherwise.
        enemyPieceFound.pinnedMoveList.Clear();

        // Need to know ahead of time of what moves the enemy piece can make
        enemyPieceFound.FindMoveSet();
        
        // Get only the moves along the checked line and add it to the enemy piece
        enemyPieceFound.pinnedMoveList = enemyPieceFound.moves.Intersect(line).ToList();

        // Only add current coordinates if its not a pawn as the pawn would be able to teleport and
        // capture pieces way out of it s moveset.
        if (!(enemyPieceFound is Pawn))
        {
            enemyPieceFound.pinnedMoveList.Add(location);
        }

        enemyPieceFound.pinned = true;
        
    }

    public virtual void MoveTo(Tile tile)
    {
        GameManager.instance.PENWriter.consecutivePieceMoves++;
        GameManager.instance.movesWithoutCaptures++;
        
        // Make sure the previous Tile no longer owns the piece
        board.tiles[location.x][location.y].piece = null;

        board.DestroyPieceAt(this, tile);

        // Move piece to new Tile
        tile.piece = this;
        // transform.position = tile.transform.position + new Vector3(0, 0.5f, 0);
        iTween.MoveTo(gameObject, tile.transform.position + new Vector3(0, 0.5f, 0), 1.0f);
        location = tile.coordinates;          
    }

    public void ColourAvailableTiles(Tile tile, Material mat)
    {
        tile.render.material = mat;
    }

    public virtual void RemoveIllegalMoves()
    {
        moves.RemoveAll(tile => tile.x < 0 || tile.x > 7 || tile.y < 0 || tile.y > 7);
    }

    public bool IsPieceAtTile(Vector2Int tile)
    {
        return board.IsInBoard(new Vector2Int(tile.x, tile.y)) && board.tiles[tile.x][tile.y].piece != null;
    }

    public bool IsFriendlyPiece(Vector2Int tile)
    {
        return IsPieceAtTile(tile) && board.tiles[tile.x][tile.y].piece.interactable == interactable;
    }

    public bool IsEnemyPiece(Vector2Int tile)
    {
        return IsPieceAtTile(tile) && board.tiles[tile.x][tile.y].piece.interactable != interactable;
    }

    public void ApplyCheck(King king)
    {
        king.check = true;
        king.canCastleRight = false;
        king.canCastleLeft = false;
        king.line.Add(location);
        GameManager.instance.kingInCheck = king;
    }
}
