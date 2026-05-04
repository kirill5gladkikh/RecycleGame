using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    void Start()
    {
        coinsText.text = "Coins: " + CoinManager.coins;
    }
}