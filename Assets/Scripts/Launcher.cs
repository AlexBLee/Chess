using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject progressLabel;

    bool isConnecting;

    private void Awake()
    {
        
    }

}
