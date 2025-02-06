using System;
using UnityEngine;

public class Histogram : MonoBehaviour
{
    private void Start()
    {
        Observer.Instance.AddObserver(EventName.AddHistory, AddHistory);
    }

    private void AddHistory(object data)
    {
        throw new NotImplementedException();
    }
}
