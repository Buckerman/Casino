using UnityEngine;
using UnityEngine.UI;

public class SpawnBalls : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Button betButton;
    [SerializeField] private RectTransform plinkoArea;

    private void Start()
    {
        betButton.onClick.AddListener(() =>  SpawnBall());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBall();
        }
    }
    public void SpawnBall()
    {
        GameObject ballObject = ObjectPooling.Instance.GetObject(ballPrefab);
        Ball ball = ballObject.GetComponent<Ball>();
        ball.Initialize(plinkoArea);
    }
}
