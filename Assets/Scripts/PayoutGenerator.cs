using UnityEngine;
using TMPro;

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
        // Clear previous payouts
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

        // Place payout boxes slightly below last peg row
        float payoutY = lastRowY - (verticalSpacing * 0.6f);

        float payoutScale = 1.0f * (8f / numRows);

        for (int i = 0; i < numRows + 1; i++)
        {
            // Shift each payout by half a spacing to align between pegs
            float xOffset = horizontalSpacing / 2;

            Vector2 payoutPosition = new Vector2(
                (-areaWidth / 2) + (i * horizontalSpacing) + xOffset,
                payoutY
            );

            GameObject payoutBox = Instantiate(payoutPrefab, transform);
            payoutBox.transform.SetParent(plinkoArea, false);
            payoutBox.GetComponent<RectTransform>().anchoredPosition = payoutPosition;
            payoutBox.transform.localScale = new Vector3(payoutScale, payoutScale, payoutScale);
        }
    }
}
