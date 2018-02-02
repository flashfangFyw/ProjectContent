using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace ffDevelopmentSpace
{
    public class BaseActionLine : BaseAction, IActionLine
    {
        protected List<IAction> m_ActionList;
        public BaseActionLine()
        {
            m_ActionList = new List<IAction>();
        }

        public void addAction(IAction action)
        {
            if (null == action) return;
            action.addEvent(ActionEvent.ACTION_END, onActionEnd);
            m_ActionList.Add(action);
        }

        private void onActionEnd(EventObject e)
        {
            //ActionEvent e = evt as ActionEvent;
            if (null == e) return;

            IAction action = e.getSender() as IAction;
            if (action != null)
            {
                action.removeEvent(ActionEvent.ACTION_END, onActionEnd);
                action.onDispose();
                removeAction(action);
            }

            if (isEnd())
                onEnd();
            else
                onNext();
        }

        private void removeAction(IAction action)
        {
            if (null == action) return;
            m_ActionList.Remove(action);
        }

        override public bool isEnd()
        {
            int len = m_ActionList.Count;
            if (len <= 0) return true;
            return false;
        }

        protected virtual void onNext()
        {

        }

        override public void onExecute()
        {
            if (isEnd())
            {
                onEnd();
            }
            else
            {
                onRun();
            }
        }

        protected virtual void onRun()
        {

        }

        override public void onDispose()
        {
            while (m_ActionList.Count > 0)
            {
                IAction action = m_ActionList[0];
                action.removeEvent(ActionEvent.ACTION_END, onActionEnd);
                action.onDispose();
                m_ActionList.Remove(action);
            }

        }
    }
}
	
