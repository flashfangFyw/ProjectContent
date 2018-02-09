using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using UnityEngine.XR.iOS;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-02-08 14:16:42 
    Desc:       基于arkit sdk的触碰检测
*/


public class ArKitHitTest : MonoBehaviour 
{

    #region public property
    //public Transform m_HitTransform;
    public bool hitTest = true;
    public Vector3 targetPosition;
    public Quaternion targetRotation;
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
        if(hitTest) ArkitHitTest();
    }
    void OnDisable()
    {
    }
    void OnDestroy()
    {
    }
    #endregion
    public void ToggleHitTest(bool flag)
    {
        hitTest = flag;
    }
    #region private function
    protected virtual  void ArkitHitTest()
    {
        //Debug.Log("ArkitHitTest");
        if (Input.touchCount > 0 )
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) // || touch.phase == TouchPhase.Moved)
            {
                Debug.Log("ArkitHitTest");
                Vector3 screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                ARPoint point = new ARPoint
                {
                    x = screenPosition.x,
                    y = screenPosition.y
                };
                ARHitTestResultType[] resultTypes = {
                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
                        // if you want to use infinite planes use this:
                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
                        ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
                    };
                foreach (ARHitTestResultType resultType in resultTypes)
                {
                    if (HitTestWithResultType(point, resultType))
                    {
                        HitTestTrueFunction(touch);
                        return;
                    }
                }
            }
        }
    }
    private bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        if (hitResults.Count > 0)
        {
            Debug.Log("hitResults.Count="+ hitResults.Count);
            foreach (var hitResult in hitResults)
            {
                Debug.Log("Got hit!");
                HitResultFunction(hitResult);
                return true;
            }
        }
        return false;
    }
    protected virtual void HitResultFunction(ARHitTestResult hitResult) {}
    protected virtual void HitTestTrueFunction(Touch  touch) { }
    #endregion

    #region event function
    #endregion
}
