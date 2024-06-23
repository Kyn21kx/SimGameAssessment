using UnityEngine;
using System.Collections;

public class ScoringManager
{
    public static int s_Money {
        get => s_money;
        set
        {
            s_money = value;

        }
    }

    private static int s_money;

    private static readonly ScoringManager s_instance = new();

}

