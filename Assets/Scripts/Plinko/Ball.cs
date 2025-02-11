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

        float newScale = 0.5f * (8f / PegGenerator.Instance.NumRows);
        transform.localScale = new Vector3(newScale, newScale, newScale);

        Vector2 spawnPositionUI = new Vector2(Random.Range(-25f, 25f), plinkoArea.rect.height / 2 + 100f);
        Vector3 worldPos = plinkoArea.TransformPoint(spawnPositionUI);
        worldPos.z = 0f;

        SetBallRisk();

        transform.position = worldPos;
    }

    private void SetBallRisk()
    {
        var risk = PlinkoManager.Instance.DropdownRisk;
        //This solution prevent from switching option order
        switch (risk.options[risk.value].text)
        {
            case "Low":
                sr.color = Color.yellow;
                difficultyOdds = 0.85f;
                xBiasStrength = 1f;
                break;
            case "Medium":
                sr.color = new Color(1f, 0.647f, 0f);
                difficultyOdds = 0.9f;
                xBiasStrength = 1f;
                break;
            case "High":
                sr.color = Color.red;
                difficultyOdds = 0.95f;
                xBiasStrength = 1.1f;
                break;
        }
        //Check by Q order
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
    }

    // You can adjust this value in the Inspector to control the pull strength
    private float xBiasStrength;
    private float difficultyOdds;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Peg"))
        {
            // Convert Plinko area position from Canvas space to World space
            Vector2 plinkoCenter = RectTransformUtility.WorldToScreenPoint(Camera.main, plinkoArea.position);
            plinkoCenter = Camera.main.ScreenToWorldPoint(plinkoCenter);

            // Apply small spin for randomness
            float spinForce = Random.Range(-0.1f, 0.1f);
            rb.AddTorque(spinForce, ForceMode2D.Impulse);

            // Apply damping on the Y velocity (vertical damping) for bounce
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.9f);

            // Determine the horizontal direction (left or right)
            float horizontalPosition = transform.position.x - plinkoCenter.x;
            float directionBias = horizontalPosition > 0 ? -1f : 1f;

            // Apply force toward the center of the Plinko area with direction bias and adjustable strength
            rb.AddForce(new Vector2(directionBias * xBiasStrength, 0) * difficultyOdds, ForceMode2D.Impulse);

            // Apply random X velocity shift (simulating randomness and dampening)
            float randomXShift = Random.Range(-0.5f, 0.5f);
            rb.velocity = new Vector2(rb.velocity.x + randomXShift, rb.velocity.y);
            rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y);

            collision.gameObject.GetComponent<Peg>().PegAnimation();
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            gameObject.SetActive(false);
            collision.gameObject.GetComponent<PayoutBox>().Score();
        }
    }
}
