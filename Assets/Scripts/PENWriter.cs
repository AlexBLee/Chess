using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PENWriter : MonoBehaviour
{
    private Board board;
    private List<Piece> rookList = new List<Piece>();
    private King blackKing;
    private King whiteKing;
    
    public string enPassantTile;
    public int consecutivePieceMoves;
    public int moveCount;

    private void Start() 
    {
        board = FindObjectOfType<Board>();

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

    private string WriteCastlePossibility()
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
            else if (rook.render.sharedMaterial != board.pieceWhite && blackKing.hasMoved)
            {
                continue;
            }

            // King or queen side
            castleChar = (rook.location.x < 4) ? 'q' : 'k';
            
            // Capitalize if white piece.
            if (rook.render.sharedMaterial == board.pieceWhite)
            {
                castleChar -= ' ';
            }

            castleString += castleChar;
        }

        return castleString;
    }

    private string WriteRanksAndFiles()
    {
        string lines = "";
        int emptyTileCount = 0;

        // The reason we start at 7 is because PEN notation starts from the 8th rank
        for (int i = 7; i >= 0; i--)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece piece = board.tiles[j][i].piece;

                if (piece != null)
                {
                    // If there is a piece in the middle of the line, add the number of empty tiles before it.
                    lines = AddNumberToLine(lines, emptyTileCount);
                    emptyTileCount = 0;
                
                    lines = AddPieceToLine(lines, piece);
                }
                else
                {
                    emptyTileCount++;
                }
            }

            // For the lines that have only empty tiles
            lines = AddNumberToLine(lines, emptyTileCount);
            emptyTileCount = 0;

            // When we reach the last line, don't put "/"
            if (i != 0)
            {
                lines += "/";
            }
        }

        return lines;
    }

    private string AddNumberToLine(string line, int number)
    {
        return (number != 0) ? line + number : line;
    }

    private string AddPieceToLine(string line, Piece piece)
    {
        char pieceChar;

        // The knight is the only piece that doesn't use their first letter
        pieceChar = (piece is Knight) ? 'n' : piece.name[0];

        // Capitalize the character if white
        if (piece.render.sharedMaterial == board.pieceWhite)
        {
            pieceChar -= ' ';
        }

        return line + pieceChar;
    }
}
