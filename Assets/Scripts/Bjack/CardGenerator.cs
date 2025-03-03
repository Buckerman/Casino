using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Sprite _heart, _diamond, _club, _spade;

    private static readonly Dictionary<string, int> _cardValue = new Dictionary<string, int>
    {
        {"2", 2}, {"3", 3}, {"4", 4}, {"5", 5}, {"6", 6},
        {"7", 7}, {"8", 8}, {"9", 9}, {"10", 10},
        {"J", 10}, {"Q", 10}, {"K", 10}, {"A", 11}
    };
    private static readonly List<string> _keys = new List<string>(_cardValue.Keys);

    private List<Sprite> _suits;
    private void Start()
    {
        _suits = new List<Sprite> { _heart, _diamond, _club, _spade };
    }
    public void GenerateCard()
    {
        GameObject cardObject = ObjectPooling.Instance.GetObject(_cardPrefab);
        Card card = cardObject.GetComponent<Card>();

        string cardText = RandomizeValue();
        Sprite suit = RandomizeSuit();

        card.Initialize(_cardValue[cardText], cardText, suit);
    }
    private Sprite RandomizeSuit() => _suits[Random.Range(0, _suits.Count)];
    private string RandomizeValue() => _keys[Random.Range(0, _keys.Count)];

    public int CalculateHandValue(List<Card> hand)
    {
        int total = 0;
        int aceCount = 0;

        foreach (Card card in hand)
        {
            total += card.Value;
            if (card.Value == 11) aceCount++;

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
