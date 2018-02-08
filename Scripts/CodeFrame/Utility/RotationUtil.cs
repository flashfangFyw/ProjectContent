using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基本旋转工具
/// 3个参数为 3个轴向的旋转速度
/// 3个状态为   旋转的3个阶段
/// </summary>
public class RotationUtil : MonoBehaviour {

    public float xSpeed;
    public float ySpeed;
    public float zSpeed;

	protected enum   RotateState:int 
	{
		StartRotate=0,
		RotateToEnd=1,
		StopRotate=2,
	}
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdateRotate();
    }
    protected virtual void UpdateRotate()
    {
        transform.Rotate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, zSpeed * Time.deltaTime);
    }
}
