using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BjackManager : MonoBehaviour
{
    public static BjackManager Instance { get; private set; }

    [SerializeField] private TMP_InputField betAmountText;
    [SerializeField] private Button betButton;
    [SerializeField] private Button divideButton;
    [SerializeField] private Button multiplyButton;

    private Wallet wallet;

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
        wallet = Wallet.Instance;

        betAmountText.onEndEdit.AddListener(delegate { GameManager.Instance.FormatBetAmount(betAmountText); });
    }

    private void Update()
    {
        GameManager.Instance.ToggleBetButton(betButton, betAmountText);
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
        throw new NotImplementedException();
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(EventName.TogglePanel, TogglePanel);
    }
}
