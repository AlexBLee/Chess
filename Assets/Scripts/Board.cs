using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Tile tile;
    public List<Tile> tiles;
    public List<Piece> pieces;

    private const float TILE_OFFSET = 1.0f;

    public int boardWidth;
    public int boardHeight;

    enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King };
    PieceType pieceType;

    public Material boardWhite;
    public Material boardBlack;
    public Material pieceWhite;
    public Material pieceBlack;


    void Start()
    {
        // making the board
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                tiles.Add
                (
                    Instantiate(tile, 
                    transform.position + new Vector3(TILE_OFFSET * j, 0.0f, TILE_OFFSET * i),
                    Quaternion.Euler(0,0,0), 
                    transform
                ));
            }
        }

        // colour
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                if ((i+j) % 2 == 0)
                {
                    tiles[i + j * boardWidth].render.material = pieceBlack;
                }
                else
                {
                    tiles[i + j * boardWidth].render.material = boardWhite;
                }
            }
        }

        float x = Time.realtimeSinceStartup;
        SpawnPieces();
        print(Time.realtimeSinceStartup - x);


        // adding pieces
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                int index = i + j * 8;
                char nextChar = (char)('a' + i);

                tiles[index].name = nextChar + (j+1).ToString();
            }
        }
    }

    void SpawnPieces()
    {
        // // pawns
        for (int i = 1; i <= 8; i++)
        {
            // white
            PlacePiecesAt(i, 2, PieceType.Pawn, pieceWhite);

            // black
            PlacePiecesAt(i, 7, PieceType.Pawn, pieceBlack);
        }

        // ---------- WHITE --------------------

        // white rooks
        PlacePiecesAt(1,1, PieceType.Rook, pieceWhite);
        PlacePiecesAt(8,1, PieceType.Rook, pieceWhite);

        // white knights
        PlacePiecesAt(2,1, PieceType.Knight, pieceWhite);
        PlacePiecesAt(7,1, PieceType.Knight, pieceWhite);

        // white bishops
        PlacePiecesAt(3,1, PieceType.Bishop, pieceWhite);
        PlacePiecesAt(6,1, PieceType.Bishop, pieceWhite);

        // white queen
        PlacePiecesAt(4,1, PieceType.Queen, pieceWhite);

        // white king
        PlacePiecesAt(5,1, PieceType.King, pieceWhite);

        // -----------BLACK-------------------
        
        // black rooks
        PlacePiecesAt(1,8, PieceType.Rook, pieceBlack);
        PlacePiecesAt(8,8, PieceType.Rook, pieceBlack);
        
        // black knights
        PlacePiecesAt(2,8, PieceType.Knight, pieceBlack);
        PlacePiecesAt(7,8, PieceType.Knight, pieceBlack);

        // black bishops
        PlacePiecesAt(3,8, PieceType.Bishop, pieceBlack);
        PlacePiecesAt(6,8, PieceType.Bishop, pieceBlack);

        // black queen
        PlacePiecesAt(4,8, PieceType.Queen, pieceBlack);

        // black king
        PlacePiecesAt(5,8, PieceType.King, pieceBlack);

    }

    private void PlacePiecesAt(int x, int y, PieceType pieceType, Material material)
    {
        int indexCoor = (x - 1) + (y - 1) * 8;

        tiles[indexCoor].piece = pieces[(int)pieceType];

        tiles[indexCoor].piece = 
        Instantiate(tiles[indexCoor].piece, 
        tiles[indexCoor].transform.position + new Vector3(0, 0.5f, 0),
        Quaternion.Euler(0,90,0),
        transform);

        tiles[indexCoor].piece.SetPieceColor(material);

    }
}
