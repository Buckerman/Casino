using UnityEngine;
using TMPro;
using UnityEngine.UI; // Dodaj tê liniê, aby korzystaæ z komponentu Image

public class PayoutGenerator : MonoBehaviour
{
    public static PayoutGenerator Instance { get; private set; }

    [SerializeField] private GameObject payoutPrefab;
    [SerializeField] private TMP_Dropdown dropDownRisk;
    private RectTransform plinkoArea;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            plinkoArea = GetComponent<RectTransform>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GeneratePayout();
    }

    public void GeneratePayout()
    {
        // Usuñ poprzednie boxy
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int numRows = PegGenerator.Instance.NumRows;
        float areaWidth = plinkoArea.rect.width;
        float areaHeight = plinkoArea.rect.height;

        float horizontalSpacing = areaWidth / (numRows + 1);
        float verticalSpacing = Mathf.Sqrt(3) / 2 * horizontalSpacing;

        float topY = areaHeight / 2;
        float lastRowY = topY - ((numRows - 1) * verticalSpacing);
        float payoutY = lastRowY - (verticalSpacing * 0.6f);

        float payoutScale = 1.0f * (8f / numRows);

        for (int i = 0; i < numRows + 1; i++)
        {
            float xOffset = horizontalSpacing / 2;
            Vector2 payoutPosition = new Vector2(
                (-areaWidth / 2) + (i * horizontalSpacing) + xOffset,
                payoutY
            );

            GameObject payoutBox = Instantiate(payoutPrefab, transform);
            payoutBox.transform.SetParent(plinkoArea, false);
            payoutBox.GetComponent<RectTransform>().anchoredPosition = payoutPosition;
            payoutBox.transform.localScale = new Vector3(payoutScale, payoutScale, payoutScale);

            Color payoutColor = GetPayoutColor(i, numRows);
            payoutBox.GetComponent<Image>().color = payoutColor;
        }
    }

    private Color GetPayoutColor(int index, int total)
    {
        Color startColor, endColor;
        float middleIndex = total / 2f;

        if (total % 2 == 0)
        {
            startColor = Color.yellow;
            endColor = Color.red;
        }
        else
        {
            startColor = new Color(1.0f, 0.6f, 0.0f);
            endColor = Color.red;

            // Make sure **two** middle payout boxes start with `startColor`
            if (index == Mathf.FloorToInt(middleIndex) || index == Mathf.CeilToInt(middleIndex))
                return startColor;
        }

        // Obliczenie odleg³oœci od œrodka (normalizacja do zakresu 0-1)
        float distanceFromCenter = Mathf.Abs(index - middleIndex) / middleIndex;

        // Interpolacja kolorów bazuj¹c na odleg³oœci od œrodka
        return Color.Lerp(startColor, endColor, distanceFromCenter);
    }
}
