using UnityEngine;

public class WalletCanvas : MonoBehaviour
{
    public static WalletCanvas Instance { get; private set; }
    void Awake()
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
}