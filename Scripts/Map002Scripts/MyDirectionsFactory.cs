using UnityEngine;
using System.Collections;
using Mapbox.Directions;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Modifiers;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using Mapbox.Unity;
using ffDevelopmentSpace;


/* 
    Author:     #AuthorName# 
    CreateDate: #CreateDate# 
    Desc:       注释 
*/
public struct RouteData
{
    int index;
    int dataIndex;
    List<Vector3> data;
}

public class MyDirectionsFactory : MonoBehaviour
{

    #region public property
    //public CameraRayTest ct;
    public TouchHitTest tht;
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    MeshModifier[] MeshModifiers;

    //[SerializeField]
    //List<Transform> _waypoints;

    [SerializeField]
    Material[] _material;

    [SerializeField]
    float _directionsLineWidth;

    public GameObject[] movePerfab;
	private List<float> speedList = new List<float> { 0.1f, 0.1f, 0.1f };


    public List<List<int>> routeList = new List<List<int>> {
                                                             new List<int> { 1, 3 },
                                                             new List<int> { 2, 1},
                                                             new List<int> { 4, 2 }
                                                            };


    //GameObject _directionsGO;
    #endregion
    #region private property
    private Directions _directions;
    private int _counter;
    private bool initFinish = false;
    //private int currentWaypoint = 1;
    //private GameObject car;
    private Dictionary<int, Transform> pointDic;//
    private bool countFinish = false;
    private bool visualizerFinish = false;
    private int pointCount = 0;
    private int routeCount = 0;
    private Dictionary<int, GameObject> _directionsDic;//显示路线管理
    private Dictionary<int, int> _dicCurrrenWaypoint;//路节点索引管理
    private Dictionary<int, List<Vector3>> _dicRouteData;//路节点坐标管理
    private Dictionary<int, GameObject> _dicCar;//车辆管理
    #endregion

    #region unity function
    void Awake()
    {
        if (_map == null)
        {
            _map = FindObjectOfType<AbstractMap>();
        }
        _directions = MapboxAccess.Instance.Directions;
        _map.OnInitialized += Query;

        var map = FindObjectOfType<AbstractMap>();
        var visualizer = map.MapVisualizer;
        visualizer.OnMapVisualizerStateChanged += (s) =>
        {
            if (this == null)
                return;

            if (s == ModuleState.Finished)
            {
                Debug.Log("=======visualizerFinish");
                visualizerFinish = true;
                CheckIfCreatRoute();
            }
        };
    }
    void OnEnable()
    {
    }
    void Start () 
	{
    }   
	void Update () 
	{
	}
    void OnDisable()
    {
    }
    void OnDestroy()
    {
        _map.OnInitialized -= Query;
    }
    #endregion

    #region public function
    
    public void AddPoint(int index,Transform trans)
    {
        if (pointDic == null) pointDic = new Dictionary<int, Transform>();
        pointDic.Add(index, trans);
        pointCount++;
        if (pointCount == 4)
        {
            //CreatRoute();
            countFinish = true;
            Debug.Log("=======countFinish");
          
            CheckIfCreatRoute();
        }
        //================================
        //GameObject pb = movePerfab[routeCount];
        //GameObject car = GameObject.Instantiate(pb) as GameObject;
        //if (car == null)
        //{
        //    Debuger.Log("=====================================================go is null ");
        //    return;
        //}
        //car.name =index+"";
        ////car.transform.localScale = Vector3.one;
        ////car.transform.localPosition = Vector3.zero;
        ////car.transform.localRotation = Quaternion.identity;
        //car.transform.position = trans.position;
        ////car.transform.SetParent(this.transform, true);

    }
    private void CheckIfCreatRoute()
    {
        //if(countFinish&& initFinish && visualizerFinish)
        if ( initFinish && visualizerFinish)
        {
            if (tht) tht.LoactionTheModel();
            //StartCoroutine(CreatRoute());
        }
    }
    private  IEnumerator  CreatRoute()
    {
        //yield return null;
        for (int i = 0; i < routeList.Count; i++)
        {
            List<Transform> rt = new List<Transform>();
            for (int j = 0; j < routeList[i].Count; j++)
            {
                rt.Add(pointDic[routeList[i][j]]);
            }
            DoWayPoint(rt);
            if (routeCount <= i) yield return null;
        }
        Debug.Log("CreatRoute is Finished");
        if (tht)
        {
            SetMaterialParam();
            tht.LoactionTheModel();
        }
    }
    
   
    private void DoWayPoint(List<Transform> rt)
    {
        if (initFinish == false) return;
        var count = rt.Count;
        var wp = new Vector2d[count];
        for (int i = 0; i < count; i++)
        {
            wp[i] = rt[i].GetGeoPosition(_map.CenterMercator, _map.WorldRelativeScale);
        }
        var _directionResource = new DirectionResource(wp, RoutingProfile.Driving);
        _directionResource.Steps = true;
        _directions.Query(_directionResource, HandleDirectionsResponse);
    }
    #endregion
    #region private function

    private GameObject CreateGameObject(MeshData data)
    {
        if (_directionsDic == null) _directionsDic = new Dictionary<int, GameObject>();
        //if (_directionsDic[routeCount]!= null)
        //{
        //    Destroy(_directionsGO);
        //}
        GameObject _directionsGO = new GameObject("direction waypoint " + " entity"+routeCount);
        _directionsGO.transform.SetParent(this.transform, true);
        var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
        mesh.subMeshCount = data.Triangles.Count;

        mesh.SetVertices(data.Vertices);
        _counter = data.Triangles.Count;
        for (int i = 0; i < _counter; i++)
        {
            var triangle = data.Triangles[i];
            mesh.SetTriangles(triangle, i);
        }

        _counter = data.UV.Count;
        for (int i = 0; i < _counter; i++)
        {
            var uv = data.UV[i];
            mesh.SetUVs(i, uv);
        }

        mesh.RecalculateNormals();
     
        _directionsGO.AddComponent<MeshRenderer>().material = _material[routeCount];
        _directionsDic[routeCount] = _directionsGO;
        routeCount++;
        //Debug.Log("routeCount ADD~~~~~~~~~~~~~~~~");
        return _directionsGO;
    }
    
    private void MoveTheCar(List<Vector3> data)
    {
        GameObject pb = movePerfab[routeCount];

        //GameObject car = Util.CreatElement(pb, this.transform, pb.name, false);
        GameObject car =  GameObject.Instantiate(pb) as GameObject;
        if (car == null)
        {
            Debug.Log("=====================================================go is null ");
            return;
        }
        car.name = pb.name;
        car.transform.localScale = Vector3.one;
        car.transform.localPosition = Vector3.zero;
        car.transform.localRotation = Quaternion.identity;
        car.transform.position = data[0];
        MeshRenderer[] mrs = car.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            mr.materials[0].SetInt("_Points_Num", tht.GetPointList().Count);
            mr.materials[0].SetVectorArray("_Points", tht.GetPList());
        }
        //car.transform.SetParent(this.transform, false);



        //car.transform.position = data[0];
        if (_dicRouteData == null) _dicRouteData = new Dictionary<int, List<Vector3>>();
        _dicRouteData[routeCount] = data;
        if (_dicCar == null) _dicCar = new Dictionary<int, GameObject>();
        _dicCar[routeCount] = car;
        //for(int i=0;i<data.Count;i++)
        //{
        //    //Transform.
        //    data[i]=car.transform.InverseTransformPoint(data[i]);
        //}
        //=======================================================
        //Hashtable hash = new Hashtable();
        //hash.Add("path", data.ToArray());
        //hash.Add("movetopath", false);
        //hash.Add("orienttopath", true);
        //hash.Add("speed", 10);
        ////hash.Add("time", 10);
        ////hash.Add("movetopath", false);
        ////hash.Add("", "");
        //hash.Add("easeType", iTween.EaseType.linear);
        //hash.Add("loopType", "loop");
        ////iTween.DrawPath(data.ToArray(), Color.yellow);
        //paths = data.ToArray();
        //iTween.MoveTo(car, hash);
        //======================================================
        //Debug.Log("routeCount ADD~~~~~~~~~~~~~~~~" + routeCount)/*;*/
        //return;
        MoveToWaypoint(routeCount);
    }
  
    void MoveToWaypoint(int index)
    {
        if (_dicCurrrenWaypoint == null) _dicCurrrenWaypoint = new Dictionary<int, int>();
        if (_dicCurrrenWaypoint.ContainsKey(index)==false)
        {
            _dicCurrrenWaypoint[index] = 0;
        }
        //Time = Distance / Rate:
        //float travelTime = Vector3.Distance(transform.position, data[0]) / rate;

        Hashtable hash = new Hashtable();
        hash.Add("position", _dicRouteData[index][_dicCurrrenWaypoint[index]]);
        //hash.Add("time", travelTime);
        hash.Add("easetype", "linear");
        hash.Add("speed", speedList[index]);//speedList[index]
        hash.Add("islocal", true);
        //hash.Add("axis", "y");
        hash.Add("oncomplete", "MoveToWaypoint");
        hash.Add("oncompleteparams", index);
        hash.Add("oncompletetarget", gameObject);
        hash.Add("orienttopath", true);
        //hash.Add("Looktarget", _dicRouteData[index][_dicCurrrenWaypoint[index]]);
        //hash.Add("Looktarget", car.transform.TransformPoint(car.transform.InverseTransformPoint(data[currentWaypoint])));
        //hash.Add("looktime", .4);

        //iTween:
        iTween.MoveTo(_dicCar[index], hash);

        //Move to next waypoint:
        _dicCurrrenWaypoint[index]++;
        if (_dicCurrrenWaypoint[index] > _dicRouteData[index].Count - 1)
        {
            _dicRouteData[index].Reverse();
            _dicCurrrenWaypoint[index] = 0;
        }
    }
    #endregion
    //private Vector3[] paths;
    //void OnDrawGizmos()
    //{
    //    //在scene视图中绘制出路径与线
    //    iTween.DrawLine(paths, Color.yellow);

    //    iTween.DrawPath(paths, Color.red);

    //}
    #region event function
    void Query()
    {
        Debug.Log("=======initFinish Finish");
        initFinish = true;
        CheckIfCreatRoute();
    }

    void HandleDirectionsResponse(DirectionsResponse response)
    {
        if (null == response.Routes || response.Routes.Count < 1)
        {
            return;
        }

        var meshData = new MeshData();
        var dat = new List<Vector3>();
        foreach (var point in response.Routes[0].Geometry)
        {
            Vector3 p = Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz();
            //Debug.Log(p);
            //p = Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz()
            //      + Vector3.right * ct.getOffsetPosition().x
            //      + Vector3.up * ct.getOffsetPosition().y
            //      + Vector3.forward * ct.getOffsetPosition().z;
            //Debug.Log("=============================="+p);
            //p = Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz()
            //   + ct.getOffsetPosition() + _map.transform.position;
            p = Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz()
                  + tht.GetOffsetPosition();
            //Debug.Log(ct.getOffsetPosition());
            //Debug.Log(_map.transform.position);
            //Debug.Log("==============================" + p);
            //dat.Add(Conversions.GeoToWorldPosition(point.x, point.y, _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz());
            dat.Add(p);
        }

        var feat = new VectorFeatureUnity();
        feat.Points.Add(dat);
        //if (movePerfab != null && movePerfab.Length > 0) MoveTheCar(dat);

        foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active))
        {
            var lineMod = mod as LineMeshModifier;
            if (lineMod != null)
            {
                lineMod.Width = _directionsLineWidth / _map.WorldRelativeScale;
            }
            mod.Run(feat, meshData, _map.WorldRelativeScale);
        }
        Debug.Log("CreateGameObject");
        CreateGameObject(meshData);
    }
    private void SetMaterialParam()
    {
        return;
        foreach(var mt in  _material)
        {
            mt.SetInt("_Points_Num", tht.GetPointList().Count);
            mt.SetVectorArray("_Points", tht.GetPList());
        }
    }
    #endregion
}
