using System;
using System.Collections.Generic;

public static class EventManager {

    private static Dictionary<string, Action<string>> _eventDictionary;

    public static void InitDict() {
        _eventDictionary = new Dictionary<string, Action<string>>();
    }

    public static void StartListening(string eventName, Action<string> listener) {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent)) {
            // Add more event to the existing one
            thisEvent += listener;

            // Update the Dictionary
            _eventDictionary[eventName] = thisEvent;
        }
        else {
            // Add event to the Dictionary for the first time
            thisEvent += listener;
            _eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<string> listener) {
        if (!_eventDictionary.TryGetValue(eventName, out var thisEvent)) return;

        // Remove event from the existing one
        thisEvent -= listener;

        // Update the Dictionary
        _eventDictionary[eventName] = thisEvent;
    }

    public static void TriggerEvent(string eventName, string eventParam) {
        if (_eventDictionary.TryGetValue(eventName, out var thisEvent)) {
            thisEvent.Invoke(eventParam);
        }
    }

}