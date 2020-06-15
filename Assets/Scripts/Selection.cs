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

                    // If the clicked tile is possible for the piece to move to, move there
                    if (selectedPiece.moves.Any(move => move == selectedTile.coordinates))
                    {
                        // Destroy the piece at that tile
                        if (selectedTile.piece != null)
                        {
                            Destroy(selectedTile.piece.gameObject);
                        }

                        // Colour the board back to normal
                        foreach (Vector2Int move in selectedPiece.moves)
                        {
                            board.tiles[move.x][move.y].render.material = board.tiles[move.x][move.y].previousMat;
                        }

                        board.MovePieceTo(selectedPiece, selectedTile);
                        GameManager.instance.SwitchSides();
                        GameManager.instance.FindAllPossibleMoves();
                    }
                    else
                    {
                        Debug.Log(selectedTile.coordinates + " not in " + selectedPiece.name + " moves!"); 
                    }

                    selecting = false;
                }
                else
                {
                    selectedPiece = hit.transform.GetComponent<Tile>().piece;
                    
                    // Make sure the player is clicking the right piece
                    if (selectedPiece != null && selectedPiece.interactable)
                    {
                        // Loop through each possible move that the piece can make
                        foreach (Vector2Int move in selectedPiece.moves)
                        {
                            Piece piece = board.tiles[move.x][move.y].piece;

                            // If the piece can attack a piece, then turn it red
                            if (piece != null && (piece.interactable != selectedPiece.interactable))
                            {
                                board.tiles[move.x][move.y].SetTileColour(board.pieceAttack);
                            }
                            // Otherwise colour it normally
                            else
                            {
                                board.tiles[move.x][move.y].SetTileColour(board.pieceWhite);
                            }
                        }

                        selecting = true;
                    }
                }
            }
        }
    }
}
