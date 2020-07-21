using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn : Piece
{
    public bool firstMove = true;
    public bool enPassantPossible = false;
    public Vector2Int enPassantTile;

    public override void FindLegalMoves()
    {
        // Does not use the CalculateMove function because of the special movement patterns of the pawn.
        if (interactable)
        {
            // If its the first move, make 2 moves instead of one
            int moveIncrement = (firstMove) ? 2 : 1;
            
            // make enPassantPossible false the turn after it was enabled.
            if (enPassantPossible)
            {
                enPassantPossible = false;
            }

            for (int i = 0; i < moveIncrement; i++)
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(location.x, location.y + (i + 1) * forwardDirection);
                
                if (!IsPieceAtTile(boardCoordPoint))
                {
                    moves.Add(boardCoordPoint);
                }
                else
                {
                    break;
                }
            }       

            CheckForEnPassant();
            AttackDiagonals();

        }
        else
        {
            // If it's not the pawn's turn, add only the attacking squares into the move list.
            AttackDiagonals();
        }
    }

    public void AttackDiagonals()
    {
        for (int i = -1; i < 2; i += 2)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(location.x + i, location.y + (1 * forwardDirection));

            Tile currentTile = board.GetTile(boardCoordPoint);
            if (currentTile == null || currentTile.piece == null) { continue; }

            if (IsFriendlyPiece(boardCoordPoint))
            {
                currentTile.piece.defended = true;
            }
            // Attacking pieces on its diagonals
            else
            {
                if (currentTile.piece is King king)
                {
                    ApplyCheck(king, boardCoordPoint);
                }
                else
                {
                    moves.Add(boardCoordPoint);
                }
            }
            
            
            // If it's not this pawn's turn, add the diagonals regardless if there is a piece or not at those tiles.
            if (!interactable)
            {
                moves.Add(boardCoordPoint);
            }          
        }        
    }

    public void CheckForEnPassant()
    {
        for (int i = -1; i < 2; i += 2)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(location.x + i, location.y);

            Tile currentTile = board.GetTile(boardCoordPoint);
            if (currentTile == null || currentTile.piece == null) { continue; }

            if (!IsFriendlyPiece(boardCoordPoint) && currentTile.piece is Pawn pawn && pawn.enPassantPossible)
            {
                Vector2Int enPassantCoordinate = 
                new Vector2Int(location.x + i, location.y + (1 * forwardDirection));

                enPassantTile = enPassantCoordinate;
                GameManager.instance.PENWriter.enPassantTile = currentTile.name;

                moves.Add(enPassantCoordinate);
            }
            
        }
    }

    public override void MoveTo(Tile tile)
    {
        GameManager.instance.PENWriter.consecutivePieceMoves = 0;

        if (firstMove)
        {
            firstMove = false;

            // If the pawn moves two tiles, then its possible for the pawn to be subject to an en passant.
            if ((tile.coordinates.y - location.y) == (2 * forwardDirection))
            {
                enPassantPossible = true;
            }
        }

        // Make sure the previous Tile no longer owns the piece
        board.tiles[location.x][location.y].piece = null;

        if (tile.coordinates == enPassantTile)
        {
            // Pass by and destroy the pawn at the tile
            board.DestroyPieceAt(this, board.tiles[enPassantTile.x][enPassantTile.y - (1 * forwardDirection)]);
        }
        else
        {
            board.DestroyPieceAt(this, tile);
        }

        // Move piece to new Tile
        tile.piece = this;
        transform.position = tile.transform.position + new Vector3(0, 0.5f, 0);
        location = tile.coordinates;

        // Mark end of the board for each side.
        int side = (render.sharedMaterial == board.pieceWhite) ? 7 : 0;

        if (tile.coordinates.y == side)
        {
            GameManager.instance.promotionPanel.gameObject.SetActive(true);
            GameManager.instance.SetGameState(false);
    
            StartCoroutine(PromotePiece(tile));        
        }
    }

    IEnumerator PromotePiece(Tile tile)
    {
        while (!GameManager.instance.promotionPanel.buttonPressed)
        {
            yield return null;
        }
        board.DestroyPieceAt(tile);
        board.PlacePiecesAt(tile.coordinates.x, tile.coordinates.y, (Board.PieceType)GameManager.instance.promotionPanel.number, render.sharedMaterial);

        // VERY UGLY WAY to determine interactable.. definitely need refactor
        if (tile.piece.render.sharedMaterial == board.pieceWhite)
        {
            tile.piece.interactable = board.whitePieces[0].interactable;
        }
        else
        {
            tile.piece.interactable = board.blackPieces[0].interactable;

        }

        GameManager.instance.promotionPanel.gameObject.SetActive(false);
        GameManager.instance.promotionPanel.buttonPressed = false;

        GameManager.instance.SetGameState(true);
        GameManager.instance.NextTurn();
    }
}