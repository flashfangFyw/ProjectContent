using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-02-10 14:02:00 
    Desc:       注释 
*/


public class FieldModel : ModelBase 
{
    public  List<Vector3> pointList;
    public  List<Vector4> pList;
    public List<List<Vector3>> paralleZList;
    public List<List<Vector3>> paralleXList;
    public float bottomOffset;
    public void CheckAreaField(GameObject target,Transform transform)
    {
        MeshUtility.GetVerticesXZ_MaxMin(target, transform, out pointList,out paralleZList,out paralleXList);
        pList = VectorUtility.List_Vec3To4Add0(pointList);
        //Debug.
    }
}
