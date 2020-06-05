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

                    selectedPiece.transform.position = selectedTile.transform.position + new Vector3(0, 0.5f, 0);
                    selectedPiece.currentCoordinates = selectedTile.coordinates;

                    selectedTile.piece = selectedPiece;
                    selectedPiece = null;
                    
                    selecting = false;
                }
                else
                {
                    selectedPiece = hit.transform.GetComponent<Tile>().piece;
                    selectedPiece.FindLegalMoves();
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
