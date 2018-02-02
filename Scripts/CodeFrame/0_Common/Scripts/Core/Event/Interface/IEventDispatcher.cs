using UnityEngine;
using System.Collections;
using System;
namespace ffDevelopmentSpace
{
    public interface IEventDispatcher
    {
        void addEvent(string type, Action<EventObject> handle);
        void removeEvent(string type, Action<EventObject> handle);
        void dispatchEvent(string type, EventObject e = null);
        void dispatchEvent(string type, object obj);
    }
}
