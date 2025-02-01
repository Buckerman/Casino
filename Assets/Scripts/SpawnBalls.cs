using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBalls : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Button betButton;
    [SerializeField] private RectTransform plinkoArea;
    [SerializeField] private TMP_InputField betAmountText;

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

        float betAmount;
        if (float.TryParse(betAmountText.text, out betAmount))
        {
            Observer.Instance.Notify(EventName.SubstractMoney, betAmount);
        }
    }
}
