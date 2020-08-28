using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;

public class Board : MonoBehaviour
{
    public Tile tile;
    public List<List<Tile>> tiles;

    // List of tiles at the end of each side - needed for turning off castle block status on tiles
    public List<Tile> endTiles = new List<Tile>();

    // Holds the type of pieces that can be spawned
    public List<Piece> pieces;

    // To keep a list of current pieces for each side on the board.
    public List<Piece> whitePieces;
    public List<Piece> blackPieces;

    private const float TILE_OFFSET = 1.0f;

    [SerializeField] private const int boardWidth = 8;
    [SerializeField] private const int boardHeight = 8;

    public enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King };
    public PieceType pieceType;

    public Material boardWhite;
    public Material boardBlack;
    public Material pieceWhite;
    public Material pieceBlack;
    public Material pieceAttack;

    public Material availableMoveColour;

    public PhotonView photonView;

    public bool initialInstantiation = true;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        InitalizeBoard();
        ColourAndNameTiles();
        SpawnPieces();

        // For some reason, when the client player joins, the piece lists are initially empty which
        // results in a NullReference error.
        // So, if you're online, the piece will find their own moves for the start of the game.
        // But if you're offline, it will just get the GameManager to find the moves.
        if (!PhotonNetwork.IsConnected)
        {
            GameManager.instance.FindAllPossibleMoves();
        }
    }

    private void InitalizeBoard()
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

        
    }

    private void ColourAndNameTiles()
    {
        // colour and naming tiles
        for (int i = 0; i < boardWidth; i++)
        {
            for (int j = 0; j < boardHeight; j++)
            {
                int index = i + j * 8;
                char nextChar = (char)('a' + i);
                Tile tile = tiles[i][j];

                tile.name = nextChar + (j+1).ToString();
                tile.coordinates = new Vector2Int(i, j);

                if (j == 7 || j == 0)
                {
                    endTiles.Add(tile);
                }

                if ((i+j) % 2 == 0)
                {
                    tile.render.material = boardBlack;
                    tile.defaultColour = boardBlack;
                }
                else
                {
                    tile.render.material = boardWhite;
                    tile.defaultColour = boardWhite;
                }
            }
        }
    }

    private void SpawnPieces()
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

        initialInstantiation = false;

    }

    public void PlacePiecesAt(int x, int y, PieceType pieceType, Material material)
    {
        Tile selectedTile = tiles[x][y];

        selectedTile.piece = pieces[(int)pieceType];

        Quaternion correctRotation = (material == pieceWhite) ? Quaternion.Euler(0,270,0) : Quaternion.Euler(0,90,0);

        // If singleplayer..
        if (!PhotonNetwork.IsConnected)
        {
            selectedTile.piece = 
            Instantiate(selectedTile.piece, 
            tiles[x][y].transform.position + new Vector3(0, 0.5f, 0),
            correctRotation,
            transform);

            selectedTile.piece.location = new Vector2Int(x, y);

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
        else  // If online..
        {
            if (!PhotonNetwork.IsMasterClient && initialInstantiation) { return; }

            // Because you can't send material through the object data, have to use a bool
            // to determine the color the piece will be and then explicitly set the material 
            // on its instatiate based on the bool.
            bool white = (material == pieceWhite) ? true : false;

            object[] instantiateData = { x, y, white }; 

            PhotonNetwork.InstantiateRoomObject(selectedTile.piece.name + "M", 
            tiles[x][y].transform.position + new Vector3(0, 0.5f, 0),
            correctRotation,
            0, instantiateData);
        }
    }
    
    public void SetTileColour(Tile tile, Material mat)
    {
        tile.render.material = mat;
    }

    public void ResetPieceMoveTileColours(Piece piece)
    {
        foreach (Vector2Int move in piece.moves)
        {
            tiles[move.x][move.y].render.material = tiles[move.x][move.y].defaultColour;
        }
    }

    public void DestroyPieceAt(Piece selectedPiece, Tile selectedTile)
    {
        // Destroy the piece at that tile
        if (selectedTile.piece != null && selectedPiece.render.sharedMaterial != selectedTile.piece.render.sharedMaterial) 
        {
            AudioManager.Instance.Play("Capture");

            GameManager.instance.movesWithoutCaptures = 0;

            // TODO: clear piece from board function
            if (selectedTile.piece.render.sharedMaterial == pieceBlack)
            {
                blackPieces.Remove(selectedTile.piece);
            }
            else
            {
                whitePieces.Remove(selectedTile.piece);
            }


            Destroy(selectedTile.piece.gameObject); 
        }
        else
        {
            AudioManager.Instance.Play("Move");
        }
    }

    public void DestroyPieceAt(Tile selectedTile)
    {
        // Destroy the piece at that tile
        if (selectedTile.piece != null)
        {
            GameManager.instance.movesWithoutCaptures = 0;

            // TODO: clear piece from board function
            if (selectedTile.piece.render.sharedMaterial == pieceBlack)
            {
                blackPieces.Remove(selectedTile.piece);
            }
            else
            {
                whitePieces.Remove(selectedTile.piece);
            }

            Destroy(selectedTile.piece.gameObject); 
        }
    }

    public bool IsInBoard(Vector2Int tile)
    {
        return tile.x >= 0 && tile.y >= 0 && tile.x <= 7 && tile.y <= 7;
    }

    public Tile GetTile(Vector2Int coordinates)
    {
        return IsInBoard(coordinates) ? tiles[coordinates.x][coordinates.y] : null;
    }


    #region PhotonNetwork Calls

    [PunRPC]
    public void DestroyPieceAt(Vector2 clickedTileLocation, Vector2 currentPieceLocation)
    {
        // Need some kind of comparison to tell which side is which
        Tile selectedTile = tiles[(int)clickedTileLocation.x][(int)clickedTileLocation.y];
        Tile currentTile = tiles[(int)currentPieceLocation.x][(int)currentPieceLocation.y];

        // Destroy the piece at that tile
        if (selectedTile.piece != null && currentTile.piece != null) 
        {
            if (selectedTile.piece.render.sharedMaterial != currentTile.piece.render.sharedMaterial)
            {
                AudioManager.Instance.Play("Capture");

                GameManager.instance.movesWithoutCaptures = 0;

                // TODO: clear piece from board function
                if (selectedTile.piece.render.sharedMaterial == pieceBlack)
                {
                    blackPieces.Remove(selectedTile.piece);
                }
                else
                {
                    whitePieces.Remove(selectedTile.piece);
                }


                Destroy(selectedTile.piece.gameObject); 
            }
        }
        else
        {
            AudioManager.Instance.Play("Move");
        }
    }

    [PunRPC]
    public void DestroyPieceAt(Vector2 location)
    {
        Tile tile = tiles[(int)location.x][(int)location.y];

        // Destroy the piece at that tile
        if (tile.piece != null)
        {
            GameManager.instance.movesWithoutCaptures = 0;

            // TODO: clear piece from board function
            if (tile.piece.render.sharedMaterial == pieceBlack)
            {
                blackPieces.Remove(tile.piece);
            }
            else
            {
                whitePieces.Remove(tile.piece);
            }

            Destroy(tile.piece.gameObject); 
        }
    }
    #endregion
}
