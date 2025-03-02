using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private SceneManagerScript sceneManager;
    private Wallet wallet;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        wallet = Wallet.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            sceneManager.LoadScene("Hilo");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sceneManager.LoadScene("Plinko");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            sceneManager.LoadScene("Bjack");
        }
    }

    public void DivideBet(TMP_InputField betAmountText)
    {
        if (float.TryParse(betAmountText.text, out float betAmount) && betAmount > 0)
        {
            betAmount /= 2;
            betAmount = Mathf.Max(betAmount, 0.1f);
            betAmountText.text = betAmount.ToString("F2");
        }
    }

    public void DoubleBet(TMP_InputField betAmountText)
    {
        if (float.TryParse(betAmountText.text, out float betAmount) && betAmount > 0)
        {
            betAmount *= 2;
            betAmount = Mathf.Min(betAmount, wallet.Money);
            betAmountText.text = betAmount.ToString("F2");
        }
    }

    public void ToggleBetButton(Button betButton, TMP_InputField betAmountText, TMP_InputField betCountText, bool autoPlay)
    {
        float betAmount;
        int betCount;

        bool isValidBetAmount = float.TryParse(betAmountText.text, out betAmount) && betAmount >= 0.1f;
        int.TryParse(betCountText.text, out betCount);

        if (!isValidBetAmount)
        {
            betButton.interactable = false;
            return;
        }

        if (autoPlay && betCount == 0)
        {
            betButton.interactable = betAmount <= wallet.Money;
            return;
        }

        betButton.interactable = (betAmount * (autoPlay ? betCount : 1)) <= wallet.Money;
    }
    
    //Overload
    public void ToggleBetButton(Button betButton, TMP_InputField betAmountText)
    {
        float betAmount;
        bool isValidBetAmount = float.TryParse(betAmountText.text, out betAmount) && betAmount >= 0.1f;

        betButton.interactable = isValidBetAmount && betAmount <= wallet.Money;
    }

    public void FormatBetAmount(TMP_InputField betAmountText)
    {
        if (float.TryParse(betAmountText.text, NumberStyles.Float, CultureInfo.InvariantCulture, out float betAmount))
        {
            betAmountText.text = betAmount.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}
