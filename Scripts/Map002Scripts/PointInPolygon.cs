using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-02-05 12:36:56 
    Desc:       注释 
*/


public class PointInPolygon : MonoBehaviour 
{

    #region public property
    private List<Vector3> pointList=new List<Vector3>();
    private bool pFlag = false;
    private Material mt;
    private Transform child;
    #endregion
    #region private property
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start () 
	{
        //mt = this.GetComponent<MeshRenderer>().materials[0];
        //child = this.transform.GetChild(0);
    }   
	void Update () 
	{
        if(pFlag)
        {
            if (pnpoly()&& CheckBottom())
            {
                this.transform.GetChild(0).gameObject.SetActive(true);
                //Debug.Log("(pnpoly()");
                //if (mt) mt.SetColor("_Color", Color.white);
            }
            else
            {
                this.transform.GetChild(0).gameObject.SetActive(false);
                //if (mt) mt.SetColor("_Color", Color.black);
            }
        }
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
    private float bottomValue = 0;
    public void SetBottom(float v)
    {
        bottomValue = v;
    }
    public void SetPointList(List<Vector3> list)
    {
        pointList = list;
        pFlag = true;
    }
    public bool  RayCasting()
    {
        float px = this.transform.position.x;
        float py = this.transform.position.z;
        bool flag = false;

    for (int i = 0, l = pointList.Count, j = l - 1; i < l; j = i, i++)
        {
            float sx = pointList[i].x;
            float sy = pointList[i].z;
            float tx = pointList[j].x;
            float ty = pointList[j].z;

      // 点与多边形顶点重合
            if ((sx == px && sy == py) || (tx == px && ty == py))
            {
                return true;// 'on'
             }

            // 判断线段两端点是否在射线两侧
            if ((sy < py && ty >= py) || (sy >= py && ty < py))
            {
                // 线段上与射线 Y 坐标相同的点的 X 坐标
                float x = sx + (py - sy) * (tx - sx) / (ty - sy);
      
        // 点在多边形的边上
                if (x == px)
                {
                    return true;// 'on'
                }

                // 射线穿过多边形的边界
                if (x > px)
                {
                    flag = !flag;
                }
            }
        }

        // 射线穿过多边形边界的次数为奇数时点在多边形内
        return flag;// ? 'in' : 'out'
  }

    bool pnpoly( )
    {
        int i, j = 0;
        bool c = false;
        float testx = this.transform.position.x;
        float testy = this.transform.position.z;
        for (i = 0, j = 4 - 1; i < 4; j = i++)
        {
            if (((pointList[i].z > testy) != (pointList[j].z > testy)) &&
             (testx < (pointList[j].x - pointList[i].x) * (testy - pointList[i].z) / (pointList[j].z - pointList[i].z) + pointList[i].x))
                c = !c;
        }
        return c;
    }
    bool CheckBottom()
    {
        return this.transform.position.y > bottomValue;
    }
    #endregion

}
