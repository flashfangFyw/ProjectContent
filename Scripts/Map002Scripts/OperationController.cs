using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-02-08 14:06:16 
    Desc:       注释 
*/


public class OperationController : SingletonMB<OperationController> 
{

    #region public property
    public bool ifTest = false;
    public bool ifMove = true;
    public bool ifScale = true;
    public float offsetFactor = 1;
    public float offsetHeight = 1;
    //public Transform m_HitTransform;
  
    //public GameObject[] poinPerfabs;
    //public Material testMeterial;
    public GameObject showPerfabs;
    public GameObject framePerfabs;
    #endregion
    #region private property
    private pTouchPut _touchPut;
    private pTouchMove _touchMove;
    private pTouchScale _touchScale;
    //private bool putFlag=false;
    [HideInInspector]
    public  List<List<Vector3>> paralleZList;
    [HideInInspector]
    public List<List<Vector3>> paralleXList;
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start () 
	{
        _touchPut = gameObject.AddComponent<pTouchPut>();
        _touchPut.InitData(showPerfabs, framePerfabs, offsetHeight);
        
        _touchPut.enabled = true;
       
    }   
    void OnDisable()
    {
    }
    void OnDestroy()
    {
    }
    #endregion

    #region public function
    public void ToggleHitTestFlag(bool flag)
    {
        if(flag==false)
        {
            if (ifMove)
            {
                _touchMove = gameObject.AddComponent<pTouchMove>();
                _touchMove.SetTargetGameObject(showPerfabs);
                //_touchMove.SetOffsetFactor(offsetFactor);
            }
            if (ifScale)
            {
                _touchScale = gameObject.AddComponent<pTouchScale>();
                _touchScale.SetTargetGameObject(showPerfabs);
            }
        }
        else
        {

        }
    }
    public void PutTheModel()
    {
        _touchPut.LocationTheModel();
    }
    public  void CheckAreaField()
    {
        Debug.Log("CheckAreaField");
        if (_touchPut) _touchPut.GetVerticesXZ_MaxMin();
        //GeetVerticesXZ_MaxMin();
    }
    public  bool CheckListDistanceParalle_Z(Vector3 point)
    {
       
        foreach (var v in paralleZList)
        {
            float distance = VectorUtility.DisPoint2Line(point, v[0], v[1]);
            if (showPerfabs.transform.localScale.x * offsetFactor / 2 <=distance)
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckListDistanceParalle_Z(float scale,Vector3 point)
    {
        foreach (var v in paralleZList)
        {
            float distance = VectorUtility.DisPoint2Line(point, v[0], v[1]);
            if (scale * offsetFactor / 2 <= distance)
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckListDistanceParalle_X(Vector3 point)
    {
        foreach (var v in paralleXList)
        {
            float distance = VectorUtility.DisPoint2Line(point, v[0], v[1]);
            if (showPerfabs.transform.localScale.z * offsetFactor / 2 <= distance)
            {
                return false;
            }
        }
        return true;
    }
    public bool CheckListDistanceParalle_X(float scale, Vector3 point)
    {
        foreach (var v in paralleXList)
        {
            float distance = VectorUtility.DisPoint2Line(point, v[0], v[1]);
            if (scale * offsetFactor / 2 <= distance)
            {
                return false;
            }
        }
        return true;
    }
    #endregion
    #region Vector function
  
   
    //protected void GeetVerticesXZ_MaxMin()
    //{
    //    //==========================
    //    float x_Max = float.MinValue;
    //    float x_Min = float.MaxValue;
    //    float z_Max = float.MinValue;
    //    float z_Min = float.MaxValue;
    //    Vector3 xMaxzMin_Point = transform.position;
    //    Vector3 xMinzMin_Point = transform.position;
    //    Vector3 xMinzMax_Point = transform.position;
    //    Vector3 xMaxzMax_Point = transform.position;

    //    Vector3 xMax_Point = transform.position;
    //    Vector3 xMin_Point = transform.position;
    //    Vector3 zMax_Point = transform.position;
    //    Vector3 zMin_Point = transform.position;
    //    MeshFilter[] filterList = framePerfabs.GetComponents<MeshFilter>();
    //    foreach (MeshFilter filter in filterList)
    //    {
    //        Mesh mesh = filter.mesh;
    //        Vector3[] vertices = mesh.vertices;
    //        int i = 0;
    //        Vector3 vertPos;
    //        foreach (Vector3 vertice in vertices)
    //        {
    //            //Debug.Log("I==" + vertices.Length);
    //            vertPos = filter.transform.TransformPoint(vertice);
    //            //vertPos = vertice;
    //            if (vertPos.x < x_Min)
    //            {
    //                x_Min = vertPos.x;
    //                if (vertPos.z > xMinzMax_Point.z) xMinzMax_Point = vertPos;
    //                if (vertPos.z < xMinzMin_Point.z) xMinzMin_Point = vertPos;
    //                xMin_Point = vertPos;
    //                //Debug.Log("=======xMax_Point==" + xMax_Point + "=======xMin_Point==" + xMin_Point + "=======zMax_Point==" + zMax_Point + "=======zMin_Point==" + zMin_Point);
    //            }
    //            if (vertPos.x > x_Max)
    //            {
    //                x_Max = vertPos.x;
    //                if (vertPos.z > xMaxzMax_Point.z) xMaxzMax_Point = vertPos;
    //                if (vertPos.z < xMaxzMin_Point.z) xMaxzMin_Point = vertPos;
    //                xMax_Point = vertPos;
    //                //Debug.Log("=======xMax_Point==" + xMax_Point + "=======xMin_Point==" + xMin_Point + "=======zMax_Point==" + zMax_Point + "=======zMin_Point==" + zMin_Point);
    //            }
    //            if (vertPos.z < z_Min)
    //            {
    //                z_Min = vertPos.z;
    //                if (vertPos.x > xMaxzMin_Point.x) xMaxzMin_Point = vertPos;
    //                if (vertPos.x < xMinzMin_Point.x) xMinzMin_Point = vertPos;
    //                zMin_Point = vertPos;
    //                //Debug.Log("=======xMax_Point==" + xMax_Point + "=======xMin_Point==" + xMin_Point + "=======zMax_Point==" + zMax_Point + "=======zMin_Point==" + zMin_Point);
    //            }
    //            if (vertPos.z > z_Max)
    //            {
    //                z_Max = vertPos.z;
    //                if (vertPos.x > xMaxzMax_Point.x) xMaxzMax_Point = vertPos;
    //                if (vertPos.x < xMinzMax_Point.x) xMinzMax_Point = vertPos;
    //                zMax_Point = vertPos;
    //                //Debug.Log("=======xMax_Point==" + xMax_Point + "=======xMin_Point==" + xMin_Point + "=======zMax_Point==" + zMax_Point + "=======zMin_Point==" + zMin_Point);
    //            }

    //            i++;
    //        }
    //    }
    //    pointList = new List<Vector3>();
    //    if (framePerfabs.transform.rotation.eulerAngles.y % 90 == 0)
    //    {
    //        xMaxzMin_Point = new Vector3(x_Max, transform.position.y, z_Min);
    //        xMinzMin_Point = new Vector3(x_Min, transform.position.y, z_Min);
    //        xMinzMax_Point = new Vector3(x_Min, transform.position.y, z_Max);
    //        xMaxzMax_Point = new Vector3(x_Max, transform.position.y, z_Max);
    //        pointList.Add(xMaxzMin_Point);
    //        pointList.Add(xMinzMin_Point);
    //        pointList.Add(xMinzMax_Point);
    //        pointList.Add(xMaxzMax_Point);
    //    }
    //    else
    //    {
    //        pointList.Add(xMax_Point);
    //        pointList.Add(zMin_Point);
    //        pointList.Add(xMin_Point);
    //        pointList.Add(zMax_Point);
    //    }
    //    Vector3 v1 = pointList[0] - pointList[1];
    //    Vector3 v2 = pointList[1] - pointList[2];
    //    Vector3 v3 = pointList[2] - pointList[3];
    //    Vector3 v4 = pointList[3] - pointList[0];
    //    Vector3 v0 = Vector3.forward;// * showPerfabs.transform.position.z;
    //    v0 = Quaternion.AngleAxis(_touchPut.targetRotation.eulerAngles.y, Vector3.up) * v0;
    //    showPerfabForward = v0.normalized;
    //    showPerfabRight = (Quaternion.AngleAxis(_touchPut.targetRotation.eulerAngles.y, Vector3.up) * Vector3.right).normalized;
    //    ParalleCheck(v1, v0, pointList[0], pointList[1]);
    //    ParalleCheck(v2, v0, pointList[1], pointList[2]);
    //    ParalleCheck(v3, v0, pointList[2], pointList[3]);
    //    ParalleCheck(v4, v0, pointList[3], pointList[0]);
    //    pList = new List<Vector4>();
    //    pList.Add(new Vector4(pointList[0].x, pointList[0].y, pointList[0].z, 0));
    //    pList.Add(new Vector4(pointList[1].x, pointList[1].y, pointList[1].z, 0));
    //    pList.Add(new Vector4(pointList[2].x, pointList[2].y, pointList[2].z, 0));
    //    pList.Add(new Vector4(pointList[3].x, pointList[3].y, pointList[3].z, 0));
    //    Debug.Log("====================VerticesXZ_MaxMin init Finished");
    //}
    //private bool ParalleCheck(Vector3 v1, Vector3 v01, Vector3 p1, Vector3 p2)
    //{
    //    if (paralleZList == null) paralleZList = new List<List<Vector3>>();
    //    if (paralleXList == null) paralleXList = new List<List<Vector3>>();
    //    if (VectorUtility.IsParallel(v1, v01))
    //    {

    //        List<Vector3> p = new List<Vector3>();// { pointList[0], pointList[1] }
    //        if (VectorUtility.IsParallelAndDirection(v1, v01) == 1)
    //        {
    //            p.Add(p1);
    //            p.Add(p2);
    //        }
    //        else
    //        {
    //            p.Add(p2);
    //            p.Add(p1);
    //        }
    //        if (paralleZList.Count == 1)
    //        {
    //            if (VectorUtility.PointOnLeftSide((paralleZList[0][0] - paralleZList[0][1]), p[0]))
    //            {
    //                paralleZList.Insert(0, p);
    //            }
    //            else
    //            {
    //                paralleZList.Add(p);
    //            }
    //        }
    //        else
    //        {
    //            paralleZList.Add(p);
    //        }
    //        return true;
    //    }
    //    else
    //    {
    //        List<Vector3> p = new List<Vector3>();// { pointList[0], pointList[1] }
    //        if (VectorUtility.IsParallelAndDirection(v1, Quaternion.AngleAxis(_touchPut.targetRotation.eulerAngles.y, Vector3.up) * Vector3.right) == 1)
    //        {
    //            p.Add(p1);
    //            p.Add(p2);
    //        }
    //        else
    //        {
    //            p.Add(p2);
    //            p.Add(p1);
    //        }
    //        if (paralleXList.Count == 1)
    //        {
    //            if (VectorUtility.PointOnLeftSide((paralleXList[0][0] - paralleXList[0][1]), p[0]))
    //            {
    //                paralleXList.Insert(0, p);
    //            }
    //            else
    //            {
    //                paralleXList.Add(p);
    //            }
    //        }
    //        else
    //        {
    //            paralleXList.Add(p);
    //        }

    //        return false;
    //    }
    //}
    #endregion

    #region event function
    #endregion
}
