using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public Board board;
    public Piece selectedPiece;
    public bool selecting;


    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (selecting)
                {
                    Tile selectedTile = hit.transform.GetComponent<Tile>();

                    if (selectedPiece.moves.Any(move => move == selectedTile.coordinates))
                    {
                        if (selectedTile.piece != null)
                        {
                            Destroy(selectedTile.piece.gameObject);
                        }

                        MovePieceTo(selectedTile);
                        GameManager.instance.SwitchSides();
                        GameManager.instance.FindAllPossibleMoves();
                    }
                    else
                    {
                        Debug.Log(selectedTile.coordinates + "not in " + selectedPiece.name + " moves!"); 
                    }

                    foreach (Vector2Int move in selectedPiece.moves)
                    {
                        board.tiles[move.x][move.y].render.material = board.tiles[move.x][move.y].previousMat;
                    }

                    selecting = false;
                }
                else
                {
                    Piece piece = hit.transform.GetComponent<Tile>().piece;
                    
                    if (piece != null && piece.interactable)
                    {
                        selectedPiece = hit.transform.GetComponent<Tile>().piece;
                        selecting = true;
                    }
                }
            }
        }
    }

    public void MovePieceTo(Tile selectedTile)
    {
        // Make sure the previous Tile no longer owns the piece
        board.tiles[selectedPiece.currentCoordinates.x][selectedPiece.currentCoordinates.y].piece = null;

        // Move piece to new Tile
        selectedTile.piece = selectedPiece;
        selectedPiece.transform.position = selectedTile.transform.position + new Vector3(0, 0.5f, 0);
        selectedPiece.currentCoordinates = selectedTile.coordinates;            
    }
}
