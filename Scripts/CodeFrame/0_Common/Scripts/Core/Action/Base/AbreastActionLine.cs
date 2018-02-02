using UnityEngine;
using System.Collections;

#region 并行行为线
#endregion
namespace ffDevelopmentSpace
{
    public class AbreastActionLine : BaseActionLine
    {
        public AbreastActionLine()
            : base()
        {

        }

        override protected void onRun()
        {
            //Debuger.Log("AbreastActionLine onRun===============================");
            //IAction action= m_ActionList[0];
            //action.onExecute();

            int len = m_ActionList.Count;
            for (int i = len - 1; i >= 0; i--)
            {
                IAction action = m_ActionList[i];
                action.onExecute();
            }

        }

        //override protected void onNext()
        //{
        //    onRun();
        //}
    }
}
