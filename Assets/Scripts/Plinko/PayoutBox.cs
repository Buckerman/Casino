using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PayoutData
{
    public Color color;
    public string text;

    public PayoutData(Color color, string text)
    {
        this.color = color;
        this.text = text;
    }
}
public class PayoutBox : MonoBehaviour
{
    private RectTransform rectTransform;
    bool animFinished = true;

    public void Initialize(RectTransform histogramArea)
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(histogramArea.rect.width, histogramArea.rect.width);

        transform.SetParent(histogramArea, false);
    }
    public void Score()
    {
        // Parse both value of box and the bet amount into float numbers
        TextMeshProUGUI textComponent = GetComponentInChildren<TextMeshProUGUI>();
        if (float.TryParse(textComponent.text.Replace("x", ""), out float boxValue) &&
            float.TryParse(GameManager.Instance.BetAmountText.text, out float betAmount))
        {
            Observer.Instance.Notify(EventName.AddMoney, betAmount * boxValue);
            Observer.Instance.Notify(EventName.AddHistory, new PayoutData(GetComponent<Image>().color, boxValue.ToString()));
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