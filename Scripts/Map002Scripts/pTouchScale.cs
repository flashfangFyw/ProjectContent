using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;


/* 
    Author:     fyw 
    CreateDate: 2018-02-08 17:19:14 
    Desc:       注释 
*/  


    public class pTouchScale : TouchScale
{

    #region public property
    public float scaleAD = 1000.0f;
    #endregion
    #region private property

    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2)  
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
    #endregion
    #region protected function
    protected override void HandleTouchScale()
    {
        TouchScaleType1();
        //TouchScaleType2();
    }
    #endregion
    private void TouchScaleType1()
    {
                bool scaleFlag = true;
                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);
                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }
                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;
                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset / scaleAD;
                Vector3 localScale = _targetGameObject.transform.localScale;
                Vector3 scale = new Vector3(localScale.x + scaleFactor,
                    localScale.y + scaleFactor,
                    localScale.z + scaleFactor);
                //if (scaleFactor < 0)
                //{
                    scaleFlag = oC.CheckListDistanceParalle_Z(scale.x,_targetGameObject.transform.position);
                    if (scaleFlag) scaleFlag = oC.CheckListDistanceParalle_X(scale.z, _targetGameObject.transform.position);
                //}
                if (scaleFlag)
                {
                    if (scale.x < 2.0f)
                    {
                        _targetGameObject.transform.localScale = scale;
                    }
                }
                //记住最新的触摸点，下次使用  
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
    private void TouchScaleType2()
    {
    }

    #region event function
    #endregion
}
