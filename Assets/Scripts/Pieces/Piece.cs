using System.Collections;
using System.Collections.Generic;
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

    public void CalculateMoves(int xIncrement, int yIncrement, bool singleJump)
    {
        int xStep = xIncrement;
        int yStep = yIncrement;
        int maxJump = 8;

        // List for if a king is checked in the same line
        List<Vector2Int> temp = new List<Vector2Int>();

        if (singleJump)
        {
            maxJump = 2;
        }
        else
        {
            maxJump = 8;
        }

        int inc = 1;
        while (inc < maxJump)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + xStep, currentCoordinates.y + (yStep * forwardDirection));

            if (IsInBoard(boardCoordPoint))
            {
                Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

                if (!IsPieceAtTile(boardCoordPoint))
                {
                    temp.Add(boardCoordPoint);
                    moves.Add(boardCoordPoint);
                }
                else if (IsPieceAtTile(boardCoordPoint))
                {
                    if (IsEnemyPiece(boardCoordPoint))
                    {
                        if (board.tiles[boardCoordPoint.x][boardCoordPoint.y].piece is King)
                        {
                            ApplyCheck((King)board.tiles[boardCoordPoint.x][boardCoordPoint.y].piece, temp);
                        }
                        else
                        {
                            moves.Add(boardCoordPoint);
                        }
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            if (xStep < 0) { xStep -= Mathf.Abs(xIncrement); } else if (xStep > 0) { xStep += xIncrement;}
            if (yStep < 0) { yStep -= Mathf.Abs(yIncrement); } else if (yStep > 0) { yStep += yIncrement;}

            inc++;

        }

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
        king.line = line;
        GameManager.instance.kingInCheck = king;
    }
}
