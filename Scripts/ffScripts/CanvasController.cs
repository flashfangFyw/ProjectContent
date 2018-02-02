using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    public Animator animator;
    public RectTransform Rec;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void FadeOut()
    {
        if (animator) animator.SetInteger("flag", 1);
    }
    public void ShowUI()
    {
        if (animator) animator.SetInteger("flag", 0);
    }
    public void UpdateUIProperty(RaycastHit hit)
    {
        if(Rec)
        {
            Rec.position=RectTransformUtility.WorldToScreenPoint(Camera.main,hit.point);
            Rec.rotation = hit.transform.rotation;
        }
    }
}
