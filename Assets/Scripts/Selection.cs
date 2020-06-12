using System.Collections;
using System.Collections.Generic;
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

                    if (selectedTile.piece != null)
                    {
                        Debug.Log("tryin to destroy");
                        Destroy(selectedTile.piece.gameObject);
                    }

                    board.tiles[selectedPiece.currentCoordinates.x][selectedPiece.currentCoordinates.y].piece = null;

                    selectedTile.piece = selectedPiece;
                    selectedPiece.transform.position = selectedTile.transform.position + new Vector3(0, 0.5f, 0);
                    selectedPiece.currentCoordinates = selectedTile.coordinates;

                    foreach (Vector2Int move in selectedPiece.moves)
                    {
                        board.tiles[move.x][move.y].render.material = board.tiles[move.x][move.y].previousMat;
                    }

                    selecting = false;
                }
                else
                {
                    selectedPiece = hit.transform.GetComponent<Tile>().piece;
                    selectedPiece.FindMoveSet();
                    selecting = true;
                }

            }
            else
            {
                Debug.Log("Did not Hit");
            }
        }
        
    }
}
