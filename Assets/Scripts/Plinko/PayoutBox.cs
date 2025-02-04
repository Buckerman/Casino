using DG.Tweening;
using TMPro;
using UnityEngine;

public class PayoutBox : MonoBehaviour
{
    bool animFinished = true;
    public void Score()
    {
        // Parse both value of box and the bet amount into float numbers
        TextMeshProUGUI textComponent = GetComponentInChildren<TextMeshProUGUI>();
        if (float.TryParse(textComponent.text.Replace("x", ""), out float boxValue) &&
            float.TryParse(GameManager.Instance.BetAmountText.text, out float betAmount))
        {
            Observer.Instance.Notify(EventName.AddMoney, betAmount * boxValue);
            Observer.Instance.Notify(EventName.AddHistory, boxValue);
        }
        PayoutAnimation();
    }
    private void PayoutAnimation()
    {
        if (!animFinished)
        {
            return;
        }

        animFinished = false;

        transform.DOMoveY(transform.position.y - 0.3f, 0.2f)
            .OnComplete(() =>
            {
                transform.DOMoveY(transform.position.y + 0.3f, 0.2f)
                    .OnComplete(() =>
                    {
                        animFinished = true;
                    });
            });
    }
}