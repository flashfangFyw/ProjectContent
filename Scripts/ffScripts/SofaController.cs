using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class SofaController : MonoBehaviour {

    public GameObject modelPerfab;
    public FaceToPlayer faceToPlayer;
	public float createHeight;
	private MaterialPropertyBlock props;


	// Use this for initialization
	void Start () {
        modelPerfab.SetActive(false);
        if (faceToPlayer) faceToPlayer.FaceToThePlayer();
        if (modelPerfab) modelPerfab.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("~~~~~~~~~~~~~~~`touchCount="+Input.touchCount);
		if (Input.touchCount > 0)
		{
			var touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began)
			{
				var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
				ARPoint point = new ARPoint
				{
					x = screenPosition.x,
					y = screenPosition.y
				};

				List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point,
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent);
				if (hitResults.Count > 0)
				{
					foreach (var hitResult in hitResults)
					{
						Vector3 position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
						CreateBall(new Vector3(position.x, position.y + createHeight, position.z));

						break;
					}
				}

			}
		}
	}
    public void DoCreate()
    {
        CreateBall(Vector3.forward*4);
    }
     void CreateBall(Vector3 atPosition)
	{
        if (modelPerfab == null) return;
        if (modelPerfab.activeSelf == false)
        {
            modelPerfab.SetActive(true);
        }
        modelPerfab.gameObject.transform.position = atPosition;
        //modelPerfab.gameObject.transform.rotation = Quaternion.identity;
        if (faceToPlayer) faceToPlayer.FaceToThePlayer();
		//GameObject ballGO = Instantiate(ballPrefab, atPosition, Quaternion.identity);


		//float r = Random.Range(0.0f, 1.0f);
		//float g = Random.Range(0.0f, 1.0f);
		//float b = Random.Range(0.0f, 1.0f);

		//props.SetColor("_InstanceColor", new Color(r, g, b));

		//MeshRenderer renderer = ballGO.GetComponent<MeshRenderer>();
		//renderer.SetPropertyBlock(props);

	}
}
