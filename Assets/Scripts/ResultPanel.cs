using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void PlayAgain()
    {

    }

    void QuitToMenu()
    {

    }
}
