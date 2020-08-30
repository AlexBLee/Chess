using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class OnlineManager : MonoBehaviourPunCallbacks
{
    public static OnlineManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Setup();
    }

    public void Setup()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            GameManager.whiteSide = !(bool)PhotonNetwork.CurrentRoom.CustomProperties["side"];
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.whiteSide)
            {
                foreach (Piece piece in GameManager.instance.board.blackPieces)
                {
                    piece.photonView.TransferOwnership(2);
                }
            }
            else
            {
                foreach (Piece piece in GameManager.instance.board.whitePieces)
                {
                    piece.photonView.TransferOwnership(2);
                }
            }
        }

        GameManager.instance.InitializeHUD();
    }

    #region Photon Callbacks

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Player left room");
    }
    

    #endregion
    
    

    
}
