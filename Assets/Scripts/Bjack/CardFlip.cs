using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardText;
    private void Flip()
    {
        transform.DORotate(new Vector3(0, 180f, 0), 0.25f);
        cardText.gameObject.SetActive(false);
    }
}
