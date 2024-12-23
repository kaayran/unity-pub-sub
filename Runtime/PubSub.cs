using System;
using System.Collections.Generic;

namespace PublisherSubscriber.Runtime
{
    public static class PubSub
    {
        public static void Subscribe(string message, Delegate callback)
        {
            if (!_subscriptions.ContainsKey(message))
            {
                _subscriptions[message] = new List<Delegate>();
            }

            _subscriptions[message].Add(callback);
        }

        public static void Unsubscribe(string message, Delegate callback)
        {
            if (!_subscriptions.ContainsKey(message)) return;

            _subscriptions[message].Remove(callback);
            if (_subscriptions[message].Count == 0)
            {
                _subscriptions.Remove(message);
            }
        }

        public static void Broadcast(string message)
        {
            if (!_subscriptions.TryGetValue(message, out var subscription)) return;

            foreach (var callback in subscription)
            {
                if (callback is Action action)
                {
                    action();
                }
            }
        }

        #region private

        private static readonly Dictionary<string, List<Delegate>> _subscriptions = new();

        #endregion
    }
}