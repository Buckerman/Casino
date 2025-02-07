using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallsGenerator : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private RectTransform plinkoArea;
    [SerializeField] private GameObject infiniteText;
    [SerializeField] private float ballSpawnDelay = 0.5f; // Adjust delay as needed

    private Button betButton;
    private TMP_InputField betAmountText;
    private TMP_InputField betCountText;
    private PlinkoManager plinkoManager;
    private Coroutine autoPlayCoroutine;

    private void Start()
    {
        plinkoManager = PlinkoManager.Instance;
        betButton = plinkoManager.BetButton;
        betAmountText = plinkoManager.BetAmountText;
        betCountText = plinkoManager.BetCountText;

        betButton.onClick.AddListener(() => StartSpawning());
        betCountText.onValueChanged.AddListener(ToggleInfiniteIcon);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartSpawning();
        }

        int activeBallsCount = GetActiveBallsCount();

        if (autoPlayCoroutine == null && plinkoManager.autoPlay)
        {
            betButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Start AutoBet";
        }
        if (activeBallsCount == 0)
        {
            // Unlock panel if no balls are active
            Observer.Instance.Notify(EventName.TogglePanel, true);
        }
    }

    private bool isAutoBetRunning = false;
    private void StartSpawning()
    {
        float.TryParse(betAmountText.text, out float betAmount);
        int.TryParse(betCountText.text, out int betCount);

        if (!betButton.interactable || betAmount <= 0 || betCount < 0) return;

        if (isAutoBetRunning)
        {
            StopCoroutine(autoPlayCoroutine);
            autoPlayCoroutine = null;
            isAutoBetRunning = false;
            betButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Start AutoBet";
            return;
        }

        if (plinkoManager.autoPlay)
        {
            autoPlayCoroutine = StartCoroutine(AutoSpawnBalls(betAmount, betCount));
            isAutoBetRunning = true;
            betButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Stop AutoBet";
        }
        else
        {
            SpawnBall(betAmount);
        }
    }
    private IEnumerator AutoSpawnBalls(float betAmount, int betCount)
    {
        bool infiniteMode = betCount == 0;

        while (infiniteMode || betCount > 0)
        {
            if (Wallet.Instance.Money < betAmount)
            {
                break; // Stop if money runs out
            }
            GameObject ballObject = ObjectPooling.Instance.GetObject(ballPrefab);
            ballObject.GetComponent<Ball>().Initialize(plinkoArea);

            Observer.Instance.Notify(EventName.SubstractMoney, betAmount);
            Observer.Instance.Notify(EventName.TogglePanel, false);

            if (!infiniteMode)
            {
                betCount--;
                betCountText.text = betCount.ToString();
            }

            yield return new WaitForSeconds(ballSpawnDelay);
        }

        isAutoBetRunning = false;
        autoPlayCoroutine = null;
    }
    private void SpawnBall(float betAmount)
    {
        GameObject ballObject = ObjectPooling.Instance.GetObject(ballPrefab);
        ballObject.GetComponent<Ball>().Initialize(plinkoArea);

        Observer.Instance.Notify(EventName.SubstractMoney, betAmount);
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
    private void ToggleInfiniteIcon(string value)
    {
        if (int.TryParse(value, out int betCount) && betCount > 0)
        {
            infiniteText.SetActive(false);
        }
        else
        {
            infiniteText.SetActive(true);
        }
    }
}
