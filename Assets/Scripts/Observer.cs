using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public static class EventName
{
    public static readonly string AddMoney = "AddMoney";
    public static readonly string SubstractMoney = "SubstractMoney";
    public static readonly string LockPanel = "LockPanel";
}

public class Observer : MonoBehaviour //do rozdzielenia zaleznosci miedzy skryptami, wywolywanie zmian w UI, zliczania, questy
{

    public static Observer Instance;
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
    public delegate void CallBackObserver(System.Object data);

    Dictionary<string, HashSet<CallBackObserver>> dictObserver = new Dictionary<string, HashSet<CallBackObserver>>();
    // Use this for initialization
    public void AddObserver(string topicName, CallBackObserver callbackObserver)
    {
        HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
        listObserver.Add(callbackObserver);
    }

    public void RemoveObserver(string topicName, CallBackObserver callbackObserver)
    {
        HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
        listObserver.Remove(callbackObserver);
    }
    public void Notify(string topicName, System.Object Data)
    {
        HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
        foreach (CallBackObserver observer in listObserver)
        {
            observer(Data);
        }
    }
    public void Notify(string topicName)
    {
        HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
        foreach (CallBackObserver observer in listObserver)
        {
            observer(null);
        }
    }
    protected HashSet<CallBackObserver> CreateListObserverForTopic(string topicName)
    {
        if (!dictObserver.ContainsKey(topicName))
            dictObserver.Add(topicName, new HashSet<CallBackObserver>());
        return dictObserver[topicName];
    }

    internal void AddObserver(string nextWave)
    {
        throw new NotImplementedException();
    }
}