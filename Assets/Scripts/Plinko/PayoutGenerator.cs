using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic; // Dodaj tê liniê, aby korzystaæ z komponentu Image

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

            float multiplier = GetPayoutMultiplier(i, numRows);
            payoutBox.GetComponentInChildren<TextMeshProUGUI>().text = multiplier % 1 == 0 ? multiplier.ToString("F0") + "x" : multiplier.ToString("F1") + "x";
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

    private float GetPayoutMultiplier(int index, int numRows)
    {
        int riskLevel = dropDownRisk.value; // 0 = Low, 1 = Medium, 2 = High

        // Predefined multipliers grouped by risk level and row count.
        var multipliers = new Dictionary<(int riskLevel, int numRows), float[]>
    {
        {(0, 8), new float[] {5.6f, 2.1f, 1.1f, 1.0f, 0.5f, 1.0f, 1.1f, 2.1f, 5.6f}},
        {(0, 9), new float[] {5.6f, 2.0f, 1.6f, 1.0f, 0.7f, 0.7f, 1.0f, 1.6f, 2.0f, 5.6f}},
        {(0, 10), new float[] {8.9f, 3.0f, 1.4f, 1.1f, 1.0f, 0.5f, 1.0f, 1.1f, 1.4f, 3.0f, 8.9f}},
        {(0, 11), new float[] {8.4f, 3.0f, 1.9f, 1.3f, 1.0f, 0.7f, 0.7f, 1.0f, 1.3f, 1.9f, 3.0f, 8.4f}},
        {(0, 12), new float[] {10.0f, 3.0f, 1.6f, 1.4f, 1.1f, 1.0f, 0.5f, 1.0f, 1.1f, 1.4f, 1.6f, 3.0f, 10.0f}},
        {(0, 13), new float[] {8.1f, 4.0f, 3.0f, 1.9f, 1.2f, 0.9f, 0.7f, 0.7f, 0.9f, 1.2f, 1.9f, 3.0f, 4.0f, 8.1f}},
        {(0, 14), new float[] {7.1f, 4.0f, 1.9f, 1.4f, 1.3f, 1.1f, 1.0f, 0.5f, 1.0f, 1.1f, 1.3f, 1.4f, 1.9f, 4.0f, 7.1f}},
        {(0, 15), new float[] {15.0f, 8.0f, 3.0f, 2.0f, 1.5f, 1.1f, 1.0f, 0.7f, 0.7f, 1.0f, 1.1f, 1.5f, 2.0f, 3.0f, 8.0f, 15.0f}},
        {(0, 16), new float[] {16.0f, 9.0f, 2.0f, 1.4f, 1.4f, 1.2f, 1.1f, 1.0f, 0.5f, 1.0f, 1.1f, 1.2f, 1.4f, 1.4f, 2.0f, 9.0f, 16.0f}},

        {(1, 8), new float[] {13.0f, 3.0f, 1.3f, 0.7f, 0.4f, 0.7f, 1.3f, 3.0f, 13.0f}},
        {(1, 9), new float[] {18.0f, 4.0f, 1.7f, 0.9f, 0.5f, 0.5f, 0.9f, 1.7f, 4.0f, 18.0f}},
        {(1, 10), new float[] {22.0f, 5.0f, 2.0f, 1.4f, 0.6f, 0.4f, 0.6f, 1.4f, 2.0f, 5.0f, 22.0f}},
        {(1, 11), new float[] {24.0f, 6.0f, 3.0f, 1.8f, 0.7f, 0.5f, 0.5f, 0.7f, 1.8f, 3.0f, 6.0f, 24.0f}},
        {(1, 12), new float[] {33.0f, 11.0f, 4.0f, 2.0f, 1.1f, 0.6f, 0.3f, 0.6f, 1.1f, 2.0f, 4.0f, 11.0f, 33.0f}},
        {(1, 13), new float[] {43.0f, 13.0f, 6.0f, 3.0f, 1.3f, 0.7f, 0.4f, 0.4f, 0.7f, 1.3f, 3.0f, 6.0f, 13.0f, 43.0f}},
        {(1, 14), new float[] {58.0f, 15.0f, 7.0f, 4.0f, 1.9f, 1.0f, 0.5f, 0.2f, 0.5f, 1.0f, 1.9f, 4.0f, 7.0f, 15.0f, 58.0f}},
        {(1, 15), new float[] {88.0f, 18.0f, 11.0f, 5.0f, 3.0f, 1.3f, 0.5f, 0.3f, 0.3f, 0.5f, 1.3f, 3.0f, 5.0f, 11.0f, 18.0f, 88.0f}},
        {(1, 16), new float[] {110.0f, 41.0f, 10.0f, 5.0f, 3.0f, 1.5f, 1.0f, 0.5f, 0.3f, 0.5f, 1.0f, 1.5f, 3.0f, 5.0f, 10.0f, 41.0f, 110.0f}},

        {(2, 8), new float[] {29.0f, 4.0f, 1.5f, 0.3f, 0.2f, 0.3f, 1.5f, 4.0f, 29.0f}},
        {(2, 9), new float[] {43.0f, 7.0f, 2.0f, 0.6f, 0.2f, 0.2f, 0.6f, 2.0f, 7.0f, 43.0f}},
        {(2, 10), new float[] {76.0f, 10.0f, 3.0f, 0.9f, 0.3f, 0.2f, 0.3f, 0.9f, 3.0f, 10.0f, 76.0f}},
        {(2, 11), new float[] {120.0f, 14.0f, 5.2f, 1.4f, 0.4f, 0.2f, 0.2f, 0.4f, 1.4f, 5.2f, 14.0f, 120.0f}},
        {(2, 12), new float[] {170.0f, 24.0f, 8.1f, 2.0f, 0.7f, 0.2f, 0.2f, 0.2f, 0.7f, 2.0f, 8.1f, 24.0f, 170.0f}},
        {(2, 13), new float[] {260.0f, 37.0f, 11.0f, 4.0f, 1.0f, 0.2f, 0.0f, 2.0f, 0.0f, 2.0f, 1.0f, 4.0f, 11.0f, 37.0f, 260.0f}},
        {(2, 14), new float[] {420.0f, 56.0f, 18.0f, 5.0f, 1.9f, 0.3f, 0.2f, 0.2f, 0.2f, 0.3f, 1.9f, 5.0f, 18.0f, 56.0f, 420.0f}},
        {(2, 15), new float[] {620.0f, 83.0f, 27.0f, 8.0f, 3.0f, 0.5f, 0.2f, 0.2f, 0.2f, 0.2f, 0.5f, 3.0f, 8.0f, 27.0f, 83.0f, 620.0f}},
        {(2, 16), new float[] {1000.0f, 130.0f, 26.0f, 9.0f, 4.0f, 2.0f, 0.2f, 0.2f, 0.2f, 0.2f, 0.2f, 2.0f, 4.0f, 9.0f, 26.0f, 130.0f, 1000.0f}},
     };

        // Try to get the appropriate multiplier values based on risk and numRows
        if (multipliers.TryGetValue((riskLevel, numRows), out var selectedMultipliers) && index < selectedMultipliers.Length)
        {
            return selectedMultipliers[index];
        }

        return 1.0f;
    }
}
