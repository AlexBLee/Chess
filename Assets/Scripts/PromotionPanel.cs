using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotionPanel : MonoBehaviour
{
    private List<Button> buttonList;
    public int number;
    public bool buttonPressed = false;
    
    private void Start() 
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            int x = i + 1;
            buttonList[i].onClick.AddListener(() => { number = x; buttonPressed = true; });
        }
    }
}
