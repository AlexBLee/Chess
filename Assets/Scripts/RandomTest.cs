using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomTest : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable _customProperties = new ExitGames.Client.Photon.Hashtable();

    // Start is called before the first frame update
    void Awake()
    {
        SetCustomNumber();
    }

    private void SetCustomNumber()
    {
        System.Random rnd = new System.Random();
        int re = rnd.Next(0,99);
        Debug.Log(re);
        _customProperties["number"] = re;
        PhotonNetwork.LocalPlayer.CustomProperties = _customProperties;
    }
}
