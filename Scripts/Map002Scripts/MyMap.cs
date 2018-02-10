using UnityEngine;
using System.Collections;
using ffDevelopmentSpace;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;


/* 
    Author:     fyw 
    CreateDate: 2018-02-09 16:40:15 
    Desc:       项目地图设置
*/


public class MyMap : BasicMap
{


    #region public function
    public void SetInitializeOnStart(bool flag)
    {
        _initializeOnStart = flag;
    }
    public void InitMap()
    {
        Initialize(Conversions.StringToLatLon(_latitudeLongitudeString), AbsoluteZoom);
    }
    #endregion

}
