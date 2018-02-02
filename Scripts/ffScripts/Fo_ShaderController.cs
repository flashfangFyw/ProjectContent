using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ffDevelopmentSpace;

public class Fo_ShaderController : ShaderController, IShaderController {


    public GameObject wireframe_body;
    public GameObject main_body;

    protected float minValue = 0;
    protected float maxValue = 0;
    private float modelHeighth = 0;

    protected Material textureMaterial
    {
        get
        {
            if(_textureMaterial==null&& main_body)
            {
                _textureMaterial = main_body.GetComponent<MeshRenderer>().materials[0];
            }
            return _textureMaterial;
        }
    }
    private Material _textureMaterial;
    private bool flag_FadeInWireFrame = false;
    private bool flag_FadeOutWireFrame = false;
    private float mTime_WireFrame;

    private bool flag_FadeInHologram = false;
    private bool flag_FadeOutHologram = false;
    private float mTime_Hologram;

    private bool flag_FadeInTexture = false;
    private bool flag_FadeOutTexture = false;
    private float mTime_Texture;

    private bool flag_Move = false;
    public float MoveSpeed = 0;
    private Vector3 targetPosition;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (flag_Move)
        {
            //transform.Translate(Vector3.up * MoveSpeed * Time.deltaTime, Space.Self);
            //float step = moveSpeed * Time.deltaTime;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, MoveSpeed * Time.deltaTime);
        }
	}
    public void StartShow()
    {
        InitData();
        flag_Move = true;
    }
    private void InitData()
    {
        SetVerticesStartAndEnd();
        targetPosition = this.transform.localPosition;
        this.transform.localPosition += Vector3.down * modelHeighth;
    }
    //获取顶点最大，最小值
    protected virtual void SetVerticesStartAndEnd()
    {
        minValue = float.MaxValue;
        maxValue = float.MinValue;
        MeshFilter[] filterList = main_body.GetComponents<MeshFilter>();
        foreach (MeshFilter filter in filterList)
        {
            Mesh mesh = filter.mesh;
            Vector3[] vertices = mesh.vertices;
            int i = 0;
            Vector3 vertPos;
            foreach (Vector3 vertice in vertices)
            {
                //Debug.Log("I==" + i);
                vertPos = filter.transform.TransformPoint(vertice);
                if (vertPos.y < minValue) minValue = vertPos.y;
                if (vertPos.y > maxValue) maxValue = vertPos.y;
                i++;
            }
        }
        modelHeighth = maxValue - minValue;
        float disValue = (maxValue - minValue) / 50;
        maxValue += disValue;
        minValue -= disValue;

        //if (textureMaterial == null) return;
        ////for (int i = 0; i < hologamMaterials.Count; i++)
        ////{
        ////    hologamMaterials[i].SetFloat("_EffectTime", minValue);
        ////    hologamMaterials[i].SetFloat("_BottomValue", minValue);
        ////}
        //hologamMaterial.SetFloat("_EffectTime", minValue);
        //hologamMaterial.SetFloat("_BottomValue", minValue);
        if (main_body == null) return;
        if (textureMaterial == null) return;
        textureMaterial.SetFloat("EffectTime", maxValue);
        textureMaterial.SetFloat("BottomValue", minValue);
        Debug.Log("====================maxValue==" + maxValue + "     minValue==" + minValue+ "     modelHeighth==" + modelHeighth);
    }
}
