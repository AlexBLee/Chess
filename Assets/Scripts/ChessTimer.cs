using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Photon.Pun;

public class ChessTimer : MonoBehaviour
{
    private float whiteTimerValue;
    private float blackTimerValue;

    private int incTime;

    [SerializeField] private TextMeshProUGUI topTimerText;
    [SerializeField] private TextMeshProUGUI bottomTimerText;

    private void Start() 
    {
        if (PhotonNetwork.IsConnected)
        {
            whiteTimerValue = (int)PhotonNetwork.CurrentRoom.CustomProperties["time"] * 60;
            blackTimerValue = (int)PhotonNetwork.CurrentRoom.CustomProperties["time"] * 60;

            incTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["inc"];
        }
        else
        {
            if (PlayerPrefs.HasKey("TimePerSide"))
            {
                whiteTimerValue = PlayerPrefs.GetInt("TimePerSide") * 60;
                blackTimerValue = PlayerPrefs.GetInt("TimePerSide") * 60;
            }

            if (PlayerPrefs.HasKey("IncTime"))
            {
                incTime = PlayerPrefs.GetInt("IncTime");
            }
        }

        
        UpdateText(topTimerText, blackTimerValue);
        UpdateText(bottomTimerText, whiteTimerValue);
    }

    public void StartCountdown()
    {
        StopAllCoroutines();
        if (GameManager.instance.whiteTurn)
        {
            StartCoroutine(StartWhiteCountdown());

            if (GameManager.instance.moveCounter != 1)
            {
                blackTimerValue += incTime;
                UpdateText(!GameManager.whiteSide ? bottomTimerText : topTimerText, blackTimerValue);
            }
        }
        else
        {
            StartCoroutine(StartBlackCountdown());

            if (GameManager.instance.moveCounter != 1)
            {
                whiteTimerValue += incTime;
                UpdateText(GameManager.whiteSide ? bottomTimerText : topTimerText, whiteTimerValue);
            }
        }
    }

    // TODO: Combine these two functions into one..
    // Problem: can't pass parameters as reference types :(
    public IEnumerator StartBlackCountdown()
    {
        while (blackTimerValue > 0)
        {
            if (GameManager.instance.gameOver)
            {
                yield break;
            }

            yield return new WaitForSeconds(1.0f);

            blackTimerValue--;
            UpdateText(!GameManager.whiteSide ? bottomTimerText : topTimerText, blackTimerValue);
        }
        GameManager.instance.ActivateGameOver("White wins by timeout");
    }

    public IEnumerator StartWhiteCountdown()
    {
        while (whiteTimerValue > 0)
        {
            if (GameManager.instance.gameOver)
            {
                yield break;
            }
            
            yield return new WaitForSeconds(1.0f);

            whiteTimerValue--;
            UpdateText(GameManager.whiteSide ? bottomTimerText : topTimerText, whiteTimerValue);
        }
        GameManager.instance.ActivateGameOver("Black wins by timeout");
    }

    public void UpdateText(TextMeshProUGUI text, float timerValue)
    {
        TimeSpan time = TimeSpan.FromSeconds(timerValue);
        text.text = time.ToString(@"mm\:ss");
    }
}
