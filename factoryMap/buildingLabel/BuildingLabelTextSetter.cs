using UnityEngine;
using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Interfaces;
using System;
using UnityEngine.UI;
using ffDevelopmentSpace;


/* 
    Author:     #AuthorName# 
    CreateDate: #CreateDate# 
    Desc:       注释 
*/


public class BuildingLabelTextSetter : MonoBehaviour ,IFeaturePropertySettable
{
    [SerializeField]
    TextMesh _textMesh;
    [SerializeField]
    GameObject _b1;
    [SerializeField]
    Text _b1Txt;

    [SerializeField]
    GameObject _b2;
    [SerializeField]
    Text _b2Txt;

    [SerializeField]
    GameObject _b3;
    [SerializeField]
    Text _b3Txt;

    //[SerializeField]
    //GameObject _b4;
    [SerializeField]
    Text _b4Txt;

    //[SerializeField]
    private MyDirectionsFactory _directionsFactory;
    private MyDirectionsFactory directionsFactory
    {
        get
        {
            if(_directionsFactory==null) _directionsFactory = FindObjectOfType<MyDirectionsFactory>();
            if(_directionsFactory == null)
            {
                CameraRayTest crt = FindObjectOfType<CameraRayTest>();
                if(crt)
                {
                    _directionsFactory = crt.GetDirectionsFactory();
                }
            }
            if (_directionsFactory == null)
            {
                MyDirectionsFactory crt = FindObjectOfType<MyDirectionsFactory>();
                if (crt)
                {
                    _directionsFactory = crt;
                }
            }
            return _directionsFactory;
        }
    }


    void Start()
    {
      
    }
    public void Set(Dictionary<string, object> props)
    {
        _textMesh.text = "";

        if (props.ContainsKey("name"))
        {
            if(props["name"].ToString()=="办公大楼")
            {
                if (_b1) _b1.SetActive(true);
                _b1Txt.text = props["name"].ToString();
            }
            else if (props["name"].ToString() == "生产车间")
            {
                if (_b2) _b2.SetActive(true);
                _b2Txt.text = props["name"].ToString();
            }
            else
            {
                if (_b3) _b3.SetActive(true);
                _b3Txt.text = props["name"].ToString();
                if (props["name"].ToString() == "员工宿舍")
                {
                    _b4Txt.text ="集团福利分配住房";
                }
                else
                {
                    _b4Txt.text = "私人公寓";
                }
                 
            }
            _textMesh.text = props["name"].ToString();
        }
        else if (props.ContainsKey("house_num"))
        {
            _textMesh.text = props["house_num"].ToString();
        }
        else if (props.ContainsKey("type"))
        {
            _textMesh.text = props["type"].ToString();
        }
        if (props.ContainsKey("routeIndex"))
        {
            //string n = props["routeIndex"].ToString();
            int m = Convert.ToInt32(props["routeIndex"]);
            //string n = props["routeIndex"].ToString();
            //Debug.Log("======= directionsFactory.AddPoint");
           directionsFactory.AddPoint(m, this.transform);
            //if (props["routetype"].ToString() == "startpoint")
            //{
            //    directionsFactory.addStartPoint(this.transform);
            //} else if (props["routetype"].ToString() == "endpoint")
            //{
            //    directionsFactory.addEndPoint(this.transform);
            //};
        }
    }
}
