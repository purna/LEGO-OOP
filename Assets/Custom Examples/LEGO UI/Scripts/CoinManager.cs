using TMPro;  // Required for TextMesh Pro
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    // Reference to the TextMesh Pro text component
    public TMP_Text coinText;

    // Variable to hold the number of coins collected
    private int coinCount = 0;

    // Method to increment the coin count and update the text
    public void CollectCoin()
    {
        coinCount++;  // Increment coin count
        UpdateCoinText();  // Update the text to show the new coin count
    }

    // Method to update the coin text in the UI
    private void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "Coins Collected: " + coinCount;  // Update the UI text
        }
        else
        {
            Debug.LogError("Coin Text is not assigned in the inspector!");
        }
    }
}
