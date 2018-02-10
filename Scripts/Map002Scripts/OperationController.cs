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
    public bool ifDirections = false;
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
    private MyMap _myMap;
    private MyDirectionsFactory _myDF;
    //private bool putFlag=false;
    //[HideInInspector]
    private List<List<Vector3>> paralleZList
    {
        get
        {
            return Singleton<FieldModel>.GetInstance().paralleZList;
        }
    }
    //[HideInInspector]
    private List<List<Vector3>> paralleXList
    {
        get
        {
            return Singleton<FieldModel>.GetInstance().paralleXList;
        }
    }
    #endregion

    #region unity function
    void Awake()
    {
        _myMap = FindObjectOfType<MyMap>();
        _myDF = FindObjectOfType<MyDirectionsFactory>();
        //if (ifTest&&_myMap!=null)
        //{
        //    _myMap.enabled = false;
        //    _myMap.SetInitializeOnStart(true);
        //}
    }
    void OnEnable()
    {
    }
    void Start () 
	{
        _touchPut = gameObject.AddComponent<pTouchPut>();
        _touchPut.InitData(showPerfabs, framePerfabs, offsetHeight);
        _touchPut.enabled = true;

        if (ifTest)
        {
            Singleton<FieldModel>.GetInstance().CheckAreaField(framePerfabs, transform);
            _myMap.InitMap();
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
  
   
    #endregion

}
