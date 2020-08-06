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
            TimeSpan time = TimeSpan.FromSeconds(blackTimerValue);
            UpdateText(blackTimerText, time.ToString(@"mm\:ss"));

            yield return new WaitForSeconds(1.0f);
        }
    }

    public IEnumerator StartWhiteCountdown()
    {
        while (whiteTimerValue > 0)
        {
            whiteTimerValue--;
            TimeSpan time = TimeSpan.FromSeconds(whiteTimerValue);
            UpdateText(whiteTimerText, time.ToString(@"mm\:ss"));
            
            yield return new WaitForSeconds(1.0f);
        }
    }

    public void UpdateText(TextMeshProUGUI text, string value)
    {
        text.text = value;
    }
}
