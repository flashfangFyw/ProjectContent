using UnityEngine;
using System.Collections;
namespace ffDevelopmentSpace
{
    /// <summary>
    ///  说明：每一个action在onExecute的时候，才监听相关Event，否则可能出现多个action操作同一个actor的时候，导致后续的action接收到Event出错
    /// </summary>
    public class BaseAction : EventDispatcher, IAction
    {
        //public  BaseAction()
        //{
        //        //super();
        //}
        public virtual void onExecute() { }

        public virtual void onDispose() { }

        protected virtual void onEnd()
        {
            dispatchEvent(ActionEvent.ACTION_END);
        }

        public virtual bool isEnd()
        {
            return false;
        }
    }
}
