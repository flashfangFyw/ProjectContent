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

    #region public property
    
    #endregion
	#region private property
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start () 
	{
    }   
	void Update () 
	{
            UpdateTouchInput();
    }
    void OnDisable()
    {
    }
    void OnDestroy()
    {
    }
        #endregion

        #region public function
       
        #endregion
        #region private function
        protected virtual void UpdateTouchInput()
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

        #region event function
        #endregion
    }
}
