using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using UnityEngine.XR.iOS;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-02-08 15:33:20 
    Desc:       注释 
*/


public class pTouchMove : TouchMove
{

    #region public property
    #endregion
    #region private property
    private Vector3 touchOffset = Vector3.zero;
    private float offsetFactor;
    private OperationController _oC;
    private OperationController oC
    {
        get
        {
            if (_oC == null) _oC = this.GetComponent<OperationController>();
            return _oC;
        }
    }
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start () 
	{
    }   
	//void Update () 
	//{
	//}
    void OnDisable()
    {
    }
    void OnDestroy()
    {
    }
    #endregion

    #region public function
    public void SetOffsetFactor(float v)
    {
        offsetFactor = v;
    }
    #endregion
    #region private function
    protected override void HandleTouchMove()
    {
        TouchMoveType1();
        //TouchMoveType2();
    }

    #endregion
    #region TouchType1
    /// <summary>
    /// 基于Arkit的触碰移动类型
    /// </summary>
    private void TouchMoveType1()
    {
        Touch touch = Input.GetTouch(0);
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
        if (touch.phase == TouchPhase.Began)
        {
            Debug.Log("TouchPhase.Began touchOffset=" + touchOffset);
            foreach (ARHitTestResultType resultType in resultTypes)
            {
                if (hitStartShowPerfab(point, resultType))
                {
                    return;
                }
            }
        }
        if (touch.phase == TouchPhase.Moved)
        {

            foreach (ARHitTestResultType resultType in resultTypes)
            {
                if (hitMoveShowPerfab(point, resultType))
                {
                    return;
                }
            }
        }
        if (touch.phase == TouchPhase.Ended)
        {
        }

    }
    private bool hitStartShowPerfab(ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        if (hitResults.Count > 0)
        {
            Debug.Log("hitResults.Count=" + hitResults.Count);
            foreach (var hitResult in hitResults)
            {
                touchOffset = UnityARMatrixOps.GetPosition(hitResult.worldTransform) - _targetGameObject.transform.position;
                return true;
            }
        }
        return false;
    }
    private bool hitMoveShowPerfab(ARPoint point, ARHitTestResultType resultTypes)
    {
        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
        if (hitResults.Count > 0)
        {
            Debug.Log("hitResults.Count=" + hitResults.Count);
            foreach (var hitResult in hitResults)
            {
                bool moveFlagX = true;
                bool moveFlagY = true;
                Vector3 vposition = UnityARMatrixOps.GetPosition(hitResult.worldTransform) - touchOffset;
                vposition.y = _targetGameObject.transform.position.y;
                Debug.Log("TouchPhase.Moved touchOffset=" + touchOffset);
                Debug.Log("vposition=" + vposition);
                moveFlagX = oC.CheckListDistanceParalle_Z(vposition);
                moveFlagY = oC.CheckListDistanceParalle_X(vposition);
                Vector3 endpostion = vposition;
                //if (!moveFlagX) endpostion.x = _targetGameObject.transform.position.x;
                //if (!moveFlagY) endpostion.z = _targetGameObject.transform.position.z;
                //Debug.Log("moveFlagX=" + moveFlagX+ "    moveFlagY="+ moveFlagY);
                //_targetGameObject.transform.position = endpostion;
                if(moveFlagX&& moveFlagY)
                {
                    _targetGameObject.transform.position = endpostion;
                }
                return true;
            }
        }
        return false;
    }
    #endregion
    #region TouchType2
    /// <summary>
    /// 基于屏幕控制物体移动
    /// </summary>
    private void TouchMoveType2()
    {
        bool moveFlagX = true;
        bool moveFlagY = true;
        Touch touch = Input.GetTouch(0);
        Vector2 deltaPos = touch.deltaPosition;
        //moveFlagX = oC.CheckListDistanceParalle_Z(_targetGameObject.transform.position + oC.showPerfabRight * deltaPos.x * 0.01f);
        //moveFlagY = oC.CheckListDistanceParalle_X(_targetGameObject.transform.position + oC.showPerfabForward * deltaPos.y * 0.01f);
        //if (moveFlagX) _targetGameObject.transform.Translate(oC.showPerfabRight * deltaPos.x * 0.001f, Space.World);
        //if (moveFlagY) _targetGameObject.transform.Translate(oC.showPerfabForward * deltaPos.y * 0.001f, Space.World);
        //=========================
        moveFlagX = oC.CheckListDistanceParalle_Z(_targetGameObject.transform.position + _targetGameObject.transform.right * deltaPos.x * 0.01f);
        moveFlagY = oC.CheckListDistanceParalle_X(_targetGameObject.transform.position + _targetGameObject.transform.forward * deltaPos.y * 0.01f);
        if (moveFlagX) _targetGameObject.transform.Translate(_targetGameObject.transform.right * deltaPos.x * 0.001f, Space.World);
        if (moveFlagY) _targetGameObject.transform.Translate(_targetGameObject.transform.forward * deltaPos.y * 0.001f, Space.World);
    }
    #endregion

    #region event function
    #endregion
}
