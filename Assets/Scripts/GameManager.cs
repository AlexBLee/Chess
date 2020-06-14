using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Board board;
    public bool playerTurn = true;

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
    }

    public void SwitchSides()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board.tiles[i][j].piece != null)
                {
                    if (board.tiles[i][j].piece.interactable)
                    {
                        board.tiles[i][j].piece.interactable = false;
                    }
                    else
                    {
                        board.tiles[i][j].piece.interactable = true;
                    }
                }
            }
        }
    }
}
