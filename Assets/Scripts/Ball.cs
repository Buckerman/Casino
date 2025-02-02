using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private RectTransform plinkoArea;
    private SpriteRenderer sr;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    public void Initialize(RectTransform gameArea)
    {
        plinkoArea = gameArea;

        float newScale = 0.5f * (8f / PlinkoGenerator.Instance.NumRows);
        transform.localScale = new Vector3(newScale, newScale, newScale);

        Vector2 spawnPositionUI = new Vector2(Random.Range(-25f, 25f), plinkoArea.rect.height / 2 + 100f);
        Vector3 worldPos = plinkoArea.TransformPoint(spawnPositionUI);
        worldPos.z = 0f;

        var risk = GameManager.Instance.DropdownRisk;

        //This solution prevent from switching option order
        switch (risk.options[risk.value].text)
        {
            case "Low":
                sr.color = Color.yellow;
                break;
            case "Medium":
                sr.color = new Color(1f, 0.647f, 0f);
                break;
            case "High":
                sr.color = Color.red;
                break;
        }
        #region
        //switch (risk.value)
        //{
        //    case 0:
        //        sr.color = Color.yellow;
        //        break;
        //    case 1:
        //        sr.color = new Color(1f, 0.647f, 0f);
        //        break;
        //    case 2:
        //        sr.color = Color.red;
        //        break;
        //}
        #endregion

        transform.position = worldPos;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Peg"))
        {
            // Apply random spin based on collision force
            float spinForce = Random.Range(-0.1f, 0.1f); // Adjust value for desired effect
            rb.AddTorque(spinForce, ForceMode2D.Impulse);
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            gameObject.SetActive(false);

            // Parse both value of box and the bet amount into float numbers
            TextMeshProUGUI textComponent = collision.gameObject.GetComponentInChildren<TextMeshProUGUI>();
            if (float.TryParse(textComponent.text, out float boxValue) &&
                float.TryParse(GameManager.Instance.BetAmountText.text, out float betAmount))
            {
                Observer.Instance.Notify(EventName.AddMoney, betAmount * boxValue);
            }
        }
    }
}
