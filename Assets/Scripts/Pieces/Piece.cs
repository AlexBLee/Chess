using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    // to tell between white and black
    public bool player;
    public Renderer render;
    

    public void SetPieceColor(Material color)
    {
        render.material = color;
    }

    public virtual void FindLegalMoves()
    {
        
    }
}
