using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab; 
    [SerializeField] private RectTransform plinkoArea;

    private Button betButton;
    private TMP_InputField betAmountText;
    private TMP_InputField betCountText;
    private PlinkoManager plinkoManager;

    private void Start()
    {
        plinkoManager = PlinkoManager.Instance;
        betButton = plinkoManager.BetButton;
        betAmountText = plinkoManager.BetAmountText;
        betCountText = plinkoManager.BetCountText;

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
            plinkoManager.LockBetButton(false); // Unlock bet button when no balls are active
            Observer.Instance.Notify(EventName.TogglePanel, true);
        }
    }
    public void SpawnBall()
    {
        float.TryParse(betAmountText.text, out float betAmount);
        int.TryParse(betCountText.text, out int betCount);

        if (!betButton.interactable || betAmount <= 0) return;

        int spawnCount = plinkoManager.autoPlay ? betCount : 1;
        float totalBetAmount = betAmount * spawnCount;

        if (plinkoManager.autoPlay)
        {
            plinkoManager.LockBetButton(true); // Lock bet button when autoplay starts
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject ballObject = ObjectPooling.Instance.GetObject(ballPrefab);
            ballObject.GetComponent<Ball>().Initialize(plinkoArea);
        }

        Observer.Instance.Notify(EventName.SubstractMoney, totalBetAmount);
        Observer.Instance.Notify(EventName.TogglePanel, false);
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
