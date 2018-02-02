using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using UnityEngine.XR.iOS;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-02-02 10:31:46 
    Desc:      ARkit 触碰 检测   需要 UnityArkit插件
*/


public class TouchHitTest : MonoBehaviour 
{

    #region public property
    public Transform m_HitTransform;
    #endregion
    #region private property
    public float scaleAD = 100.0f;
    private bool putFlag = false;
    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2)  
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start () 
	{
    }
    // Update is called once per frame
    void Update()
    {
        PutHitTest();
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
    private void TouchControl()
    {
        if (putFlag == false) return;
        if (Input.touchCount > 0 )
        {
            Touch touch = Input.GetTouch(0);
            if (1 == Input.touchCount)
            {
                Vector2 deltaPos = touch.deltaPosition;
                transform.Translate(Vector3.left * deltaPos.x, Space.World);
                //transform.Rotate(Vector3.right * deltaPos.y, Space.World);
            }
            if (2 == Input.touchCount)
            {
                Touch newTouch2 = Input.GetTouch(1);
                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = touch;
                    return;
                }
                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(touch.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset / scaleAD;
                Vector3 localScale = transform.localScale;
                Vector3 scale = new Vector3(localScale.x + scaleFactor,
                    localScale.y + scaleFactor,
                    localScale.z + scaleFactor);

                //最小缩放到 0.3 倍  
                if (scale.x > 0.3f && scale.y > 0.3f && scale.z > 0.3f)
                {
                    transform.localScale = scale;
                }

                //记住最新的触摸点，下次使用  
                oldTouch1 = touch;
                oldTouch2 = newTouch2;
            }
        }
    }
    private void PutHitTest()
    {
        if (putFlag == true) return;
        if (Input.touchCount > 0 && m_HitTransform != null)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) // || touch.phase == TouchPhase.Moved)
            {
                var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                ARPoint point = new ARPoint
                {
                    x = screenPosition.x,
                    y = screenPosition.y
                };

                // prioritize reults types
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
                        putFlag = true;
                        CheckAreaField();
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
            foreach (var hitResult in hitResults)
            {
                Debug.Log("Got hit!");
                m_HitTransform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                m_HitTransform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                return true;
            }
        }
        return false;
    }
    private void CheckAreaField()
    {
        Debug.Log("CheckAreaField");
        SingletonMB<ARGeneratePlane>.Instance.GetPlaneEdge();
    }
    #endregion

    #region event function
    #endregion
}
