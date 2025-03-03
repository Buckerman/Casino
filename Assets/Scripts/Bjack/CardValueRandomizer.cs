using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardValueRandomizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardText;
    private static readonly Dictionary<string, int> _cardValue = new Dictionary<string, int>
    {
        {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6},
        {"7", 7}, {"8", 8}, {"9", 9}, {"10", 10},
        {"J", 10}, {"Q", 10}, {"K", 10}, {"A", 11}
    };
    private static readonly List<string> keys = new List<string>(_cardValue.Keys);
    private void Start()
    {
        RandomValue();
    }
    public void RandomValue()
    {
        if (cardText == null) return;

        string randomCard = keys[Random.Range(0, keys.Count)];
        cardText.text = randomCard;
    }
    public int CalculateHandValue(List<string> hand)
    {
        int total = 0;
        int aceCount = 0;

        foreach (string card in hand)
        {
            if (_cardValue.TryGetValue(card, out int value))
            {
                total += value;
                if (card == "A") aceCount++;
            }
        }
        // Convert Aces from 11 to 1 if total exceeds 21
        while (total > 21 && aceCount > 0)
        {
            total -= 10;
            aceCount--;
        }

        return total;
    }
}