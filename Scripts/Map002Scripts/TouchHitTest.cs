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
    public bool ifTest = false;
    public float offsetFactor=1;
    public Transform m_HitTransform;
    public GameObject[] poinPerfabs;
    public Material testMeterial;
    public GameObject showPerfabs;
    public GameObject FramePerfabs;
    //public GameObject showFrame;
    //public PointInPolygon pp;
    public bool moveFlagX = true;
    public bool moveFlagY = true;
    public bool scaleFlag = true;
    public GameObject map;
    public int upHeight = 1;
    #endregion
    #region private property
    public float scaleAD = 1000.0f;
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
        if(ifTest) CheckAreaField();
    }
    // Update is called once per frame
    void Update()
    {
        PutHitTest();
        //ControlCheck();
        TouchControl();
        KeyControl();
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
    private void ControlCheck()
    {
        //Debug.Log("================================Camera.main.transform.rotation=============" + Camera.main.transform.rotation.eulerAngles);
       
        //Vector3 pt = showPerfabs.transform.position;
        //if (Util.PointInPolygon(pt, pointList))
        //{
        //    moveFlag = true;
        //    scaleFlag = true;
        //}
        //else
        //{
        //    moveFlag = false;
        //    scaleFlag = false;
        //}
    }
    private void KeyControl()
    {
        Vector3 deltaPos = Vector3.zero;
        bool flag = true;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            deltaPos = Vector3.right;
            foreach(var v in paralleYlList)
            {
                if (showPerfabs.transform.localScale.x* offsetFactor / 2 <
                GeometryTools.DisPoint2Line(showPerfabs.transform.position + Vector3.right * deltaPos.x * 0.01f, v[0], v[1])
                   )
                {
                    flag = false;
                    break;
                }
            }
            //if(flag) showPerfabs.transform.Translate(Vector3.right * 0.01f, Space.World);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            deltaPos = Vector3.left;
            foreach (var v in paralleYlList)
            {
                if (showPerfabs.transform.localScale.x * offsetFactor / 2 <
                GeometryTools.DisPoint2Line(showPerfabs.transform.position + Vector3.right * deltaPos.x * 0.01f, v[0], v[1])
                   )
                {
                    flag = false;
                    break;
                }
            }
        }
       


        if (Input.GetKey(KeyCode.UpArrow))
        {
            deltaPos = Vector3.forward;
            foreach (var v in paralleXlList)
            {
                if (showPerfabs.transform.localScale.z * offsetFactor / 2 <
                GeometryTools.DisPoint2Line(showPerfabs.transform.position + Vector3.forward * deltaPos.z * 0.01f, v[0], v[1])
                   )
                {
                    flag = false;
                    break;
                }
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            deltaPos = Vector3.back;
            foreach (var v in paralleXlList)
            {
                if (showPerfabs.transform.localScale.z * offsetFactor / 2 <
                GeometryTools.DisPoint2Line(showPerfabs.transform.position + Vector3.forward * deltaPos.z * 0.01f, v[0], v[1])
                   )
                {
                    flag = false;
                    break;
                }
            }
        }
        //if (flag) showPerfabs.transform.Translate(Vector3.right * deltaPos.x * 0.001f, Space.World);
        //if (flag) showPerfabs.transform.Translate(Vector3.forward * deltaPos.z * 0.001f, Space.World);
        if(flag) showPerfabs.transform.Translate((paralleXlList[0][0]- paralleXlList[0][1]).normalized * deltaPos.x * 0.001f, Space.World);
        if (flag) showPerfabs.transform.Translate((paralleYlList[0][0]- paralleYlList[0][1]).normalized * deltaPos.z * 0.001f, Space.World);
        //=================================
        if (Input.GetKey(KeyCode.Q))
        {
            // 向量v0沿着Y轴旋转45度得到v1
            Vector3 v0 = Vector3.forward * showPerfabs.transform.position.z;
            Vector3 v1 = Quaternion.AngleAxis(showPerfabs.transform.rotation.eulerAngles.y, Vector3.up) * v0;
            showPerfabs.transform.Rotate(v1);
          
        }
        if (Input.GetKey(KeyCode.S))
        {
            //两个距离之差，为正表示放大手势， 为负表示缩小手势  
            float offset = -1;

            //放大因子， 一个像素按 0.01倍来算(100可调整)  
            float scaleFactor = offset / scaleAD;
            Vector3 localScale = showPerfabs.transform.localScale;
            Vector3 scale = new Vector3(localScale.x + scaleFactor,
                localScale.y + scaleFactor,
                localScale.z + scaleFactor);

            foreach (var v in paralleYlList)
            {
                if (showPerfabs.transform.localScale.x * offsetFactor / 2 <
                GeometryTools.DisPoint2Line(showPerfabs.transform.position, v[0], v[1])
                   )
                {
                    flag = false;
                    break;
                }
            }
            foreach (var v in paralleXlList)
            {
                if (showPerfabs.transform.localScale.z * offsetFactor / 2 <
                GeometryTools.DisPoint2Line(showPerfabs.transform.position, v[0], v[1])
                   )
                {
                    flag = false;
                    break;
                }
            }
            if (flag) showPerfabs.transform.localScale = scale;
            ////最小缩放到 0.1 倍  
            //if (scale.x > 0.1f && scale.y > 0.1f && scale.z > 0.1f)
            //{
            //    showPerfabs.transform.localScale = scale;
            //}
        }

    }
        private void TouchControl()
    {
        if (putFlag == false) return;
        Vector3 pt = showPerfabs.transform.position;
        if (Input.touchCount > 0 )
        {
            if (1 == Input.touchCount )
            {
                Touch touch = Input.GetTouch(0);
                Vector2 deltaPos = touch.deltaPosition;
                //=============================================
                foreach (var v in paralleYlList)
                {
                    if (showPerfabs.transform.localScale.x * 3 / 2 <
                    GeometryTools.DisPoint2Line(showPerfabs.transform.position + Vector3.right * deltaPos.x * 0.01f, v[0], v[1])
                       )
                    {
                        moveFlagX = false;
                        break;
                    }
                }
                foreach (var v in paralleXlList)
                {
                    if (showPerfabs.transform.localScale.z * 3 / 2 <
                    GeometryTools.DisPoint2Line(showPerfabs.transform.position + Vector3.forward * deltaPos.y * 0.01f, v[0], v[1])
                       )
                    {
                        moveFlagY = false;
                        break;
                    }
                }
                if (moveFlagX) showPerfabs.transform.Translate(Vector3.right * deltaPos.x * 0.001f, Space.World);
                if (moveFlagY) showPerfabs.transform.Translate(Vector3.forward * deltaPos.y * 0.001f, Space.World);
                //=============================================
                //Debug.Log("deltaPos=" + touch.deltaPosition);
                //pt = showPerfabs.transform.position + (Vector3.right * Mathf.Sin(Camera.main.transform.rotation.eulerAngles.y) + Vector3.forward * Mathf.Cos(Camera.main.transform.rotation.eulerAngles.y))
                //                            * deltaPos.x * 0.001f;
                //if (Util.PointInPolygon(pt, pointList))
                //{
                //    showPerfabs.transform.Translate(
                //                          (Vector3.right * Mathf.Sin(Camera.main.transform.rotation.y) + Vector3.forward * Mathf.Cos(Camera.main.transform.rotation.y))
                //                          * deltaPos.x * 0.001f, Space.World);
                //}
            //================
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
            Vector3 localScale = showPerfabs.transform.localScale;
            Vector3 scale = new Vector3(localScale.x + scaleFactor,
                localScale.y + scaleFactor,
                localScale.z + scaleFactor);

            foreach (var v in paralleYlList)
            {
                if (showPerfabs.transform.localScale.x*3 / 2 <
                GeometryTools.DisPoint2Line(showPerfabs.transform.position, v[0], v[1])
                   )
                {
                    scaleFlag = false;
                    break;
                }
            }
            foreach (var v in paralleXlList)
            {
                if (showPerfabs.transform.localScale.z *3/ 2 <
                GeometryTools.DisPoint2Line(showPerfabs.transform.position, v[0], v[1])
                   )
                {
                    scaleFlag = false;
                    break;
                }
            }
            if (scaleFlag) showPerfabs.transform.localScale = scale;

            ////最小缩放到 0.1 倍  
            //if (scale.x > 0.1f && scale.y > 0.1f && scale.z > 0.1f)
            //{
            //    showPerfabs.transform.localScale = scale;
            //}

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
                Vector3 screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
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
                        ray = Camera.main.ScreenPointToRay(touch.position);// screenPosition);
                        Debug.Log("Raycast=" + Physics.Raycast(ray, out hit, 100));
                        if (Physics.Raycast(ray, out hit, 100))
                        {
                            Debug.DrawLine(ray.origin, hit.point, Color.green);
                            Debug.Log(" hit.collider.gameObject.name=" + hit.collider.gameObject.name);
                            Debug.Log(" hit.collider.gameObject.transform.localScale=" + hit.collider.gameObject.transform.localScale);
                            targetPosition = hit.collider.gameObject.transform.position;
                            //targetRotation = hit.collider.gameObject.transform.rotation;
                            FramePerfabs.transform.localScale = hit.collider.gameObject.transform.localScale;
                            FramePerfabs.transform.position = targetPosition;
                            FramePerfabs.transform.rotation = targetRotation;
                            //    CheckAreaField(hit.transform);
                            //    putFlag = true;
                        }
                        CheckAreaField();
                        if (showPerfabs)
                        {
                            showPerfabs.SetActive(true);
                            showPerfabs.BroadcastMessage("InitMap",SendMessageOptions.DontRequireReceiver);
                        }
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
                //targetPosition = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                targetRotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
                //m_HitTransform.position = targetPosition;
                //m_HitTransform.rotation = targetRotation;
                //FramePerfabs.transform.position = targetPosition;
                // FramePerfabs.transform.rotation = targetRotation;
                Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
                return true;
            }
        }
        return false;
    }
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    public void LoactionTheModel()
    {
        SetVerticeData();
        showPerfabs.transform.position = targetPosition+Vector3.down* upHeight;
        showPerfabs.transform.rotation = targetRotation;

        Hashtable hash = new Hashtable();
        hash.Add("position", targetPosition);
        hash.Add("time", 1);
        hash.Add("delay",0.5f);
        iTween.MoveTo(showPerfabs, hash);
        SingletonMB<ARGeneratePlane>.Instance.HidePlane();
    }
    public Vector3 GetOffsetPosition()
    {
        return targetPosition;
    }
    private void CheckAreaField()
    {
        Debug.Log("CheckAreaField");
        //if (tf == null) return;
        GeetVerticesXZ_MaxMin();
        if(poinPerfabs!=null)
        {
            for(int i=0;i< poinPerfabs.Length;i++)
            {
                GameObject point = Instantiate(poinPerfabs[i]);
                point.transform.position = pointList[i];
               // point.transform.SetParent(this.transform);
            }
        }
        //SingletonMB<ARGeneratePlane>.Instance.GetPlaneEdge();
    }
    //获取顶点最大，最小值
    private float x_Max;
    private float x_Min;
    private float z_Max;
    private float z_Min;
    //==========================
    private Vector3 xMaxzMin_Point;
    private Vector3 xMinzMin_Point;
    private Vector3 xMinzMax_Point;
    private Vector3 xMaxzMax_Point;
    private Vector3 xMax_Point ;
    private Vector3 xMin_Point ;
    private Vector3 zMax_Point ;
    private Vector3 zMin_Point ;
    private List<Vector3> pointList;
    private List<Vector4> pList;
    private Vector3 v0;
    private Vector3 v1;
    private Vector3 v2;
    private Vector3 v3;
    private Vector3 v4;
    private List<List<Vector3>> paralleYlList;
    private List<List<Vector3>> paralleXlList;


    protected void GeetVerticesXZ_MaxMin()
    {
        x_Max = float.MinValue;
        x_Min = float.MaxValue;
        z_Max = float.MinValue;
        z_Min = float.MaxValue;
        xMaxzMin_Point = transform.position;
        xMinzMin_Point = transform.position;
        xMinzMax_Point = transform.position;
        xMaxzMax_Point = transform.position;

        xMax_Point = transform.position;
        xMin_Point = transform.position;
        zMax_Point = transform.position;
        zMin_Point = transform.position;
        MeshFilter[] filterList = FramePerfabs.GetComponents<MeshFilter>();
        foreach (MeshFilter filter in filterList)
        {
            Mesh mesh = filter.mesh;
            Vector3[] vertices = mesh.vertices;
            int i = 0;
            Vector3 vertPos;
            foreach (Vector3 vertice in vertices)
            {
                //Debug.Log("I==" + vertices.Length);
                vertPos = filter.transform.TransformPoint(vertice);
                //vertPos = vertice;
                if (vertPos.x < x_Min)
                {
                    x_Min = vertPos.x;
                    if (vertPos.z > xMinzMax_Point.z) xMinzMax_Point = vertPos;
                    if (vertPos.z < xMinzMin_Point.z) xMinzMin_Point = vertPos;
                    xMin_Point = vertPos;
                    //Debug.Log("=======xMax_Point==" + xMax_Point + "=======xMin_Point==" + xMin_Point + "=======zMax_Point==" + zMax_Point + "=======zMin_Point==" + zMin_Point);
                }
                if (vertPos.x > x_Max)
                {
                    x_Max = vertPos.x;
                    if (vertPos.z > xMaxzMax_Point.z) xMaxzMax_Point = vertPos;
                    if (vertPos.z < xMaxzMin_Point.z) xMaxzMin_Point = vertPos;
                    xMax_Point = vertPos;
                    //Debug.Log("=======xMax_Point==" + xMax_Point + "=======xMin_Point==" + xMin_Point + "=======zMax_Point==" + zMax_Point + "=======zMin_Point==" + zMin_Point);
                }
                if (vertPos.z < z_Min)
                {
                    z_Min = vertPos.z;
                    if (vertPos.x > xMaxzMin_Point.x) xMaxzMin_Point = vertPos;
                    if (vertPos.x < xMinzMin_Point.x) xMinzMin_Point = vertPos;
                    zMin_Point = vertPos;
                    //Debug.Log("=======xMax_Point==" + xMax_Point + "=======xMin_Point==" + xMin_Point + "=======zMax_Point==" + zMax_Point + "=======zMin_Point==" + zMin_Point);
                }
                if (vertPos.z > z_Max)
                {
                    z_Max = vertPos.z;
                    if (vertPos.x > xMaxzMax_Point.x) xMaxzMax_Point = vertPos;
                    if (vertPos.x < xMinzMax_Point.x) xMinzMax_Point = vertPos;
                    zMax_Point = vertPos;
                    //Debug.Log("=======xMax_Point==" + xMax_Point + "=======xMin_Point==" + xMin_Point + "=======zMax_Point==" + zMax_Point + "=======zMin_Point==" + zMin_Point);
                }

                i++;
            }
        }
        //Vector4.op
        pointList = new List<Vector3>();


        //if(Mathf.Abs(Mathf.))
        if (FramePerfabs.transform.rotation.y % 90 == 0)
        {
            xMaxzMin_Point = new Vector3(x_Max, transform.position.y, z_Min);
            xMinzMin_Point = new Vector3(x_Min, transform.position.y, z_Min);
            xMinzMax_Point = new Vector3(x_Min, transform.position.y, z_Max);
            xMaxzMax_Point = new Vector3(x_Max, transform.position.y, z_Max);
            pointList.Add(xMaxzMin_Point);
            pointList.Add(xMinzMin_Point);
            pointList.Add(xMinzMax_Point);
            pointList.Add(xMaxzMax_Point);
        }
        else
        {
            pointList.Add(xMax_Point);
            pointList.Add(zMin_Point);
            pointList.Add(xMin_Point);
            pointList.Add(zMax_Point);
        }
        v1 = pointList[0] - pointList[1];
        v2 = pointList[1] - pointList[2];
        v3 = pointList[2] - pointList[3];
        v4 = pointList[3] - pointList[0];
        v0 = Vector3.forward;// * showPerfabs.transform.position.z;
        v0 = Quaternion.AngleAxis(showPerfabs.transform.rotation.eulerAngles.y, Vector3.up) * v0;
        //paralleYlList = new List<Vector3>();
        //paralleXlList = new List<Vector3>();
        ParalleCheck(v1, v0, new List<Vector3> { pointList[0], pointList[1]});
        ParalleCheck(v2, v0, new List<Vector3> { pointList[1], pointList[2]});
        ParalleCheck(v3, v0, new List<Vector3> { pointList[2], pointList[3]});
        ParalleCheck(v4, v0, new List<Vector3> { pointList[3], pointList[0]});

        //pp.SetPointList( pointList);
        pList = new List<Vector4>();
        //pList.Add(new Vector4(x_Max, transform.position.y, z_Min, 0));
        //pList.Add(new Vector4(x_Min, transform.position.y, z_Min, 0));
        //pList.Add(new Vector4(x_Min, transform.position.y, z_Max, 0));
        //pList.Add(new Vector4(x_Max, transform.position.y, z_Max, 0));
        pList.Add(new Vector4(pointList[0].x, pointList[0].y, pointList[0].z, 0));
        pList.Add(new Vector4(pointList[1].x, pointList[1].y, pointList[1].z, 0));
        pList.Add(new Vector4(pointList[2].x, pointList[2].y, pointList[2].z, 0));
        pList.Add(new Vector4(pointList[3].x, pointList[3].y, pointList[3].z, 0));
        Debug.Log("====================VerticesXZ_MaxMin init Finished");

        //modelHeighth = maxValue - minValue;
        //float disValue = (maxValue - minValue) / 50;
        //maxValue += disValue;
        //minValue -= disValue;
        //SetMaterial();
        //showPerfabs.GetComponent<MeshRenderer>().materials[0].SetInt("_Points_Num", pointList.Count);
        //showPerfabs.GetComponent<MeshRenderer>().materials[0].SetVectorArray("_Points", pList);
        //Debug.Log("=======xMax_Point==" + xMax_Point);
        //Debug.Log("=======xMin_Point==" + xMin_Point);
        //Debug.Log("=======zMax_Point==" + zMax_Point);
        //Debug.Log("=======zMin_Point==" + zMin_Point);
        //textureMaterial.SetFloat("EffectTime", maxValue);
        //textureMaterial.SetFloat("BottomValue", minValue);
        //Debug.Log("====================maxValue==" + maxValue + "     minValue==" + minValue + "     modelHeighth==" + modelHeighth);
    }
    private bool ParalleCheck(Vector3 v1,Vector3 v01, List<Vector3> p)
    {
        if(GeometryTools.IsParallel(v1, v01))
        {
            if (paralleYlList == null) paralleYlList = new List<List<Vector3>>();
            paralleYlList.Add(p);
            return true;
        }
        else
        {
            if(paralleXlList==null ) paralleXlList = new List<List<Vector3>>();
            paralleXlList.Add(p);
            return false;
        }
    }
    private void SetVerticeData()
    {
        SetMaterial();
        SetPointInPolygon();
    }
    private  void SetMaterial()
    {
        MeshRenderer[] mrs = showPerfabs.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            //if (mr.materials[0].HasProperty("_Points_Num"))
            //{
            mr.materials[0].SetInt("_Points_Num", pointList.Count);
            //Debug.Log("====================_Points_Num Finished");
            //}
            //if (mr.materials[0].HasProperty("_Points"))
            //{
            mr.materials[0].SetVectorArray("_Points", pList);
            mr.materials[0].SetFloat("_Points_Bottom", targetPosition.y-0.01f);
            //    Debug.Log("====================SetVectorArray Finished");
            //}
        }
        //Debug.Log("====================SetMaterial Finished");
    }
    public void SetPointInPolygon()
    {
        PointInPolygon[] pIps = showPerfabs.GetComponentsInChildren<PointInPolygon>();
        foreach (PointInPolygon p in pIps)
        {
            p.SetBottom(targetPosition.y - 0.01f);
            p.SetPointList(pointList);
        }
    }
    public List<Vector3> GetPointList()
    {
        return pointList;
    }
    public List<Vector4> GetPList()
    {
        return pList;
    }
    #endregion

    #region event function
    #endregion
}
