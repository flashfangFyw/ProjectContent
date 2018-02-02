using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class CameraRayTest : MonoBehaviour {

    public enum FocusState
    {
        Initializing,
        Finding,
        Found
    }

    //public CanvasController canvasController;
    public GameObject panelFlag;
    public GameObject findingSquare;
    public GameObject foundSquare;
    //======//for editor version
    public float maxRayDistance = 30.0f;
    public LayerMask collisionLayerMask;
    public float findingSquareDist = 0.5f;
    //
    private FocusState squareState;
    public FocusState SquareState
    {
        get
        {
            return squareState;
        }
        set
        {
            squareState = value;
            foundSquare.SetActive(squareState == FocusState.Found);
            findingSquare.SetActive(squareState != FocusState.Found);
        }
    }
    //===============
    public GameObject modelPerfab;
    public bool faceToPlayerFlag;
    private FaceToPlayer _faceToPlayer;
    public float createHeight;


    private Camera _camera;
    private Camera thisCamera
    {
        get{
            if (_camera == null) _camera = this.gameObject.GetComponent<Camera>();
            return _camera;
        }
    }
    private FaceToPlayer faceToPlayer
    {
        get
        {
            if (_faceToPlayer == null) _faceToPlayer = this.gameObject.GetComponentInChildren<FaceToPlayer>();
            return _faceToPlayer;
        }
    }
    private bool trackingInitialized;
	// Use this for initialization
	void Start () {
        //if (panelFlag) panelFlag.SetActive(false);
        int layerIndex = LayerMask.NameToLayer("ARGameObject");
        collisionLayerMask = 1 << layerIndex;
        SquareState = FocusState.Initializing;
        trackingInitialized = true;
        if (modelPerfab) modelPerfab.SetActive(false);
        //if (faceToPlayerFlag) faceToPlayer.FaceToThePlayer();
        //CheckFace();
	}
    Ray ray;  
    RaycastHit hit;  
    Vector3 center = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);  
    Vector3 hitpoint = Vector3.zero;  
  
	// Update is called once per frame
	void Update () {
        //射线沿着屏幕X轴从左向右循环扫描  
        //v3.x = v3.x >= Screen.width ? 0.0f : v3.x + 1.0f;  
        //生成射线  
        ray = thisCamera.ScreenPointToRay(center);  
        if (Physics.Raycast(ray, out hit, maxRayDistance))  
        {  
            //绘制线，在Scene视图中可见  
            //Debug.DrawLine(ray.origin, hit.point, Color.green);  
            //输出射线探测到的物体的名称  
            //Debug.Log("射线探测到的物体名称：" + hit.transform.name);
            //and the rotation from the transform of the plane collider

            //TogglePanelFlag(true);
            SetPanelProperty(hit);
            SquareState = FocusState.Found;
        } else{
            if(trackingInitialized)
            {
                SquareState = FocusState.Finding;

                //check camera forward is facing downward
                if (Vector3.Dot(Camera.main.transform.forward, Vector3.down) > 0)
                {
                    //Debug.Log("camera forward is facing downward：" + hit.transform.name);
                    //position the focus finding square a distance from camera and facing up
                    findingSquare.transform.position = Camera.main.ScreenToWorldPoint(center);

                    //vector from camera to focussquare
                    Vector3 vecToCamera = findingSquare.transform.position - Camera.main.transform.position;

                    //find vector that is orthogonal to camera vector and up vector
                    Vector3 vecOrthogonal = Vector3.Cross(vecToCamera, Vector3.up);

                    //find vector orthogonal to both above and up vector to find the forward vector in basis function
                    Vector3 vecForward = Vector3.Cross(vecOrthogonal, Vector3.up);

                    if(vecForward!=Vector3.zero)
                    {
                        findingSquare.transform.rotation = Quaternion.LookRotation(vecForward, Vector3.up);
                    }
                   

                }
                else
                {
                    //we will not display finding square if camera is not facing below horizon
                    findingSquare.SetActive(false);
                }
            }

        }
        TouchCheck();
	}

    public void TogglePanelFlag(bool flag)
    {
        Debug.Log("~~~~~~~~~~~~~~~`TogglePanelFlag=" + flag);
        if (panelFlag) panelFlag.SetActive(flag);

    }
    public void SetPanelProperty(RaycastHit hit)
    {
        if (panelFlag)
        {
            panelFlag.transform.position = hit.point;
            panelFlag.transform.rotation = hit.transform.rotation;
        }

    }
    private void TouchCheck()
    {
        //Debug.Log("~~~~~~~~~~~~~~~`touchCount="+Input.touchCount);
        if (SquareState != FocusState.Found) return;
		if(flagModel) return;
        if (Input.touchCount > 0 )
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                ShowGameObject();

                //var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
                //ARPoint point = new ARPoint
                //{
                //    x = screenPosition.x,
                //    y = screenPosition.y
                //};

                //List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point,
                //    ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
                //if (hitResults.Count > 0)
                //{
                //    foreach (var hitResult in hitResults)
                //    {
                //        Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
                //        ShowGameObject(new Vector3(position.x, position.y + createHeight, position.z));

                //        break;
                //    }
                //}

            }
        }
        if(Input.mousePresent)
        {
            if(Input.GetMouseButtonUp(0))
            {
                ShowGameObject();
            }
        }
    }
    //private bool modelState;
    //private void ShowGameObject(Vector3 atPosition)
    private void ShowGameObject()
    {
        if (modelPerfab == null) return;
        //modelPerfab.transform.position = panelFlag.transform.position;
        //modelPerfab.transform.rotation = panelFlag.transform.rotation;
        if (modelPerfab.activeSelf == false)
        {
            modelPerfab.SetActive(true);
            if(modelPerfab.GetComponent<BasicMap>())
            {
                //modelPerfab.GetComponent<BasicMap>().InitMap(panelFlag.transform.position);
                offsetPosition = panelFlag.transform.position;
                modelPerfab.BroadcastMessage("InitMap", panelFlag.transform, SendMessageOptions.DontRequireReceiver);
            }
        }
        //CheckFace();
        TogglePanelFlag(false);
		flagModel=true;
        //modelState = true;
    }
    private Vector3 offsetPosition = Vector3.one;
    public Vector3 getOffsetPosition()
    {
        return offsetPosition;
    }
    public void LoactionTheModel()
    {
        modelPerfab.transform.position = panelFlag.transform.position;
    }
	private bool flagModel=false;
    private void CheckFace()
    {
        if (faceToPlayerFlag)
        {
            if(faceToPlayer) faceToPlayer.FaceToThePlayer();
            modelPerfab.BroadcastMessage("LookToPlayer", SendMessageOptions.DontRequireReceiver);
        }
    }
    public void ShowModel()
    {
        
        if (modelPerfab == null) return;
        if (modelPerfab.activeSelf == false)
        {
            modelPerfab.SetActive(true);
        }
        modelPerfab.gameObject.transform.position = panelFlag.transform.position;
//        if (faceToPlayerFlag)
//        {
//            //faceToPlayer.FaceToThePlayer();
//            modelPerfab.BroadcastMessage("LookToPlayer", SendMessageOptions.DontRequireReceiver);
//        }
        TogglePanelFlag(false);
    }
    [SerializeField]
    MyDirectionsFactory _mDF;
    public MyDirectionsFactory GetDirectionsFactory()
    {
        return _mDF;
    }
}
