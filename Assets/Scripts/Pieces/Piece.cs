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

    public void CalculateMoves(int xIncrement, int yIncrement)
    {
        int inc = 1;
        while (inc < 8)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + xIncrement, currentCoordinates.y + (yIncrement * forwardDirection));

            if (boardCoordPoint.x > 0 && boardCoordPoint.y > 0 && !IsPieceAtTile(boardCoordPoint))
            {
                moves.Add(boardCoordPoint);
            }
            else
            {
                break;
            }

            xIncrement = xIncrement > 0 ? xIncrement + 1 : xIncrement - 1;
            yIncrement = yIncrement > 0 ? yIncrement + 1 : yIncrement - 1;

            inc++;

        }
    }

    public void RemoveIllegalMoves()
    {
        moves.RemoveAll(tile => tile.x < 1 || tile.x > 8 || tile.y < 1 || tile.y > 8);

        for (int i = 0; i < moves.Count; i++)
        {
            board.tiles[(moves[i].x - 1) + (moves[i].y - 1) * 8].render.material = board.pieceWhite;
        }
    }

    public bool IsPieceAtTile(Vector2Int tile)
    {
        return board.tiles[(tile.x - 1) + (tile.y - 1) * 8].piece != null;
    }
}
