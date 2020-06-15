using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Tile tile;
    public List<List<Tile>> tiles;

    // Holds the type of pieces that can be spawned
    public List<Piece> pieces;

    // To keep a list of current pieces for each side on the board.
    public List<Piece> whitePieces;
    public List<Piece> blackPieces;

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

    public Material availableMoveColour;


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
                tiles[i][j].coordinates = new Vector2Int(i, j);

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
        GameManager.instance.FindAllPossibleMoves();
    }

    void SpawnPieces()
    {
        // // pawns
        for (int i = 0; i < 8; i++)
        {
            // white
            PlacePiecesAt(i, 1, PieceType.Pawn, pieceWhite);

            // black
            PlacePiecesAt(i, 6, PieceType.Pawn, pieceBlack);
        }

        // ---------- WHITE --------------------

        // white rooks
        PlacePiecesAt(0,0, PieceType.Rook, pieceWhite);
        PlacePiecesAt(7,0, PieceType.Rook, pieceWhite);

        // white knights
        PlacePiecesAt(1,0, PieceType.Knight, pieceWhite);
        PlacePiecesAt(6,0, PieceType.Knight, pieceWhite);

        // white bishops
        PlacePiecesAt(2,0, PieceType.Bishop, pieceWhite);
        PlacePiecesAt(5,0, PieceType.Bishop, pieceWhite);

        // white queen
        PlacePiecesAt(3,0, PieceType.Queen, pieceWhite);

        // white king
        PlacePiecesAt(4,0, PieceType.King, pieceWhite);

        // -----------BLACK-------------------
        
        // black rooks
        PlacePiecesAt(0,7, PieceType.Rook, pieceBlack);
        PlacePiecesAt(7,7, PieceType.Rook, pieceBlack);
        
        // black knights
        PlacePiecesAt(1,7, PieceType.Knight, pieceBlack);
        PlacePiecesAt(6,7, PieceType.Knight, pieceBlack);

        // black bishops
        PlacePiecesAt(2,7, PieceType.Bishop, pieceBlack);
        PlacePiecesAt(5,7, PieceType.Bishop, pieceBlack);

        // black queen
        PlacePiecesAt(3,7, PieceType.Queen, pieceBlack);

        // black king
        PlacePiecesAt(4,7, PieceType.King, pieceBlack);

    }

    private void PlacePiecesAt(int x, int y, PieceType pieceType, Material material)
    {
        Tile selectedTile = tiles[x][y];

        selectedTile.piece = pieces[(int)pieceType];

        selectedTile.piece = 
        Instantiate(selectedTile.piece, 
        tiles[x][y].transform.position + new Vector3(0, 0.5f, 0),
        Quaternion.Euler(0,90,0),
        transform);

        selectedTile.piece.currentCoordinates = new Vector2Int(x, y);

        selectedTile.piece.SetPieceColor(material);

        if (material == pieceWhite)
        {
            whitePieces.Add(selectedTile.piece);
            selectedTile.piece.forwardDirection = 1;
            selectedTile.piece.interactable = true;
        }
        else
        {
            blackPieces.Add(selectedTile.piece);
            selectedTile.piece.forwardDirection = -1;
            selectedTile.piece.interactable = false;
        }
    }

    public void MovePieceTo(Piece selectedPiece, Tile tile)
    {
        // Make sure the previous Tile no longer owns the piece
        tiles[selectedPiece.currentCoordinates.x][selectedPiece.currentCoordinates.y].piece = null;

        // Move piece to new Tile
        tile.piece = selectedPiece;
        selectedPiece.transform.position = tile.transform.position + new Vector3(0, 0.5f, 0);
        selectedPiece.currentCoordinates = tile.coordinates;          
    }

    
    public void SetTileColour(Tile tile, Material mat)
    {
        tile.previousMat = tile.render.material;
        tile.render.material = mat;
    }

    public void ResetPieceMoveTileColours(Piece piece)
    {
        foreach (Vector2Int move in piece.moves)
        {
            tiles[move.x][move.y].render.material = tiles[move.x][move.y].previousMat;
        }
    }
}
