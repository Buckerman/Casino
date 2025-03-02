using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlinkoManager : MonoBehaviour
{
    public static PlinkoManager Instance { get; private set; }
    public Button BetButton { get => betButton; set => betButton = value; }
    public TMP_InputField BetAmountText { get => betAmountText; }
    public TMP_Dropdown DropdownRisk { get => dropdownRisk; }
    public TMP_InputField BetCountText { get => betCountText; set => betCountText = value; }

    [SerializeField] private GameObject switchMode;
    [SerializeField] private TMP_InputField betAmountText;
    [SerializeField] private TMP_InputField betCountText;
    [SerializeField] private Button betButton;
    [SerializeField] private Button divideButton;
    [SerializeField] private Button multiplyButton;
    [SerializeField] private TMP_Dropdown dropdownRisk;
    [SerializeField] private TMP_Dropdown dropdownRows;

    private Wallet wallet;
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

        betAmountText.onEndEdit.AddListener(delegate { GameManager.Instance.FormatBetAmount(betAmountText); });
    }

    private void AutoPlay(object data)
    {
        autoPlay = (bool)data;
    }

    private void Update()
    {
        GameManager.Instance.ToggleBetButton(betButton, betAmountText, betCountText, autoPlay);
    }

    public void DivideBet()
    {
        GameManager.Instance.DivideBet(betAmountText);
    }

    public void DoubleBet()
    {
        GameManager.Instance.DoubleBet(betAmountText);
    }

    private void TogglePanel(object data)
    {
        bool state = (bool)data;
        switchMode.GetComponent<ToggleSwitch>().enabled = state;
        betAmountText.interactable = state;
        betCountText.interactable = state;
        divideButton.interactable = state;
        multiplyButton.interactable = state;
        dropdownRisk.interactable = state;
        dropdownRows.interactable = state;
    }

    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(EventName.TogglePanel, TogglePanel);
        Observer.Instance.RemoveObserver(EventName.AutoPlay, AutoPlay);
    }
}
