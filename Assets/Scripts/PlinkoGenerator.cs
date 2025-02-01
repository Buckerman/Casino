using TMPro;
using UnityEngine;

public class PlinkoGenerator : MonoBehaviour
{
    public static PlinkoGenerator Instance { get; private set; }
    public int NumRows { get => numRows; set => numRows = value; }

    [SerializeField] private GameObject pegPrefab;
    [SerializeField] private TMP_Dropdown dropDownRows;
    [SerializeField] private RectTransform plinkoArea;
    private int numRows = 8;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        GeneratePlinkoPyramid();
    }

    public void GeneratePlinkoPyramid()
    {
        // Clear previous pegs
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        NumRows = int.Parse(dropDownRows.options[dropDownRows.value].text);

        float areaWidth = plinkoArea.rect.width;
        float areaHeight = plinkoArea.rect.height;

        float topY = areaHeight / 2;

        float horizontalSpacing = areaWidth / (NumRows + 1);

        float verticalSpacing = Mathf.Sqrt(3) / 2 * horizontalSpacing;
        for (int row = 0; row < NumRows; row++)
        {
            int numPegs = row + 3;

            float rowWidth = (numPegs - 1) * horizontalSpacing;
            float rowStartX = -rowWidth * 0.5f;

            float rowStartY = topY - (row * verticalSpacing);

            // Calculate the new scale based on the number of rows
            float newScale = 0.5f * (8f / NumRows); // 0.5 scale for 8 rows, reduced as rows increase
            for (int col = 0; col < numPegs; col++)
            {
                Vector2 pegPosition = new Vector2(
                    rowStartX + col * horizontalSpacing,
                    rowStartY
                );
                GameObject peg = Instantiate(pegPrefab, transform);

                // Convert local position to world space
                Vector3 worldPos = plinkoArea.TransformPoint(pegPosition);
                worldPos.z = 0f; // Ensure pegs are in front of the background image
                peg.transform.position = worldPos;

                peg.transform.localScale = new Vector3(newScale, newScale, newScale);
            }
        }
    }
}
