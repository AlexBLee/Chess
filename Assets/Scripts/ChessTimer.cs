using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ChessTimer : MonoBehaviour
{
    public float whiteTimerValue = 300;
    public float blackTimerValue = 300;

    public TextMeshProUGUI whiteTimerText;
    public TextMeshProUGUI blackTimerText;

    private void Start() 
    {
        UpdateText(whiteTimerText, whiteTimerValue);
        UpdateText(blackTimerText, blackTimerValue);
    }

    public void StartCountdown()
    {
        StopAllCoroutines();
        if (GameManager.instance.whiteTurn)
        {
            StartCoroutine(StartWhiteCountdown());
        }
        else
        {
            StartCoroutine(StartBlackCountdown());
        }
    }

    // TODO: Combine these two functions into one..
    // Problem: can't pass parameters as reference types :(
    public IEnumerator StartBlackCountdown()
    {
        while (blackTimerValue > 0)
        {
            blackTimerValue--;
            UpdateText(blackTimerText, blackTimerValue);

            yield return new WaitForSeconds(1.0f);
        }
    }

    public IEnumerator StartWhiteCountdown()
    {
        while (whiteTimerValue > 0)
        {
            whiteTimerValue--;
            UpdateText(whiteTimerText, whiteTimerValue);

            yield return new WaitForSeconds(1.0f);
        }
    }

    public void UpdateText(TextMeshProUGUI text, float timerValue)
    {
        TimeSpan time = TimeSpan.FromSeconds(timerValue);
        text.text = time.ToString(@"mm\:ss");
    }
}
