using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private TextMeshProUGUI walletText;
    private float money;
    public float Money { get => money;}

    private void Awake()
    {
        walletText = GetComponentInChildren<TextMeshProUGUI>();

        string text = walletText.text.Replace("$", "");
        float.TryParse(text, out money);

        UpdateWalletText();
    }

    private void Start()
    {
        Observer.Instance.AddObserver(EventName.AddMoney, AddMoney);
        Observer.Instance.AddObserver(EventName.SubstractMoney, SubstractMoney);
    }

    private void SubstractMoney(object data)
    {
        if (data is float amount)
        {
            money = Mathf.Max(0, money - amount);
            UpdateWalletText();
        }
    }

    private void AddMoney(object data)
    {
        if (data is float amount)
        {
            money += amount;
            UpdateWalletText();
        }
    }
    private void UpdateWalletText()
    {
        walletText.text = string.Format(CultureInfo.InvariantCulture, "${0:0.00}", money);
    }
    private void OnDestroy()
    {
        Observer.Instance.RemoveObserver(EventName.AddMoney, AddMoney);
        Observer.Instance.RemoveObserver(EventName.SubstractMoney, SubstractMoney);
    }
}
