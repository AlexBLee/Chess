using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class OnlineManager : MonoBehaviourPunCallbacks
{
    public static OnlineManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Setup();
    }

    private void Setup()
    {
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
    }

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
        base.OnLeftRoom();
        Debug.Log("Player left room");
    }

    
    #endregion
    
}
