using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PENWriter : MonoBehaviour
{
    public Board board;
    public string enPassantTile;
    public int consecutivePieceMoves;
    public int moveCount;

    public List<Piece> rookList = new List<Piece>();
    public King blackKing;
    public King whiteKing;

    private void Start() 
    {
        blackKing = (King)board.blackPieces.Find(x => x is King);
        whiteKing = (King)board.whitePieces.Find(x => x is King);
        rookList.AddRange(board.blackPieces.FindAll(x => x is Rook));
        rookList.AddRange(board.whitePieces.FindAll(x => x is Rook));
        WritePosition();
    }

    public void WritePosition()
    {
        string PEN = "";

        PEN += WriteRanksAndFiles();
        PEN += (GameManager.instance.whiteTurn) ? " w " : " b ";
        PEN += WriteCastlePossibility();
        PEN += (enPassantTile != "") ? " " + enPassantTile : " - ";
        PEN += " " + consecutivePieceMoves;
        PEN += " " + moveCount;

        Debug.Log(PEN);
    }

    public string WriteCastlePossibility()
    {
        string castleString = "";
        char castleChar = ' ';

        for (int i = rookList.Count - 1; i >= 0; i--)
        {
            Rook rook = (Rook)rookList[i];

            // If kings or rook have moved, skip finding a character for the string.
            if (rook.hasMoved)
            {
                continue;
            }

            if (rook.render.sharedMaterial == board.pieceWhite && whiteKing.hasMoved)
            {
                continue;
            }
            else if (rook.render.sharedMaterial == board.pieceWhite && blackKing.hasMoved)
            {
                continue;
            }

            // King or queen side
            castleChar = (rook.currentCoordinates.x < 4) ? 'q' : 'k';
            
            // Capitalize if white piece.
            if (rook.render.sharedMaterial == board.pieceWhite)
            {
                castleChar -= ' ';
            }

            castleString += castleChar;
        }

        return castleString;
    }

    public string WriteRanksAndFiles()
    {
        string lines = "";
        int emptyTileCount = 0;

        for (int i = 7; i >= 0; i--)
        {
            for (int j = 0; j < 8; j++)
            {
                char pieceChar;
                Piece piece = board.tiles[j][i].piece;

                if (piece != null)
                {
                    if (emptyTileCount != 0)
                    {
                        lines += emptyTileCount;
                    }
                    emptyTileCount = 0;

                    if (piece is Knight)
                    {
                        pieceChar = 'n';
                    }
                    else
                    {
                        pieceChar = piece.name[0];
                    }

                    if (piece.render.sharedMaterial == board.pieceWhite)
                    {
                        pieceChar -= ' ';
                    }

                    lines += pieceChar;
                }
                else
                {
                    emptyTileCount++;
                }
            }

            if (emptyTileCount != 0)
            {
                lines += emptyTileCount;
            }
            emptyTileCount = 0;

            if (i != 0)
            {
                lines += "/";
            }
        }

        return lines;
    }
}
