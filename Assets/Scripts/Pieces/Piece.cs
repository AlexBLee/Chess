using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // to tell between white and black
    public bool player;
    public Renderer render;
    public List<Vector2> moves;
    public Vector2 currentCoordinates;

    public void SetPieceColor(Material color)
    {
        render.material = color;
    }

    public virtual void FindLegalMoves()
    {
    }
}
