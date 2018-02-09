using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;


/* 
    Author:     fyw 
    CreateDate: 2018-02-08 14:11:41 
    Desc:       注释 
*/
namespace ffDevelopmentSpace
{

    public class TouchScale : TouchActoinBase
    {
        #region private function
        protected override void UpdateTouchInput()
        {
            if (Input.touchCount <= 0) return;
            HandleTouchScale();
        }
        protected virtual void HandleTouchScale() { }
        #endregion

      
    }
}