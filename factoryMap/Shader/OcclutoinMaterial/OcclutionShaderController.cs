using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-02-10 10:09:16 
    Desc:       OcclutoinMaterial shader的控制
*/


public class OcclutionShaderController : MonoBehaviour
{

    /// <summary>
    /// 获取 目标对象 各个坐标 最大值组成的向量 和 最小值组成的向量
    /// </summary>
    /// <param name="target"></param>目标对象
    /// <param name="max"></param> 各个坐标 最大值组成的向量
    /// <param name="min"></param>各个坐标 最小值组成的向量
    public static void SetMaterialParamsToTarget(GameObject target, List<Vector4> pList,float h)
    {
        MeshRenderer[] mrs = target.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            //Debug.Log("shader.name =="+ mr.materials[0].shader.name);
            //if (mr.materials[0].shader.name == "CustomMobile/OcclutoinMaterial")
            //{
                //Debug.Log("_Points count="+ pList.Count);
                mr.materials[0].SetInt("_Points_Num", pList.Count);
                mr.materials[0].SetVectorArray("_Points", pList);
                mr.materials[0].SetFloat("_Points_Bottom", h);
            //}
        }
    }

    public static void SetPointInPolygon(GameObject target, List<Vector3> pointList, float h)
    {
        PointInPolygon[] pIps = target.GetComponentsInChildren<PointInPolygon>();
        foreach (PointInPolygon p in pIps)
        {
            p.SetBottom(h);
            p.SetPointList(pointList);
        }
    }
}
