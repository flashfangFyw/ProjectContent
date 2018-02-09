using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;


/* 
    Author:     fyw 
    CreateDate: 2018-02-08 14:10:39 
    Desc:       注释 
*/

namespace ffDevelopmentSpace
{


public class TouchMove : TouchActoinBase
    {

        #region private function
        protected override void UpdateTouchInput()
        {
            if (Input.touchCount > 0)
            {
                if (1 == Input.touchCount)
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                    HandleTouchMove();
                }
            }
        }
        protected virtual void HandleTouchMove() { }
        #endregion

    }
}
