using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Debug.Log("Player left room");
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if youre the player connecting.
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects.
    }

    #endregion
    
    

    
}
