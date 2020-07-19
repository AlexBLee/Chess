using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PENWriter : MonoBehaviour
{
    public Board board;
    public string enPassantTile;
    public int consecutivePieceMoves;
    public int moveCount;

    public void WritePosition()
    {
        string PEN = "";

        PEN += WriteRanksAndFiles();

        // write white or black depending on turn
        PEN += (GameManager.instance.whiteTurn) ? " w " : " b ";

        // TODO!!!!!!!!!! : VERY HACKY SOLUTION.. need a better one
        List<Piece> WRookList = board.whitePieces.FindAll(x => x is Rook);
        King k = (King)board.whitePieces.Find(x => x is King);

        List<Piece> BRookList = board.blackPieces.FindAll(x => x is Rook);
        King kx = (King)board.blackPieces.Find(x => x is King);


        if (!k.hasMoved)
        {
            PEN += WriteCastlePossibility(WRookList);
        }

        if (!kx.hasMoved)
        {
            PEN += WriteCastlePossibility(BRookList);
        }

        PEN += (enPassantTile != "") ? " " + enPassantTile : PEN += " - ";
        PEN += " " + consecutivePieceMoves;
        PEN += " " + moveCount;

        Debug.Log(PEN);
    }

    public string WriteCastlePossibility(List<Piece> pieceList)
    {
        string a = "";

        for (int i = pieceList.Count - 1; i >= 0; i--)
        {
            Rook rook = (Rook)pieceList[i];
            char castleChar = ' ';

            if (!rook.hasMoved)
            {
                if (rook.currentCoordinates.x < 4)
                {
                    castleChar = 'q';
                }
                else 
                {
                    castleChar = 'k';
                }
            }
            
            if (rook.render.sharedMaterial == board.pieceWhite)
            {
                castleChar -= ' ';
            }

            if (castleChar != '\0')
            {
                a += castleChar;
            }
        }

        return a;
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
