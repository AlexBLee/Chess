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
    public Vector2Int currentCoordinates;
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
        List<Vector2Int> temp = new List<Vector2Int>();

        int inc = 0;
        while (inc < maxJump)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + xStep, currentCoordinates.y + (yStep * forwardDirection));

            if (IsInBoard(boardCoordPoint))
            {
                Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

                if (!IsPieceAtTile(boardCoordPoint))
                {
                    // If an enemy piece hasn't already appeared, keep adding moves.
                    if (enemyPieceFound == null)
                    {
                        temp.Add(boardCoordPoint);
                        moves.Add(boardCoordPoint);
                    }
                }
                else if (IsPieceAtTile(boardCoordPoint))
                {
                    if (IsFriendlyPiece(boardCoordPoint))
                    {
                        currentTile.piece.defended = true;
                        return;
                    }
                    else if (IsEnemyPiece(boardCoordPoint))
                    {
                        if (currentTile.piece is King)
                        {
                            // If the 1ST ENEMY PIECE found is the King, apply the check
                            if (enemyPieceFound == null)
                            {
                                ApplyCheck((King)currentTile.piece, temp);
                            }
                            // If the 2ND ENEMY PIECE found is the King AFTER the 1ST ENEMY PIECE is found, then the first piece
                            // will only be allowed to move within the line that the king is on as the king will be in 
                            // check otherwise.
                            else
                            {
                                // Need to know ahead of time of what moves the enemy piece can make
                                enemyPieceFound.FindMoveSet();
                                
                                // Get only the moves along the checked line and add it to the enemy piece
                                enemyPieceFound.moves = enemyPieceFound.moves.Intersect(temp).ToList();
                                enemyPieceFound.moves.Add(currentCoordinates);

                                enemyPieceFound.pinned = true;
                            }
                            break;
                        }
                        else
                        {
                            // If a piece has not yet been set, set it and begin looking at the tiles behind the piece
                            if (enemyPieceFound == null)
                            {
                                enemyPieceFound = currentTile.piece;
                                moves.Add(boardCoordPoint);
                            }
                            // If an enemy piece has already been found and the next piece is anything but a king, do nothing and break.
                            else
                            {
                                return;
                            }
                        }
                    }
                }
            }

            if (xStep < 0) { xStep -= Mathf.Abs(xIncrement); } else if (xStep > 0) { xStep += xIncrement;}
            if (yStep < 0) { yStep -= Mathf.Abs(yIncrement); } else if (yStep > 0) { yStep += yIncrement;}

            inc++;

        }

    }

    public virtual void MoveTo(Tile tile)
    {
        // Make sure the previous Tile no longer owns the piece
        board.tiles[currentCoordinates.x][currentCoordinates.y].piece = null;

        // Move piece to new Tile
        tile.piece = this;
        transform.position = tile.transform.position + new Vector3(0, 0.5f, 0);
        currentCoordinates = tile.coordinates;          
    }

    public void ColourAvailableTiles(Tile tile, Material mat)
    {
        tile.previousMat = tile.render.material;
        tile.render.material = mat;
    }

    public virtual void RemoveIllegalMoves()
    {
        moves.RemoveAll(tile => tile.x < 0 || tile.x > 7 || tile.y < 0 || tile.y > 7);
    }

    public bool IsPieceAtTile(Vector2Int tile)
    {
        return board.tiles[tile.x][tile.y].piece != null;
    }

    public bool IsFriendlyPiece(Vector2Int tile)
    {
        return board.tiles[tile.x][tile.y].piece.interactable == interactable;
    }

    public bool IsEnemyPiece(Vector2Int tile)
    {
        return board.tiles[tile.x][tile.y].piece.interactable != interactable;
    }

    public bool IsInBoard(Vector2Int tile)
    {
        return tile.x >= 0 && tile.y >= 0 && tile.x <= 7 && tile.y <= 7;
    }

    public void ApplyCheck(King king, List<Vector2Int> line)
    {
        king.check = true;
        king.line.Add(currentCoordinates);
        king.line.AddRange(line);
        GameManager.instance.kingInCheck = king;
    }

    public void ApplyCheck(King king, Vector2Int tile)
    {
        king.check = true;
        king.line.Add(currentCoordinates);
        king.line.Add(tile);
        GameManager.instance.kingInCheck = king;
    }
}
