using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBalls : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private RectTransform plinkoArea;

    private Button betButton;
    private TMP_InputField betAmountText;

    private void Start()
    {
        betButton = GameManager.Instance.GetComponent<GameManager>().BetButton;
        betAmountText = GameManager.Instance.GetComponent<GameManager>().BetAmountText;

        betButton.onClick.AddListener(() => SpawnBall());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBall();
        }
        int activeBallsCount = GetActiveBallsCount();

        if (activeBallsCount == 0)
        {
            // Unlock panel if no balls are active
            Observer.Instance.Notify(EventName.TogglePanel, true);
        }
    }
    public void SpawnBall()
    {
        if (betButton.interactable && float.Parse(betAmountText.text) > 0)
        {
            GameObject ballObject = ObjectPooling.Instance.GetObject(ballPrefab);
            Ball ball = ballObject.GetComponent<Ball>();
            ball.Initialize(plinkoArea);

            float betAmount;
            if (float.TryParse(betAmountText.text, out betAmount))
            {
                Observer.Instance.Notify(EventName.SubstractMoney, betAmount);
            }

            Observer.Instance.Notify(EventName.TogglePanel, false);
        }
    }
    private int GetActiveBallsCount()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        int count = 0;
        foreach (GameObject ball in balls)
        {
            if (ball.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }
}
