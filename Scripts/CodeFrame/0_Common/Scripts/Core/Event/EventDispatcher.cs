using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ffDevelopmentSpace
{


    public class EventDispatcher : IEventDispatcher
    {
        private Dictionary<string, Action<EventObject>> handleList = new Dictionary<string, Action<EventObject>>();

        public void addEvent(string type, Action<EventObject> handle)
        {
            if (handleList.ContainsKey(type))
            {
                handleList[type] += handle;

            }
            else
            {
                handleList[type] = handle;
            }
        }

        public void removeEvent(string type, Action<EventObject> handle)
        {
            if (handleList.ContainsKey(type))
            {
                handleList[type] -= handle;
            }
        }

        public void dispatchEvent(string type, EventObject e = null)
        {
            if (e == null) e = new EventObject();
            if (handleList.ContainsKey(type))
            {
                Action<EventObject> handle = handleList[type];
                if (null != handle)
                {
                    if (null != e) e.setSender(this);
                    handle(e);
                }
            }
        }
        public void dispatchEvent(string type, object obj)
        {
            EventObject e = new EventObject();
            e.obj = obj;
            if (handleList.ContainsKey(type))
            {
                Action<EventObject> handle = handleList[type];
                if (null != handle)
                {
                    if (null != e) e.setSender(this);
                    handle(e);
                }
            }
        }
    }
}

/*	测试例子 需要事件的类可以从此类派生 用法基本同as3 EventDispatcher
 * 	public class XXXClass : EventDispatcher

 	private EventDispatcher dis = new EventDispatcher();
 	dis.addEvent ("aaa", test);

 	EventObject obj = new EventObject();
	obj.obj = "123";
	dis.dispatchEvent("aaa",obj);
	
 	void test(EventObject e)
	{
		Debuger.Log ("测试" + e.sender + e.obj);
		dis.removeEvent ("aaa", test);
	}
	
 * */