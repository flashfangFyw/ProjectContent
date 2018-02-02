using UnityEngine;
using System.Collections;

#region 顺序行为线
#endregion
namespace ffDevelopmentSpace
{
    public class OrderActionLine : BaseActionLine
    {
        public OrderActionLine() : base()
        {

        }

        protected override void onRun()
        {
            //Debuger.Log("OrderActionLine onRun===============================");
            IAction action = m_ActionList[0];
            action.onExecute();
        }

        override protected void onNext()
        {
            onRun();
        }

    }
}
