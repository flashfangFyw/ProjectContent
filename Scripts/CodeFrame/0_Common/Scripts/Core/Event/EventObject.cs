using UnityEngine;
using System.Collections;
using System;
namespace ffDevelopmentSpace
{
    public class EventObject : EventArgs
    {
        private object sender;
        public object obj;

        public object getSender()
        {
            return sender;
        }

        public void setSender(object obj)
        {
            sender = obj;
        }
    }
}
