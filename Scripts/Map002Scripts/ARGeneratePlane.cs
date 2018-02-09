using UnityEngine;
using System.Collections;
using UnityEngine.XR.iOS;
using ffDevelopmentSpace;
using System.Collections.Generic;


/* 
    Author:     fyw 
    CreateDate: 2018-02-02 11:07:49 
    Desc:        面生成个管理器 需要 arkit插件
*/


public class ARGeneratePlane : SingletonMB<ARGeneratePlane>
{

    #region public property
    public GameObject planePrefab;
    private UnityARAnchorManager unityARAnchorManager;
    #endregion
    #region private property
    #endregion

    #region unity function
    void OnEnable()
    {
    }
    void Start () 
	{
        unityARAnchorManager = new UnityARAnchorManager();
        UnityARUtility.InitializePlanePrefab(planePrefab);
    }   
	void Update () 
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
    public void GetPlaneEdge()
    {
        Debug.Log("GetPlaneEdge");
    }
    public void HidePlane()
    {
        List<ARPlaneAnchorGameObject> list = unityARAnchorManager.GetCurrentPlaneAnchors();
        Debug.Log("list="+ list.Count);
        foreach (ARPlaneAnchorGameObject plane in list)

        {
            //plane.gameObject.GetComponent<MeshRenderer>().sharedMaterials[0]
            //plane.gameObject.GetComponent<MeshRenderer>().enabled = false;
            plane.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        unityARAnchorManager.Destroy();
    }
    public void GetFocusPlane()
    {

    }
	#endregion
	#region private function
	#endregion

    #region event function
    #endregion
}
