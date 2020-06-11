using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Tile tile;
    public List<List<Tile>> tiles;
    public List<Piece> pieces;

    private const float TILE_OFFSET = 1.0f;

    [SerializeField] private const int boardWidth = 8;
    [SerializeField] private const int boardHeight = 8;

    enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King };
    PieceType pieceType;

    public Material boardWhite;
    public Material boardBlack;
    public Material pieceWhite;
    public Material pieceBlack;
    public Material pieceAttack;


    void Start()
    {
        tiles = new List<List<Tile>>();

        // making the board
        for (int i = 0; i < boardWidth; i++)
        {
            List<Tile> innerTiles = new List<Tile>();

            for (int j = 0; j < boardHeight; j++)
            {
                innerTiles.Add
                (
                    Instantiate(tile, 
                    transform.position + new Vector3(TILE_OFFSET * i, 0.0f, TILE_OFFSET * j),
                    Quaternion.Euler(0,0,0), 
                    transform
                ));
            }
            tiles.Add(innerTiles);
        }

        // colour and naming tiles
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                int index = i + j * 8;
                char nextChar = (char)('a' + i);

                tiles[i][j].name = nextChar + (j+1).ToString();
                tiles[i][j].coordinates = new Vector2Int(i+1, j+1);

                if ((i+j) % 2 == 0)
                {
                    tiles[i][j].render.material = pieceBlack;
                }
                else
                {
                    tiles[i][j].render.material = boardWhite;
                }
            }
        }

        SpawnPieces();
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

        tiles[x - 1][y - 1].piece = pieces[(int)pieceType];

        tiles[x - 1][y - 1].piece = 
        Instantiate(tiles[x - 1][y - 1].piece, 
        tiles[x - 1][y - 1].transform.position + new Vector3(0, 0.5f, 0),
        Quaternion.Euler(0,90,0),
        transform);

        tiles[x - 1][y - 1].piece.currentCoordinates = new Vector2Int(x, y);

        tiles[x - 1][y - 1].piece.SetPieceColor(material);

        if (material == pieceWhite)
        {
            tiles[x - 1][y - 1].piece.forwardDirection = 1;
            tiles[x - 1][y - 1].piece.playerOwned = true;
        }
        else
        {
            tiles[x - 1][y - 1].piece.forwardDirection = -1;
            tiles[x - 1][y - 1].piece.playerOwned = false;
        }

    }
}
