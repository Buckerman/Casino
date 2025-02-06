using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Histogram : MonoBehaviour
{
    [SerializeField] private GameObject payoutBoxPrefab;
    [SerializeField] private RectTransform histogramArea;
    private readonly Queue<GameObject> activeBoxes = new Queue<GameObject>();
    private int MaxBoxes;
    private float BoxSpacing;
    private const float AnimationDuration = 0.3f;

    private void Start()
    {
        Observer.Instance.AddObserver(EventName.AddHistory, AddHistory);

        BoxSpacing = (int)(histogramArea.rect.width);
        MaxBoxes = Mathf.FloorToInt(histogramArea.rect.height / BoxSpacing);
    }

    private void AddHistory(object data)
    {
        PayoutData payoutData = (PayoutData)data;

        if (activeBoxes.Count >= MaxBoxes + 1)
        {
            RemoveOldestBox();
        }

        AnimateExistingBoxesUp();

        GameObject payoutObject = ObjectPooling.Instance.GetObject(payoutBoxPrefab);
        payoutObject.transform.SetParent(histogramArea, false);
        payoutObject.GetComponent<Image>().color = payoutData.color;
        payoutObject.GetComponentInChildren<TextMeshProUGUI>().text = payoutData.text + "x";

        PayoutBox payoutBox = payoutObject.GetComponent<PayoutBox>();
        payoutBox.Initialize(histogramArea);

        RectTransform rectTransform = payoutObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -histogramArea.rect.height / 2 - BoxSpacing / 2);
        rectTransform.DOAnchorPosY(GetBottomPosition(), AnimationDuration)
            .SetEase(Ease.OutQuad);

        activeBoxes.Enqueue(payoutObject);
    }

    //Using Sequence
    #region
    //private void AnimateExistingBoxesUp()
    //{
    //    Sequence sequence = DOTween.Sequence();
    //    int index = activeBoxes.Count;
    //    foreach (GameObject box in activeBoxes)
    //    {
    //        RectTransform rectTransform = box.GetComponent<RectTransform>();
    //        float targetY = GetPositionForIndex(index);

    //        sequence.Join(rectTransform.DOAnchorPosY(targetY, AnimationDuration)
    //            .SetEase(Ease.OutQuad));

    //        index--;
    //    }
    //]
    #endregion

    private void AnimateExistingBoxesUp()
    {
        int index = activeBoxes.Count;

        foreach (GameObject box in activeBoxes)
        {
            RectTransform rectTransform = box.GetComponent<RectTransform>();
            float targetY = GetPositionForIndex(index);

            rectTransform.DOComplete();
            rectTransform.DOAnchorPosY(targetY, AnimationDuration)
                .SetEase(Ease.OutQuad);

            index--;
        }
    }

    private void RemoveOldestBox()
    {
        if (activeBoxes.Count > 0)
        {
            GameObject oldestBox = activeBoxes.Dequeue();
            RectTransform rectTransform = oldestBox.GetComponent<RectTransform>();

            rectTransform.DOComplete(); // Prevent animation stacking
            rectTransform.DOAnchorPosY(histogramArea.rect.height / 2 + BoxSpacing * 1.5f, AnimationDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() => oldestBox.SetActive(false));
        }
    }

    private float GetPositionForIndex(int index)
    {
        return GetBottomPosition() + (index * BoxSpacing);
    }

    private float GetBottomPosition()
    {
        return -histogramArea.rect.height / 2 + BoxSpacing / 2;
    }
}