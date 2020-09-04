using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class ResultPanel : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public Button playAgainButton;
    public Button quitButton;

    void Start()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
        quitButton.onClick.AddListener(QuitToMenu);
    }

    public void DisplayText(string text)
    {
        resultText.text = text;
    }

    private void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void QuitToMenu()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
        }

        SceneManager.LoadScene("Menu");
    }
}
