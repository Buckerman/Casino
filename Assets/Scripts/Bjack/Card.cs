using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _suitSpriteRenderer;
    [SerializeField] private TextMeshProUGUI _cardText;
    public int Value { get; private set; }
    public void Initialize(int value, string cardText, Sprite suit)
    {
        Value = value;
        _cardText.text = cardText;
        _suitSpriteRenderer.sprite = suit;
    }
}
