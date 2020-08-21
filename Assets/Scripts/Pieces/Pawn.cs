using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

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
            enPassantPossible = false;

            for (int i = 0; i < moveIncrement; i++)
            {
                Vector2Int boardCoordPoint = 
                new Vector2Int(location.x, location.y + (i + 1) * forwardDirection);
                
                if (IsPieceAtTile(boardCoordPoint))
                {
                    break;
                }
                moves.Add(boardCoordPoint);
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
            if (currentTile == null) { continue; }

            if (IsFriendlyPiece(boardCoordPoint))
            {
                currentTile.piece.defended = true;
            }
            // Attacking pieces on its diagonals
            else if (IsEnemyPiece(boardCoordPoint))
            {
                if (currentTile.piece is King king)
                {
                    ApplyCheck(king);
                }
                else
                {
                    moves.Add(boardCoordPoint);
                }
            }
            
            // If it's not this pawn's turn, add the diagonals regardless if there is a piece or not at those tiles.
            if (!interactable) { moves.Add(boardCoordPoint); }        
        }        
    }

    public void CheckForEnPassant()
    {
        for (int i = -1; i < 2; i += 2)
        {
            Vector2Int boardCoordPoint = 
            new Vector2Int(location.x + i, location.y);

            Tile currentTile = board.GetTile(boardCoordPoint);
            if (currentTile == null) { continue; }

            if (IsEnemyPiece(boardCoordPoint) && currentTile.piece is Pawn pawn && pawn.enPassantPossible)
            {
                Vector2Int enPassantCoordinate = 
                new Vector2Int(location.x + i, location.y + (1 * forwardDirection));

                enPassantTile = enPassantCoordinate;
                GameManager.instance.PENWriter.enPassantTile = board.GetTile(enPassantTile).name;

                moves.Add(enPassantCoordinate);
            }
            
        }
    }

    public override void MoveTo(Tile tile)
    {
        GameManager.instance.PENWriter.consecutivePieceMoves = 0;

        FirstMoveCheck(tile);

        // Make sure the previous Tile no longer owns the piece
        board.tiles[location.x][location.y].piece = null;

        CheckForMovementToEnPassantTile(tile);

        // Move piece to new Tile
        tile.piece = this;
        iTween.MoveTo(gameObject, tile.transform.position + new Vector3(0, 0.5f, 0), 0.5f);
        location = tile.coordinates;

        CheckForMovementToPromotionTile(tile);
    }
    
    public void FirstMoveCheck(Tile tile)
    {
        firstMove = false;

        // If the pawn moves two tiles, then its possible for the pawn to be subject to an en passant.
        enPassantPossible = ((tile.coordinates.y - location.y) == (2 * forwardDirection)) ? true : false;
    }

    public void CheckForMovementToEnPassantTile(Tile tile)
    {        
        Tile tilePieceToDestroy = (tile.coordinates == enPassantTile) ? board.GetTile(new Vector2Int(enPassantTile.x, enPassantTile.y - (1 * forwardDirection))) : tile;

        board.DestroyPieceAt(this, tilePieceToDestroy);
        
    }

    public void CheckForMovementToPromotionTile(Tile tile)
    {
        // Mark end of the board for each side.
        int side = (render.sharedMaterial == board.pieceWhite) ? 7 : 0;

        if (tile.coordinates.y == side)
        {
            if (GameManager.instance.playerControlled)
            {
                GameManager.instance.promotionPanel.gameObject.SetActive(true);
            }
            GameManager.instance.SetGameState(false);
                
            int x = (GameManager.instance.playerControlled) ? GameManager.instance.promotionPanel.number : GameManager.instance.stockfish.promotionNumber;
            StartCoroutine(PromotePiece(tile, x));        
        }
    }

    IEnumerator PromotePiece(Tile tile, int promotionNumber)
    {
        while (!GameManager.instance.promotionPanel.buttonPressed && GameManager.instance.playerControlled)
        {
            yield return null;
        }
        board.DestroyPieceAt(tile);
        board.PlacePiecesAt(tile.coordinates.x, tile.coordinates.y, (Board.PieceType)promotionNumber, render.sharedMaterial);

        GameManager.instance.promotionPanel.gameObject.SetActive(false);
        GameManager.instance.promotionPanel.buttonPressed = false;

        GameManager.instance.SetGameState(true);

        if (GameManager.instance.playerControlled)
        {
            GameManager.instance.NextTurn();
        }
    }

    #region PhotonNetwork Calls

    [PunRPC]
    public override void MoveTo(Vector2 tileLoc)
    {
        Tile tile = board.tiles[(int)tileLoc.x][(int)tileLoc.y];

        GameManager.instance.PENWriter.consecutivePieceMoves = 0;

        FirstMoveCheck(tile);

        photonView.RPC("CheckForMovementToEnPassantTile", RpcTarget.All, tileLoc);

        // Make sure the previous Tile no longer owns the piece
        board.tiles[location.x][location.y].piece = null;

        // Move piece to new Tile
        tile.piece = this;
        iTween.MoveTo(gameObject, tile.transform.position + new Vector3(0, 0.5f, 0), 0.5f);
        location = tile.coordinates;

        CheckForMovementToPromotionTile(tile);
    }

    [PunRPC]
    public void CheckForMovementToEnPassantTile(Vector2 tile)
    {        
        Vector2 tilePieceToDestroy = (tile == enPassantTile) ? new Vector2(enPassantTile.x, enPassantTile.y - (1 * forwardDirection)) : tile;
  
        board.photonView.RPC("DestroyPieceAt", RpcTarget.All, tilePieceToDestroy, new Vector2(location.x, location.y));
    }

    #endregion
}