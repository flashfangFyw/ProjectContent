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
            if (Input.touchCount <= 0) return;
            HandleTouchScale();
        }
        protected virtual void HandleTouchScale() { }
        #endregion

        #region event function
        #endregion
    }
}