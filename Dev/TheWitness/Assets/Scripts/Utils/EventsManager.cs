using UnityEngine.Events;
using UnityEngine;

namespace Utils
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class EventsManager : Singleton<EventsManager>
    {
        [SerializeField] Events m_objectEvent;
        Dictionary<string, UnityEvent> m_events;

        public Dictionary<string, UnityEvent> events {  get { return m_events; } }

        public void Active(string _eventName)
        {
            events[_eventName]?.Invoke();
        }

        private void OnEnable()
        {
            m_events = m_objectEvent.Get();
        }
    }

    [Serializable]
    public class Events
    {
        Dictionary<string, UnityEvent> m_dictEvents = new Dictionary<string, UnityEvent>();

        [SerializeField] List<eventDict> events;

        public Dictionary<string, UnityEvent> Get()
        {
            for (int i = 0; i < events.Count; i++)
            {
                m_dictEvents.Add(events[i].id, events[i].unityEvent);
            }

            return m_dictEvents;
        }
    }

    [Serializable]
    public class eventDict
    {
        [SerializeField] string m_id;
        [SerializeField] UnityEvent m_event;

        public string id { get { return m_id; } set { m_id = value; } }
        public UnityEvent unityEvent { get { return m_event; } set { m_event = value; } }
    }
}