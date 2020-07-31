using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PENWriter PENWriter;
    public Stockfish stockfish;
    public Board board;
    public bool whiteTurn = true;
    public bool check;
    public bool paused;
    public King kingInCheck;
    public PromotionPanel promotionPanel;
    public ResultPanel resultPanel;
    public int moveCounter;
    public int movesWithoutCaptures;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        promotionPanel.gameObject.SetActive(false);
        resultPanel.gameObject.SetActive(false);
    }

    public void FindWhiteMoves()
    {
        foreach (Piece piece in board.whitePieces)
        {
            if (!piece.pinned)
            {
                piece.FindMoveSet();
            }
            else if (piece.pinned && piece.interactable)
            {
                piece.moves = piece.pinnedMoveList;
            }
        }
    }

    public void FindBlackMoves()
    {
        foreach (Piece piece in board.blackPieces)
        {
            if (!piece.pinned)
            {
                piece.FindMoveSet();
            }
            else if (piece.pinned && piece.interactable)
            {
                piece.moves = piece.pinnedMoveList;
            }
        }
    }

    public void FindAllPossibleMoves()
    {
        FindWhiteMoves();
        FindBlackMoves();

        // King movesets are found at the end because if one king is called first in FindXMoves without the opposite
        // side piece's to have been updated, the king will only take the previous moves into consideration.
        // PS: may need to find a better way to write this.
        King k = (King)board.whitePieces.Find(x => x is King);
        k.FindMoveSet();

        King kx = (King)board.blackPieces.Find(x => x is King);
        kx.FindMoveSet();
    }

    public void SwitchSides()
    {
        whiteTurn = (whiteTurn == true) ? false : true;

        moveCounter--;
        if (moveCounter % 2 == 0)
        {
            PENWriter.moveCount++;
            moveCounter = 2;
        }

        PENWriter.enPassantTile = "-";

        // Resets checks and defended statuses as well.
        if (kingInCheck != null)
        {
            kingInCheck.check = false;
            kingInCheck.checkDefended = false;
            kingInCheck = null;
        }

        foreach (Piece piece in board.whitePieces)
        {
            piece.interactable = (piece.interactable == true) ? false : true;
            piece.defended = false;
            piece.pinned = false;
        }

        foreach (Piece piece in board.blackPieces)
        {
            piece.interactable = (piece.interactable == true) ? false : true;
            piece.defended = false;
            piece.pinned = false;
        }

    }

    public void SetGameState(bool state)
    {
        // If state is true -> resume game, if state is false -> pause game
        paused = !state;
    }

    public void CheckKingCheck()
    {
        // Assign the correct list
        List<Piece> pieceList = 
        (kingInCheck.render.sharedMaterial == board.pieceBlack) ? board.blackPieces : board.whitePieces;

        for (int i = 0; i < pieceList.Count; i++)
        {
            List<Vector2Int> tempCoors = new List<Vector2Int>();

            foreach (Vector2Int move in kingInCheck.line)
            {
                // If any moves that intersect with the piece are found, add them to the list
                if (pieceList[i].moves.Any(x => x == move))
                {
                    kingInCheck.checkDefended = true;
                    tempCoors.Add(move);
                }
            }

            // Change the move list for the pieces that found any intersecting moves
            // If there's nothing, it will give an empty list, as any piece that cant move in the way shouldn't be able to move.
            if (!(pieceList[i] is King))
            {
                pieceList[i].moves = tempCoors;
            }
            
        }
    }

    public void CheckForCheckMate()
    {
        if (kingInCheck.check && kingInCheck.moves.Count == 0 && !kingInCheck.checkDefended)
        {
            string side = (kingInCheck.render.sharedMaterial == board.pieceWhite) ? "Black " : "White ";
            resultPanel.gameObject.SetActive(true);
            resultPanel.DisplayText(side + "wins by checkmate");
        }
        kingInCheck.line.Clear();
    }

    public void NextTurn()
    {
        if (!paused)
        {
            SwitchSides();
            FindAllPossibleMoves();
                
            if (kingInCheck != null)
            {
                CheckKingCheck();
                CheckForCheckMate();
            }

            stockfish.GetBestMove(PENWriter.WritePosition());
            CheckDraw();
        }
    }

    public void CheckDraw()
    {
        // If there are no more legal moves but king isn't in check..
        if (kingInCheck == null && (board.blackPieces.Where(x => x.moves.Count == 0).Count() == board.blackPieces.Count ||
                                    board.whitePieces.Where(x => x.moves.Count == 0).Count() == board.blackPieces.Count))
        {
            resultPanel.gameObject.SetActive(true);
            resultPanel.DisplayText("Stalemate");
        }

        // If there are 3 identical positions at any point in the game..
        List<string> posHistory = PENWriter.positionHistory;
        if (posHistory.Where(x => x.Equals(posHistory[posHistory.Count - 1])).Count() == 3)
        {
            resultPanel.gameObject.SetActive(true);
            resultPanel.DisplayText("Draw by repitition");
        }

        // If 50 complete moves without captures or pawn movement has happened..
        if (PENWriter.consecutivePieceMoves == 100 && movesWithoutCaptures == 100)
        {
            resultPanel.gameObject.SetActive(true);
            resultPanel.DisplayText("Fifty move draw");
        }

    }
}
