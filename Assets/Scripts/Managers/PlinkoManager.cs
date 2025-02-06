using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlinkoManager : MonoBehaviour
{
    public static PlinkoManager Instance { get; private set; }
    public Button BetButton { get => betButton; set => betButton = value; }
    public TMP_InputField BetAmountText { get => betAmountText; }
    public TMP_Dropdown DropdownRisk { get => dropdownRisk; }
    public TMP_InputField BetCountText { get => betCountText; }

    [SerializeField] private GameObject switchMode;
    [SerializeField] private TMP_InputField betAmountText;
    [SerializeField] private TMP_InputField betCountText;
    [SerializeField] private Button betButton;
    [SerializeField] private Button divideButton;
    [SerializeField] private Button multiplyButton;
    [SerializeField] private TMP_Dropdown dropdownRisk;
    [SerializeField] private TMP_Dropdown dropdownRows;

    private Wallet wallet;
    private bool isBetLocked = false;
    public bool autoPlay;

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

    private void Start()
    {
        Observer.Instance.AddObserver(EventName.TogglePanel, TogglePanel);
        Observer.Instance.AddObserver(EventName.AutoPlay, AutoPlay);

        wallet = Wallet.Instance;

        BetAmountText.onEndEdit.AddListener(FormatBetAmount);
    }

    private void AutoPlay(object data)
    {
        autoPlay = (bool)data;
    }

    private void Update()
    {
        if (isBetLocked) return;
        ToggleBetButton();
    }
    private void ToggleBetButton()
    {
        float betAmount;
        int betCount;

        bool isValidBetAmount = float.TryParse(BetAmountText.text, out betAmount) && betAmount >= 0.1f;
        bool isValidBetCount = int.TryParse(BetCountText.text, out betCount) && betCount > 0;

        if (isValidBetAmount)
        {
            if (autoPlay && !isValidBetCount)
            {
                BetButton.interactable = false;
                return;
            }

            BetButton.interactable = (betAmount * (autoPlay ? betCount : 1)) <= wallet.Money;
        }
        else
        {
            BetButton.interactable = false;
        }
    }

    public void DivideBet()
    {
        if (float.TryParse(BetAmountText.text, out float betAmount) && betAmount > 0)
        {
            betAmount /= 2;
            betAmount = Mathf.Max(betAmount, 0.1f);
            BetAmountText.text = betAmount.ToString("F2");
        }
    }

    public void DoubleBet()
    {
        if (float.TryParse(BetAmountText.text, out float betAmount) && betAmount > 0)
        {
            betAmount *= 2;
            betAmount = Mathf.Min(betAmount, wallet.Money);
            BetAmountText.text = betAmount.ToString("F2");
        }
    }

    private void TogglePanel(object data)
    {
        bool state = (bool)data;
        switchMode.GetComponent<ToggleSwitch>().enabled = state;
        BetAmountText.interactable = state;
        BetCountText.interactable = state;
        divideButton.interactable = state;
        multiplyButton.interactable = state;
        DropdownRisk.interactable = state;
        dropdownRows.interactable = state;
    }

    public void LockBetButton(bool lockState)
    {
        isBetLocked = lockState;
        if (lockState) BetButton.interactable = false;
    }
    private void FormatBetAmount(string value)
    {
        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float betAmount))
        {
            BetAmountText.text = betAmount.ToString("F2", CultureInfo.InvariantCulture);
        }
    }

    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(EventName.TogglePanel, TogglePanel);
        Observer.Instance.RemoveObserver(EventName.AutoPlay, AutoPlay);
    }
}
