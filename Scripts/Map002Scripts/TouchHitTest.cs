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
    public GameObject[] poinPerfabs;
    public Material testMeterial;
    public GameObject cube;
    #endregion
    #region private property
    public float scaleAD = 100.0f;
    private bool putFlag = false;
    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2)  
    private Ray ray;
    RaycastHit hit;
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
        TouchControl();
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
            if (1 == Input.touchCount)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 deltaPos = touch.deltaPosition;
                Debug.Log("deltaPos=" + touch.deltaPosition);
                transform.Translate(Vector3.forward * deltaPos.x*0.001f, Space.World);
                //transform.Rotate(Vector3.right * deltaPos.y, Space.World);
            }

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
            oldTouch1 = newTouch1;
            oldTouch2 = newTouch2;

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

                        //ray = Camera.main.ScreenPointToRay(touch.position);// screenPosition);
                        ray = Camera.main.ScreenPointToRay(screenPosition);// screenPosition);

                        Debug.Log("Raycast=" + Physics.Raycast(ray, out hit, 100));
                        if (Physics.Raycast(ray, out hit, 100))
                        {
                            Debug.DrawLine(ray.origin, hit.point, Color.green);
                            //    CheckAreaField(hit.transform);
                            //    putFlag = true;
                        }
                        CheckAreaField(cube.transform);
                        putFlag = true;
                        Debug.Log("screenPosition=" + screenPosition  + "  hit="+ hit);
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
    private void CheckAreaField(Transform tf)
    {
        Debug.Log("CheckAreaField");
        if (tf == null) return;
        GeetVerticesXZ_MaxMin(tf);
        if(poinPerfabs!=null)
        {
            for(int i=0;i< poinPerfabs.Length;i++)
            {
                GameObject point = Instantiate(poinPerfabs[i]);
                point.transform.position = pointList[i];
                point.transform.SetParent(this.transform);
            }
        }
        SingletonMB<ARGeneratePlane>.Instance.GetPlaneEdge();
    }
    //获取顶点最大，最小值
    private float x_Max;
    private float x_Min;
    private float z_Max;
    private float z_Min;
    //==========================
    private Vector3 xMax_Point;
    private Vector3 xMin_Point;
    private Vector3 zMax_Point;
    private Vector3 zMin_Point;
    private List<Vector3> pointList;
    private List<Vector4> pList;


    protected  void GeetVerticesXZ_MaxMin(Transform tf)
    {
        x_Max = float.MinValue;
        x_Min = float.MaxValue;
        z_Max = float.MinValue;
        z_Min = float.MaxValue;
        xMax_Point = xMin_Point = zMax_Point = zMin_Point = transform.position;
        MeshFilter[] filterList = tf.GetComponents<MeshFilter>();
        foreach (MeshFilter filter in filterList)
        {
            Mesh mesh = filter.mesh;
            Vector3[] vertices = mesh.vertices;
            int i = 0;
            Vector3 vertPos;
            foreach (Vector3 vertice in vertices)
            {
                //Debug.Log("I==" + i);
                vertPos = filter.transform.TransformPoint(vertice);
                if (vertPos.x < x_Min) x_Min = vertPos.x; xMin_Point = vertPos;
                if (vertPos.x > x_Max) x_Max = vertPos.x; xMax_Point = vertPos;
                if (vertPos.z < z_Min) z_Min = vertPos.z; zMin_Point = vertPos;
                if (vertPos.z > z_Max) z_Max = vertPos.z; zMax_Point = vertPos;
                i++;
            }
        }
        //Vector4.op
        pointList = new List<Vector3>();
        pointList.Add(xMax_Point);
        pointList.Add(xMin_Point);
        pointList.Add(zMax_Point);
        pointList.Add(zMin_Point);
        pList = new List<Vector4>();
        pList.Add(new Vector4(xMax_Point.x, xMax_Point.y, xMax_Point.z,0));
        pList.Add(new Vector4(xMin_Point.x, xMin_Point.y, xMin_Point.z, 0));
        pList.Add(new Vector4(zMax_Point.x, zMax_Point.y, zMax_Point.z, 0));
        pList.Add(new Vector4(zMin_Point.x, zMin_Point.y, zMin_Point.z, 0));

        //modelHeighth = maxValue - minValue;
        //float disValue = (maxValue - minValue) / 50;
        //maxValue += disValue;
        //minValue -= disValue;
        cube.GetComponent<MeshRenderer>().materials[0].SetInt("_Points_Num", pointList.Count);
        cube.GetComponent<MeshRenderer>().materials[0].SetVectorArray("_Points", pList);
        Debug.Log("=======xMax_Point==" + xMax_Point);
        Debug.Log("=======xMin_Point==" + xMin_Point);
        Debug.Log("=======zMax_Point==" + zMax_Point);
        Debug.Log("=======zMin_Point==" + zMin_Point);
        //textureMaterial.SetFloat("EffectTime", maxValue);
        //textureMaterial.SetFloat("BottomValue", minValue);
        //Debug.Log("====================maxValue==" + maxValue + "     minValue==" + minValue + "     modelHeighth==" + modelHeighth);
    }
    public static bool Contains(Vector3[] points, Vector3 p)
    {
        bool result = false;
        for (int i = 0; i < points.Length - 1; i++)
        {
            if ( (((points[i + 1].z <= p.z) && (p.z < points[i].z))
                        ||
                         ((points[i].z <= p.z) && (p.z < points[i + 1].z)))
                          &&
                        (p.x < (points[i].x - points[i + 1].x) * (p.z - points[i + 1].z) / (points[i].z - points[i + 1].z) + points[i + 1].x)
                        )
                {
                result = !result;
            }

        }
        return result;
    }
    #endregion

    #region event function
    #endregion
}
