using UnityEngine;
using System.Collections;
using TMPro;

public class ScoringManager
{
    private const string SCORE_TEXT_TAG = "Money";

    public static int s_Money {
        get => s_money;
        set
        {
            if (s_displayMoneyText == null)
            {
                GameObject textObj = GameObject.FindGameObjectWithTag(SCORE_TEXT_TAG);
                s_displayMoneyText = textObj.GetComponent<TextMeshProUGUI>();
            }
            s_money = value;
            s_displayMoneyText.text = $"Money: {value}.00";
        }
    }

    private static int s_money = 0;

    private static TextMeshProUGUI s_displayMoneyText;

}

