using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Josh.EventSystem {

    [System.Serializable]
    public class Event : UnityEvent<object> { };

    public class EventResponder : MonoBehaviour, ISerializationCallbackReceiver {

        public Dictionary<string, System.Action<object>> globalEventRegister = new Dictionary<string, System.Action<object>>();
        public static List<EventResponder> responders = new List<EventResponder>();

        public Dictionary<string, Event> eventDefinitons = new Dictionary<string, Event>();

        public static string EVENT_TRIGGERS_PROPERTY = "serializedEventTriggers";
        [SerializeField]
        private List<string> serializedEventTriggers = new List<string>();

        public static string EVENTS_PROPERTY = "serializedEvents";
        [SerializeField]
        private List<Event> serializedEvents = new List<Event>();

        public void OnEnable()
        {
            OnBeforeSerialize();
            if (!responders.Contains(this))
            {
                responders.Add(this);
            }
        }

        public void OnDisable()
        {
            responders.Remove(this);
        }

        public void OnAfterDeserialize()
        {
            eventDefinitons.Clear();
            for (int i = 0; i < serializedEventTriggers.Count; i++)
            {
                eventDefinitons.Add(serializedEventTriggers[i], serializedEvents[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            serializedEventTriggers = new List<string>();
            serializedEvents = new List<Event>();
            foreach (var pair in eventDefinitons)
            {
                serializedEventTriggers.Add(pair.Key);
                serializedEvents.Add(pair.Value);
            }
        }


        public static void TriggerEvent(string eventName, object arguements = null)
        {
            for (int i = 0; i < responders.Count; i++)
            {
                responders[i].RecieveEvent(eventName, arguements);
            }
        }

        private void RecieveEvent(string eventName, object arguements = null)
        {
            if (eventDefinitons.ContainsKey(eventName))
            {
                eventDefinitons[eventName].Invoke(arguements);
            }
        }

    }
}
