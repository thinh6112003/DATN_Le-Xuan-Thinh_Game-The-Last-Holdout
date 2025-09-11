using System;
using System.Collections.Generic;

public static class EventObserver
{
    // Dictionary chứa event: key -> list listener
    private static Dictionary<string, List<Action<object[]>>> _events
        = new Dictionary<string, List<Action<object[]>>>();

    // Đăng ký listener
    public static void AddListener(string key, Action<object[]> callback)
    {
        if (!_events.ContainsKey(key))
        {
            _events[key] = new List<Action<object[]>>();
        }

        if (!_events[key].Contains(callback))
        {
            _events[key].Add(callback);
        }
    }

    // Gỡ listener
    public static void RemoveListener(string key, Action<object[]> callback)
    {
        if (_events.ContainsKey(key))
        {
            _events[key].Remove(callback);

            // Nếu list rỗng thì remove key
            if (_events[key].Count == 0)
                _events.Remove(key);
        }
    }

    // Gửi thông báo (Notice)
    public static void Notice(string key, params object[] args)
    {
        if (_events.ContainsKey(key))
        {
            foreach (var callback in _events[key])
            {
                callback?.Invoke(args);
            }
        }
    }
}
