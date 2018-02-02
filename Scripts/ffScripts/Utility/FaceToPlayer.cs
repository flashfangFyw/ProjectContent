using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FaceToPlayer : MonoBehaviour {

    public bool lockX;
    public bool lockY;
    public bool lockZ;

    private Camera player;
    private Vector3 lookAtPosition = Vector3.zero;
    // Use this for initialization
    void Start () {
        player = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        if (Application.isPlaying)
        {
            ToLookAt();

            transform.LookAt(lookAtPosition);
        }
    }
    private void ToLookAt()
    {
        if (player==null || player.transform == null ) return;
        if (!lockX) lookAtPosition.x = player.transform.position.x;
        else lookAtPosition.x = transform.position.x;
        if (!lockY) lookAtPosition.y = player.transform.position.y;
        else lookAtPosition.y = transform.position.y;
        if (!lockZ) lookAtPosition.z = player.transform.position.z;
        else lookAtPosition.z = transform.position.z;
    }

    public void FaceToThePlayer()
    {
        if (Application.isPlaying)
        {
            ToLookAt();

            transform.LookAt(lookAtPosition);
        }
    }
}
