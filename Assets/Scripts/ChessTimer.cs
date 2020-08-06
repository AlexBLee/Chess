using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTimer : MonoBehaviour
{
    public float whiteTimerValue = 300;
    public float blackTimerValue = 300;

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
            Debug.Log("Countdown: " + blackTimerValue);
            yield return new WaitForSeconds(1.0f);
            blackTimerValue--;
        }
    }

    public IEnumerator StartWhiteCountdown()
    {
        while (whiteTimerValue > 0)
        {
            Debug.Log("Countdown: " + whiteTimerValue);
            yield return new WaitForSeconds(1.0f);
            whiteTimerValue--;
        }
    }
}
