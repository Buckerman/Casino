using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PayoutGenerator : MonoBehaviour
{
    public static PayoutGenerator Instance { get; private set; }

    [SerializeField] private GameObject payoutPrefab;
    [SerializeField] private TMP_Dropdown dropDownRisk;
    [SerializeField] private RectTransform plinkoArea;
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
        GeneratePayout();
    }
    public void GeneratePayout()
    {

    }
}
