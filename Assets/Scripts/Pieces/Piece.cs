using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // to tell between white and black
    public static Board board;
    public bool playerOwned;
    public Renderer render;
    public List<Vector2Int> moves;
    public Vector2Int currentCoordinates;
    public int forwardDirection;
    
    private void Start() 
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
            // need to refactor the code to be more cleaner
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + xStep, currentCoordinates.y + (yStep * forwardDirection));

            if (IsInBoard(boardCoordPoint) && !IsPieceAtTile(boardCoordPoint))
            {
                board.tiles[boardCoordPoint.x][boardCoordPoint.y].previousMat = board.tiles[boardCoordPoint.x][(boardCoordPoint.y)].render.material;
                board.tiles[(boardCoordPoint.x)][(boardCoordPoint.y)].render.material = board.pieceWhite;
                moves.Add(boardCoordPoint);
            }
            else if (IsInBoard(boardCoordPoint) && IsPieceAtTile(boardCoordPoint))
            {
                if (IsEnemyPiece(boardCoordPoint))
                {
                    board.tiles[(boardCoordPoint.x)][(boardCoordPoint.y)].previousMat = board.tiles[(boardCoordPoint.x)][(boardCoordPoint.y)].render.material;
                    board.tiles[(boardCoordPoint.x)][(boardCoordPoint.y)].render.material = board.pieceAttack;
                    moves.Add(boardCoordPoint);
                    
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

            if (xStep < 0) { xStep -= Mathf.Abs(xIncrement); } else if (xStep > 0) { xStep += xIncrement;}
            if (yStep < 0) { yStep -= Mathf.Abs(yIncrement); } else if (yStep > 0) { yStep += yIncrement;}

            inc++;

        }

    }

    public void RemoveIllegalMoves()
    {
        moves.RemoveAll(tile => tile.x < 0 || tile.x > 7 || tile.y < 0 || tile.y > 7);
    }

    public bool IsPieceAtTile(Vector2Int tile)
    {
        return board.tiles[tile.x][tile.y].piece != null;
    }

    public bool IsFriendlyPiece(Vector2Int tile)
    {
        return board.tiles[tile.x][tile.y].piece.playerOwned == playerOwned;
    }

    public bool IsEnemyPiece(Vector2Int tile)
    {
        return board.tiles[tile.x][tile.y].piece.playerOwned != playerOwned;
    }

    public bool IsInBoard(Vector2Int tile)
    {
        return tile.x >= 0 && tile.y >= 0 && tile.x <= 7 && tile.y <= 7;
    }
}
