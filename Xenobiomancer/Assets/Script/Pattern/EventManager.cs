using System;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns
{
    public class EventManager
    {
        private static EventManager instance;

        public static EventManager Instance // making the eventmanager singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventManager();
                }
                return instance;
            }
        }

        private Dictionary<EventName, List<Delegate>> eventListeners;

        public EventManager() //constructor to initalize eventListeners dictionary
        {
            eventListeners = new();
        }

        /* Method for add, remove and trigger events
         * Tried to use method overloading to make all of them use the same method but with
         * different returns and parameters
         */

        //uses the Action delegate which does not return a value
        #region ADD/REMOVE/TRIGGER (NO PARAMETERS)
        public void AddListener(EventName eventName, Action listener)
        {
            // If the event does not exist in the dictionary, add it.
            if (!eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName] = new List<Delegate>();
            }

            // Add the listener to the event's list of delegates.
            eventListeners[eventName].Add(listener);
        }

        public void RemoveListener(EventName eventName, Action listener)
        {
            // If the event exists, remove the listener from its list of delegates.
            if (eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName].Remove(listener);
            }
        }
        public void TriggerEvent(EventName eventName)
        {
            // If the event exists, invoke all listeners associated with it.
            if (eventListeners.ContainsKey(eventName))
            {
                var listeners = eventListeners[eventName].ToArray();
                foreach (var listener in listeners)
                {
                    if (listener is Action action)
                    {
                        action.Invoke();
                    }
                }
            }
            else
            {
                
                Debug.LogError("Event not in list");
            }

        }
        #endregion

        //similar to the above but allows passing of parameters
        #region ADD/REMOVE/TRIGGER (W/ PARAM)
        //takes in 1 parameter
        public void AddListener<TParam>(EventName eventName, Action<TParam> listener)
        {
            if (!eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName] = new List<Delegate>();
            }

            eventListeners[eventName].Add(listener);
        }

        public void RemoveListener<TParam>(EventName eventName, Action<TParam> listener)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName].Remove(listener);
            }
        }

        public void TriggerEvent<TParam>(EventName eventName, TParam param)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                var listeners = eventListeners[eventName].ToArray();
                foreach (var listener in listeners)
                {
                    if (listener is Action<TParam> action)
                    {
                        action.Invoke(param);
                    }
                }
            }
        }

        //takes in 2 parameters
        public void AddListener<TParam1, TParam2>(EventName eventName, Action<TParam1, TParam2> listener)
        {
            if (!eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName] = new List<Delegate>();
            }

            eventListeners[eventName].Add(listener);
        }

        public void RemoveListener<TParam1, TParam2>(EventName eventName, Action<TParam1, TParam2> listener)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName].Remove(listener);
            }
        }

        public void TriggerEvent<TParam1, TParam2>(EventName eventName, TParam1 param1, TParam2 param2)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                foreach (var listener in eventListeners[eventName])
                {
                    if (listener is Action<TParam1, TParam2> action)
                    {
                        action.Invoke(param1, param2);
                    }
                }
            }
        }
        #endregion

        //uses the Func delegate which returns a value
        #region ADD/REMOVE/TRIGGER (W/ RETURN)
        public void AddListener<TResult>(EventName eventName, Func<TResult> listener)
        {
            if (!eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName] = new List<Delegate>();
            }

            eventListeners[eventName].Add(listener);
        }

        public void RemoveListener<TResult>(EventName eventName, Func<TResult> listener)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName].Remove(listener);
            }
        }

        public TResult TriggerEvent<TResult>(EventName eventName)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                var listeners = eventListeners[eventName].ToArray();

                foreach (var listener in listeners)
                {
                    if (listener is Func<TResult> function)
                    {
                        return function.Invoke();
                    }
                }
            }

            Debug.Log("returning default...");
            return default(TResult);
        }
        #endregion

        //similar to the above but allows passing of parameters w/ return of a value
        #region ADD/REMOVE/TRIGGER (W/ RETURN + PARAM)
        public void AddListener<TParam, TResult>(EventName eventName, Func<TParam, TResult> listener)
        {
            if (!eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName] = new List<Delegate>();
            }

            eventListeners[eventName].Add(listener);
        }

        public void RemoveListener<TParam, TResult>(EventName eventName, Func<TParam, TResult> listener)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                eventListeners[eventName].Remove(listener);
            }
        }

        public TResult TriggerEvent<TParam, TResult>(EventName eventName, TParam param)
        {
            if (eventListeners.ContainsKey(eventName))
            {
                var listeners = eventListeners[eventName].ToArray();

                foreach (var listener in listeners)
                {
                    if (listener is Func<TParam, TResult> function)
                    {
                        return function.Invoke(param);
                    }
                }
            }

            Debug.Log("returning default...");
            return default(TResult);
        }
        #endregion
    }

    //list of possible events
    public enum EventName
    {
        MAP_NODE_CLICKED,
        TURN_START, //allow all the other things to interact with player
        TURN_COMPLETE, //when every thing is done it will call this event to have a mini time pause for the player.
        TURN_END,//When player ends their turn
        LEVEL_COMPLETED, //when player completes the level

        UseUpgradeStation,
        LeaveUpgradeStation,

    }

    //how the turn trigger
    //Turn_start --> Turn_Complete --> TurnEnd -->repeat again
}

