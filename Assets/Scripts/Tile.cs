using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject square;
    public Renderer render;
    public Piece piece;
    public Vector2Int coordinates;
    public Material defaultColour;

    // Mark the tiles that could possibly block the king from castling
    public bool possibleCastleBlocked;
}
