using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    Author:     fyw 
    CreateDate: 2018-02-08 11:04:21 
    Desc:       Mesh顶点工具集
*/
public class MeshUtility
{
    /// <summary>
    /// 获取 目标对象 各个坐标 最大值组成的向量 和 最小值组成的向量
    /// </summary>
    /// <param name="target"></param>目标对象
    /// <param name="max"></param> 各个坐标 最大值组成的向量
    /// <param name="min"></param>各个坐标 最小值组成的向量
    public static void GetMeshMinMaxWorldPoints(GameObject target, out Vector3 max, out Vector3 min)
    {
        max = Vector3.one*float.MinValue;
        min = Vector3.one * float.MaxValue;

        MeshFilter[] mf0s = target.GetComponents<MeshFilter>();
        MeshFilter[] mf1s = target.GetComponentsInChildren<MeshFilter>();

        List<MeshFilter> meshFilterList = new List<MeshFilter>();
        foreach (MeshFilter mf0 in mf0s)
        {
            if (mf0.mesh)
            {
                meshFilterList.Add(mf0);
            }
        }

        foreach (MeshFilter mf1 in mf1s)
        {
            if (mf1.mesh)
            {
                meshFilterList.Add(mf1);
            }
        }


        foreach(MeshFilter meshF in meshFilterList)
        {
            Vector3[] vs = meshF.mesh.vertices;

            foreach(Vector3 locV in vs)
            {
                Vector3 worldV = meshF.gameObject.transform.TransformPoint(locV);
                if(worldV.x <min.x)
                {
                    min.x = worldV.x;
                }
                else
                {
                    if (worldV.x > max.x)
                    {
                        max.x = worldV.x;
                    }
                }

                if (worldV.y < min.y)
                {
                    min.y = worldV.y;
                }
                else
                {
                    if (worldV.y > max.y)
                    {
                        max.y = worldV.y;
                    }
                }

                if (worldV.z < min.z)
                {
                    min.z = worldV.z;
                }
                else
                {
                    if (worldV.z > max.z)
                    {
                        max.z = worldV.z;
                    }
                }
            }
        }
    }


    public static void GetVerticesXZ_MaxMin(GameObject target, Transform origin, out List<Vector3> pointList, out List<List<Vector3>> paralleZList, out List<List<Vector3>> paralleXList)// out List<Vector4> pList)
    {
        //==========================
        float x_Max = float.MinValue;
        float x_Min = float.MaxValue;
        float z_Max = float.MinValue;
        float z_Min = float.MaxValue;
        Vector3 xMaxzMin_Point = origin.position;
        Vector3 xMinzMin_Point = origin.position;
        Vector3 xMinzMax_Point = origin.position;
        Vector3 xMaxzMax_Point = origin.position;

        Vector3 xMax_Point = origin.position;
        Vector3 xMin_Point = origin.position;
        Vector3 zMax_Point = origin.position;
        Vector3 zMin_Point = origin.position;
        MeshFilter[] filterList = target.GetComponents<MeshFilter>();
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
        if (target.transform.rotation.eulerAngles.y % 90 == 0)
        {
            xMaxzMin_Point = new Vector3(x_Max, origin.position.y, z_Min);
            xMinzMin_Point = new Vector3(x_Min, origin.position.y, z_Min);
            xMinzMax_Point = new Vector3(x_Min, origin.position.y, z_Max);
            xMaxzMax_Point = new Vector3(x_Max, origin.position.y, z_Max);
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
        //Vector3 v1 = pointList[0] - pointList[1];
        //Vector3 v2 = pointList[1] - pointList[2];
        //Vector3 v3 = pointList[2] - pointList[3];
        //Vector3 v4 = pointList[3] - pointList[0];
        //Vector3 v0 = Vector3.forward;// * showPerfabs.transform.position.z;
        //v0 = Quaternion.AngleAxis(targetRotation.eulerAngles.y, Vector3.up) * v0;
        //showPerfabForward = v0.normalized;
        //showPerfabRight = (Quaternion.AngleAxis(targetRotation.eulerAngles.y, Vector3.up) * Vector3.right).normalized;
        paralleXList = new List<List<Vector3>>();
        paralleZList = new List<List<Vector3>>();
        List<List<Vector3>> list = new List<List<Vector3>>();
        if (ParalleCheck(target, pointList[0], pointList[1],  paralleZList,  paralleXList ,out list))
        {
            paralleZList = list;
        }
        else
        {
            paralleXList = list;
        }
        //============================
        if (ParalleCheck(target, pointList[1], pointList[2], paralleZList, paralleXList, out list))
        {
            paralleZList = list;
        }
        else
        {
            paralleXList = list;
        }
        //============================
        if (ParalleCheck(target, pointList[2], pointList[3], paralleZList, paralleXList, out list))
        {
            paralleZList = list;
        }
        else
        {
            paralleXList = list;
        }
        //============================
        if (ParalleCheck(target, pointList[3], pointList[0], paralleZList, paralleXList, out list))
        {
            paralleZList = list;
        }
        else
        {
            paralleXList = list;
        }
        //ParalleCheck(target, pointList[1], pointList[2], out paralleZList, out paralleXList);
        //ParalleCheck(target, pointList[2], pointList[3], out paralleZList, out paralleXList);
        //ParalleCheck(target, pointList[3], pointList[0], out paralleZList, out paralleXList);


        //pList = new List<Vector4>();
        //pList.Add(new Vector4(pointList[0].x, pointList[0].y, pointList[0].z, 0));
        //pList.Add(new Vector4(pointList[1].x, pointList[1].y, pointList[1].z, 0));
        //pList.Add(new Vector4(pointList[2].x, pointList[2].y, pointList[2].z, 0));
        //pList.Add(new Vector4(pointList[3].x, pointList[3].y, pointList[3].z, 0));
        Debug.Log("====================VerticesXZ_MaxMin init Finished");
    }
    private static void pointListParalleCheck(GameObject target, List<Vector3> pList, out List<List<Vector3>> paralleZList, out List<List<Vector3>> paralleXList)
    {
         paralleXList = new List<List<Vector3>>();
         paralleZList = new List<List<Vector3>>();
        for (int i=0;i<pList.Count;i++)
       {

       }
    }
    private static bool ParalleCheck(GameObject target, Vector3 p1, Vector3 p2, List<List<Vector3>> paralleZList,  List<List<Vector3>> paralleXList ,out List<List<Vector3>> list)
    {
        // paralleZList = new List<List<Vector3>>();
        //paralleXList = new List<List<Vector3>>();
         Vector3 p3 = p1 - p2;
        if (VectorUtility.IsParallel(p3, target.transform.forward))
        {

            List<Vector3> p = new List<Vector3>();// { pointList[0], pointList[1] }
            if (VectorUtility.IsParallelAndDirection(p3, target.transform.forward) == 1)
            {
                p.Add(p1);
                p.Add(p2);
            }
            else
            {
                p.Add(p2);
                p.Add(p1);
            }
            if (paralleZList.Count == 1)
            {
                if (VectorUtility.PointOnLeftSide((paralleZList[0][0] - paralleZList[0][1]), p[0]))
                {
                    paralleZList.Insert(0, p);
                }
                else
                {
                    paralleZList.Add(p);
                }
            }
            else
            {
                paralleZList.Add(p);
            }
            list = paralleZList;
            return true;
        }
        else
        {
            List<Vector3> p = new List<Vector3>();// { pointList[0], pointList[1] }
            if (VectorUtility.IsParallelAndDirection(p3, target.transform.right) == 1)
            {
                p.Add(p1);
                p.Add(p2);
            }
            else
            {
                p.Add(p2);
                p.Add(p1);
            }
            if (paralleXList.Count == 1)
            {
                if (VectorUtility.PointOnLeftSide((paralleXList[0][0] - paralleXList[0][1]), p[0]))
                {
                    paralleXList.Insert(0, p);
                }
                else
                {
                    paralleXList.Add(p);
                }
            }
            else
            {
                paralleXList.Add(p);
            }
            list = paralleXList;
            return false;
        }
    }
}
