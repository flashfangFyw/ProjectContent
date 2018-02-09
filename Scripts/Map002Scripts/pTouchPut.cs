using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using UnityEngine.XR.iOS;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-02-08 15:33:35 
    Desc:       注释 
*/


public class pTouchPut : ArKitHitTest
{
    #region public property
    [HideInInspector]
    public Vector3 showPerfabForward;
    [HideInInspector]
    public Vector3 showPerfabRight;
    #endregion
    #region private property
    private OperationController _oC;
    private OperationController oC
    {
        get
        {
            if (_oC == null) _oC = this.GetComponent<OperationController>();
            return _oC;
        }
    }
    private GameObject showPerfabs;
    private GameObject framePerfabs;
    private float heightOffset;
    private Ray ray;
    private RaycastHit hit;
    private List<Vector3> pointList;
    private List<Vector4> pList;
   
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start()
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
    public  void InitData(GameObject a, GameObject b,float c)
    {
        showPerfabs = a;
        framePerfabs = b;
        heightOffset = c;
    }
    public void LocationTheModel()
    {
        SetVerticeData();
        showPerfabs.transform.position = targetPosition + Vector3.down * heightOffset;
        showPerfabs.transform.rotation = targetRotation;

        Hashtable hash = new Hashtable();
        hash.Add("position", targetPosition);
        hash.Add("time", 1);
        hash.Add("delay", 0.5f);
        iTween.MoveTo(showPerfabs, hash);
        SingletonMB<ARGeneratePlane>.Instance.HidePlane();
    }
    #endregion
    #region private function
    protected override void HitResultFunction(ARHitTestResult hitResult)
    {
        targetPosition = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
        targetRotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
    }
    protected override void HitTestTrueFunction(Touch touch)
    {
        ray = Camera.main.ScreenPointToRay(touch.position);// screenPosition);
        Debug.Log("Raycast=" + Physics.Raycast(ray, out hit, 100));
        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            Debug.Log(" hit.collider.gameObject.name=" + hit.collider.gameObject.name);
            Debug.Log(" hit.collider.gameObject.transform.localScale=" + hit.collider.gameObject.transform.localScale);
            targetPosition = hit.collider.gameObject.transform.position;
            if (framePerfabs)
            {
                framePerfabs.transform.localScale = hit.collider.gameObject.transform.localScale;
                framePerfabs.transform.position = targetPosition;
                framePerfabs.transform.rotation = targetRotation;
            }
        }
        oC.CheckAreaField();
        if (showPerfabs)
        {
            showPerfabs.SetActive(true);
            showPerfabs.BroadcastMessage("InitMap", SendMessageOptions.DontRequireReceiver);
        }
    }
    private void SetVerticeData()
    {
        SetMaterial();
        SetPointInPolygon();
    }
    private void SetMaterial()
    {
        MeshRenderer[] mrs = showPerfabs.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            mr.materials[0].SetInt("_Points_Num", pointList.Count);
            mr.materials[0].SetVectorArray("_Points", pList);
            mr.materials[0].SetFloat("_Points_Bottom", targetPosition.y - 0.1f);
        }
    }
    private void SetPointInPolygon()
    {
        PointInPolygon[] pIps = showPerfabs.GetComponentsInChildren<PointInPolygon>();
        foreach (PointInPolygon p in pIps)
        {
            p.SetBottom(targetPosition.y - 0.1f);
            p.SetPointList(pointList);
        }
    }
    public  void GetVerticesXZ_MaxMin()
    {
        //==========================
        float x_Max = float.MinValue;
        float x_Min = float.MaxValue;
        float z_Max = float.MinValue;
        float z_Min = float.MaxValue;
        Vector3 xMaxzMin_Point = transform.position;
        Vector3 xMinzMin_Point = transform.position;
        Vector3 xMinzMax_Point = transform.position;
        Vector3 xMaxzMax_Point = transform.position;

        Vector3 xMax_Point = transform.position;
        Vector3 xMin_Point = transform.position;
        Vector3 zMax_Point = transform.position;
        Vector3 zMin_Point = transform.position;
        MeshFilter[] filterList = framePerfabs.GetComponents<MeshFilter>();
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
        pointList = new List<Vector3>();
        if (framePerfabs.transform.rotation.eulerAngles.y % 90 == 0)
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
        Vector3 v1 = pointList[0] - pointList[1];
        Vector3 v2 = pointList[1] - pointList[2];
        Vector3 v3 = pointList[2] - pointList[3];
        Vector3 v4 = pointList[3] - pointList[0];
        Vector3 v0 = Vector3.forward;// * showPerfabs.transform.position.z;
        v0 = Quaternion.AngleAxis(targetRotation.eulerAngles.y, Vector3.up) * v0;
        showPerfabForward = v0.normalized;
        showPerfabRight = (Quaternion.AngleAxis(targetRotation.eulerAngles.y, Vector3.up) * Vector3.right).normalized;
        ParalleCheck(v1, v0, pointList[0], pointList[1]);
        ParalleCheck(v2, v0, pointList[1], pointList[2]);
        ParalleCheck(v3, v0, pointList[2], pointList[3]);
        ParalleCheck(v4, v0, pointList[3], pointList[0]);
        pList = new List<Vector4>();
        pList.Add(new Vector4(pointList[0].x, pointList[0].y, pointList[0].z, 0));
        pList.Add(new Vector4(pointList[1].x, pointList[1].y, pointList[1].z, 0));
        pList.Add(new Vector4(pointList[2].x, pointList[2].y, pointList[2].z, 0));
        pList.Add(new Vector4(pointList[3].x, pointList[3].y, pointList[3].z, 0));
        Debug.Log("====================VerticesXZ_MaxMin init Finished");
    }
    private bool ParalleCheck(Vector3 v1, Vector3 v01, Vector3 p1, Vector3 p2)
    {
        if (oC.paralleZList == null) oC.paralleZList = new List<List<Vector3>>();
        if (oC.paralleXList == null) oC.paralleXList = new List<List<Vector3>>();
        if (VectorUtility.IsParallel(v1, v01))
        {

            List<Vector3> p = new List<Vector3>();// { pointList[0], pointList[1] }
            if (VectorUtility.IsParallelAndDirection(v1, v01) == 1)
            {
                p.Add(p1);
                p.Add(p2);
            }
            else
            {
                p.Add(p2);
                p.Add(p1);
            }
            if (oC.paralleZList.Count == 1)
            {
                if (VectorUtility.PointOnLeftSide((oC.paralleZList[0][0] - oC.paralleZList[0][1]), p[0]))
                {
                    oC.paralleZList.Insert(0, p);
                }
                else
                {
                    oC.paralleZList.Add(p);
                }
            }
            else
            {
                oC.paralleZList.Add(p);
            }
            return true;
        }
        else
        {
            List<Vector3> p = new List<Vector3>();// { pointList[0], pointList[1] }
            if (VectorUtility.IsParallelAndDirection(v1, Quaternion.AngleAxis(targetRotation.eulerAngles.y, Vector3.up) * Vector3.right) == 1)
            {
                p.Add(p1);
                p.Add(p2);
            }
            else
            {
                p.Add(p2);
                p.Add(p1);
            }
            if (oC.paralleXList.Count == 1)
            {
                if (VectorUtility.PointOnLeftSide((oC.paralleXList[0][0] - oC.paralleXList[0][1]), p[0]))
                {
                    oC.paralleXList.Insert(0, p);
                }
                else
                {
                    oC.paralleXList.Add(p);
                }
            }
            else
            {
                oC.paralleXList.Add(p);
            }

            return false;
        }
    }
    #endregion

    #region event function
    #endregion
}
