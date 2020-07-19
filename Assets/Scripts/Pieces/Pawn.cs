using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    public bool firstMove = true;
    public bool enPassantPossible = false;
    public Vector2Int enPassantTile;

    public override void FindLegalMoves()
    {
        // Does not use the CalculateMove function because of the special movement patterns of the pawn.

        // TODO: very crap solution need to refactor
        if (interactable)
        {
            // First move can move 2 tiles
            if (firstMove)
            {
                for (int i = 1; i < 3; i++)
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
            }
            // Normal movement
            else
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(currentCoordinates.x, currentCoordinates.y + (1 * forwardDirection));
                    
                // Moving normally
                if (IsInBoard(boardCoordPoint))
                {
                    if (!IsPieceAtTile(boardCoordPoint))
                    {
                        moves.Add(boardCoordPoint);
                    }
                }

                // make enPassantPossible false the turn after it was enabled.
                if (enPassantPossible)
                {
                    enPassantPossible = false;
                }

            }

            CheckForEnPassant();
            AttackDiagonals();

        }
        else
        {
            // If it's not the pawn's turn, add only the attacking squares into the move list.
            for (int i = -1; i < 2; i += 2)
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(currentCoordinates.x + i, currentCoordinates.y + (1 * forwardDirection));

                if (IsInBoard(boardCoordPoint))
                {
                    Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

                    if (IsPieceAtTile(boardCoordPoint))
                    {
                        if (IsFriendlyPiece(boardCoordPoint))
                        {
                            currentTile.piece.defended = true;
                        }
                        // Attacking pieces on its diagonals
                        else if (IsEnemyPiece(boardCoordPoint))
                        {
                            if (currentTile.piece is King)
                            {
                                ApplyCheck((King)currentTile.piece, boardCoordPoint);
                            }
                        }
                    }
                    
                    moves.Add(boardCoordPoint);
                }
            }
        }
    }

    public void AttackDiagonals()
    {
        for (int i = -1; i < 2; i += 2)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + i, currentCoordinates.y + (1 * forwardDirection));

            // Moving normally
            if (IsInBoard(boardCoordPoint))
            {
                Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

                if (IsPieceAtTile(boardCoordPoint))
                {
                    if (IsFriendlyPiece(boardCoordPoint))
                    {
                        currentTile.piece.defended = true;
                    }
                    // Attacking pieces on its diagonals
                    else if (IsEnemyPiece(boardCoordPoint))
                    {
                        if (currentTile.piece is King)
                        {
                            ApplyCheck((King)currentTile.piece, boardCoordPoint);
                        }
                        else
                        {
                            moves.Add(boardCoordPoint);
                        }
                    }
                }          
            }
        }
    }

    public void CheckForEnPassant()
    {
        for (int i = -1; i < 2; i += 2)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(currentCoordinates.x + i, currentCoordinates.y);

            if (IsInBoard(boardCoordPoint) && IsPieceAtTile(boardCoordPoint))
            {
                Tile currentTile = board.tiles[boardCoordPoint.x][boardCoordPoint.y];

                if (IsEnemyPiece(boardCoordPoint) && currentTile.piece is Pawn pawn)
                {
                    if (pawn.enPassantPossible)
                    {
                        Vector2Int enPassantCoordinate = 
                        new Vector2Int(currentCoordinates.x + i, currentCoordinates.y + (1 * forwardDirection));

                        enPassantTile = enPassantCoordinate;
                        GameManager.instance.PENWriter.enPassantTile = board.tiles[enPassantCoordinate.x][enPassantCoordinate.y].name;

                        moves.Add(enPassantCoordinate);
                    }
                }
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
            if ((tile.coordinates.y - currentCoordinates.y) == (2 * forwardDirection))
            {
                enPassantPossible = true;
            }
        }

        // Make sure the previous Tile no longer owns the piece
        board.tiles[currentCoordinates.x][currentCoordinates.y].piece = null;

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
        currentCoordinates = tile.coordinates;

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