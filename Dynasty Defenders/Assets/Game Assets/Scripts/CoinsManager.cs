using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    [Min(0)] public int coins;
    public TMP_Text coinText;
    
    public static CoinsManager Instance;

    void Awake()
    {
        Instance = this;
        
        UpdateCoinText();
    }

    public void ChangeCoins(int amount)
    {
        coins += amount;
        if(coins <= 0) coins = 0;
        
        UpdateCoinText();
    }
    
    void UpdateCoinText() => coinText.text = coins + "";
}
