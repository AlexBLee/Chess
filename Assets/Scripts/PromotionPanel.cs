using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromotionPanel : MonoBehaviour
{
    [SerializeField]
    private List<Button> buttonList;
    public int number;
    public bool buttonPressed = false;
    
    public void ChangePromotionNumber(int promotionNumber)
    {
        number = promotionNumber;
        buttonPressed = true;
    }
}
