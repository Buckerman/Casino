using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private RectTransform plinkoArea;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Initialize(RectTransform gameArea)
    {
        plinkoArea = gameArea;
        Vector2 spawnPositionUI = new Vector2(Random.Range(-25f, 25f), plinkoArea.rect.height / 2 + 100f);

        Vector3 worldPos = plinkoArea.TransformPoint(spawnPositionUI);
        worldPos.z = 0f;

        transform.position = worldPos;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Peg"))
        {
            // Apply random spin based on collision force
            float spinForce = Random.Range(-1f, 1f); // Adjust value for desired effect
            rb.AddTorque(spinForce, ForceMode2D.Impulse);
        }
    }
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
