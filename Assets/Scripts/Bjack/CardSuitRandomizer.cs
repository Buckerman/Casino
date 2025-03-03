using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSuitRandomizer : MonoBehaviour
{
    [SerializeField] private Sprite _heart;
    [SerializeField] private Sprite _diamond;
    [SerializeField] private Sprite _club;
    [SerializeField] private Sprite _spade;
    [SerializeField] private SpriteRenderer _suitSpriteRenderer;
    private void Start()
    {
        RandomizeSuit();
    }
    public void RandomizeSuit()
    {
        Sprite[] suits = { _heart, _diamond, _club, _spade };
        _suitSpriteRenderer.sprite = suits[Random.Range(0, suits.Length)];
    }
}
