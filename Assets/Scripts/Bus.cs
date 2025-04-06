using System;
using System.Collections.Generic;

public static class Bus
{
    // Dictionary where the key is the event type, and value is the delegate (action)
    private static Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

    // Subscribe to an event
    public static void Subscribe<T>(Action<T> listener)
    {
        if (_events.TryGetValue(typeof(T), out var del))
        {
            _events[typeof(T)] = Delegate.Combine(del, listener);
        }
        else
        {
            _events[typeof(T)] = listener;
        }
    }

    // Unsubscribe from an event
    public static void Unsubscribe<T>(Action<T> listener)
    {
        if (_events.TryGetValue(typeof(T), out var del))
        {
            var currentDel = Delegate.Remove(del, listener);

            if (currentDel == null)
                _events.Remove(typeof(T));
            else
                _events[typeof(T)] = currentDel;
        }
    }

    // Publish (fire) an event
    public static void Publish<T>(T eventData)
    {
        if (_events.TryGetValue(typeof(T), out var del))
        {
            (del as Action<T>)?.Invoke(eventData);
        }
    }

    // Clear all events (use carefully!)
    public static void ClearAll()
    {
        _events.Clear();
    }
}
