using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Common.Events
{
    //T is only used to guarantee there is implictely an independant available queue for each given type to communicate
    public static class EventQueue<T>
    {
        private static Queue<Action> _queue = new ();
        public static void QueueEvent(Action eventToAdd)
        {
            _queue.Enqueue(eventToAdd);
        }
        public static void ProcessAll()
        {
            if (_queue.Count > 0)
            {
                var eventToInvoke = _queue.Dequeue();
                eventToInvoke?.Invoke();
            }
        }
    }
}