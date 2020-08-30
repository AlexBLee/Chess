using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class NameDisplay : MonoBehaviour
{
    public TextMeshProUGUI topNameText;
    public TextMeshProUGUI bottomNameText;

    private void Start() 
    {
        if (PhotonNetwork.PlayerList[0] != null)
        {
            UpdateText(bottomNameText, PhotonNetwork.NickName);
        }

    }

    public void UpdateText(TextMeshProUGUI text, string textValue)
    {
        text.text = textValue;
    }

}
