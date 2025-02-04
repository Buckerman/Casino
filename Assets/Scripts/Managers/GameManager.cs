using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Button BetButton { get => betButton; }
    public TMP_InputField BetAmountText { get => betAmountText; }
    public TMP_Dropdown DropdownRisk { get => dropdownRisk; }

    [SerializeField] private Wallet wallet;

    [SerializeField] private GameObject switchMode;
    [SerializeField] private TMP_InputField betAmountText;
    [SerializeField] private TMP_InputField betCountText;
    [SerializeField] private Button betButton;
    [SerializeField] private Button divideButton;
    [SerializeField] private Button multiplyButton;
    [SerializeField] private TMP_Dropdown dropdownRisk;
    [SerializeField] private TMP_Dropdown dropdownRows;

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
        Observer.Instance.AddObserver(EventName.TogglePanel, TogglePanel);
    }

    private void Update()
    {
        float betAmount;
        if (float.TryParse(BetAmountText.text, out betAmount) && betAmount >= 0.1f)
        {
            BetButton.interactable = betAmount <= wallet.Money;
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

            if (betAmount < 0.1f)
            {
                betAmount = 0.1f;
            }

            BetAmountText.text = betAmount.ToString("F2");
        }
    }
    public void DoubleBet()
    {
        if (float.TryParse(BetAmountText.text, out float betAmount) && betAmount > 0)
        {
            betAmount *= 2;

            if (betAmount > wallet.Money)
            {
                betAmount = wallet.Money;
            }

            BetAmountText.text = betAmount.ToString("F2");
        }
    }
    private void TogglePanel(object data)
    {
        switchMode.GetComponent<ToggleSwitch>().enabled = (bool)data;
        betAmountText.interactable = (bool)data;
        betCountText.interactable = (bool)data;
        divideButton.interactable = (bool)data;
        multiplyButton.interactable = (bool)data;
        DropdownRisk.interactable = (bool)data;
        dropdownRows.interactable = (bool)data;
    }
}
