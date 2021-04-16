using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    private static Dictionary<string, UnityEvent> _unityEvents = new Dictionary<string, UnityEvent>();

    public static void Listen(string eventName, UnityAction listener)
    {
        UnityEvent unityEvent;
        if (_unityEvents.TryGetValue(eventName, out unityEvent))
        {
            unityEvent.AddListener(listener);
        }
        else
        {
            unityEvent = new UnityEvent();
            unityEvent.AddListener(listener);
            _unityEvents.Add(eventName, unityEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        UnityEvent unityEvent;
        if (_unityEvents.TryGetValue(eventName, out unityEvent))
        {
            unityEvent.RemoveListener(listener);
        }
    }

    public static void Invoke(string eventName)
    {
        UnityEvent unityEvent;
        if (_unityEvents.TryGetValue(eventName, out unityEvent))
        {
            unityEvent.Invoke();
        }
    }

    public static void GetUnityEvents()
    {
        foreach (KeyValuePair<string, UnityEvent> entry in _unityEvents)
        {
            Debug.Log(entry.Key + "   :   " + entry.Value);
        }
    }
}