using DG.Tweening;
using TMPro;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardText;
    private bool isFlipped = false;

    private void Flip()
    {
        isFlipped = !isFlipped;

        float targetRotation = isFlipped ? 180f : 0f;

        transform.DORotate(new Vector3(0, targetRotation, 0), 0.25f);
        cardText.gameObject.SetActive(!isFlipped);
    }
}
