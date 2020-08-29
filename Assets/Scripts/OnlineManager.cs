using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class OnlineManager : MonoBehaviourPunCallbacks
{
    public static OnlineManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    #region Photon Callbacks



    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
        Debug.Log("Player left room");
    }

    public override void OnPlayerEnteredRoom(Player other)
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
        
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if youre the player connecting.
    }

    public override void OnJoinedRoom()
    {
        GameManager.whiteSide = !(bool)PhotonNetwork.CurrentRoom.CustomProperties["side"];

        GameManager.instance.InitializeHUD();
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects.
    }

    #endregion
    
    

    
}
