using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override void FindLegalMoves()
    {
        for (int i = 1; i < 8; i++)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + i, currentCoordinates.y);

            if (!IsPieceAtTile(boardCoordPoint))
            {
                moves.Add(boardCoordPoint);
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x - i, currentCoordinates.y + (i * forwardDirection));

            if (!IsPieceAtTile(boardCoordPoint))
            {
                moves.Add(boardCoordPoint);
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x, currentCoordinates.y + (i * forwardDirection));

            if (!IsPieceAtTile(boardCoordPoint))
            {
                moves.Add(boardCoordPoint);
            }
            else
            {
                break;
            }
        }

        for (int i = 1; i < 8; i++)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x, currentCoordinates.y - (i * forwardDirection));

            if (!IsPieceAtTile(boardCoordPoint))
            {
                moves.Add(boardCoordPoint);
            }
            else
            {
                break;
            }
        }
    }

}
