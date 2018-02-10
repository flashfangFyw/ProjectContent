Shader "CustomMobile/OcclutoinMaterial"
{
	 Properties {
    _MainTex ("Texture", 2D) = "white" {}
    [NoScaleOffset] _BumpMap ("Normalmap", 2D) = "bump" {}
	//_Points("points" , float3[]) 
	 _Points_Bottom("y_Range" , Float) = 10.0
    //_Z_Range("Z_Range" , Float) = 0.0
    //_X_Range("X_Range" , Float) = 0.0
    _Color("Color", Color) = (1,1,1,1)
    //_TransitLineVal("TransitLineVal",Range(0,0.1)) = 0.02
    }

SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 250
    Cull Off
CGPROGRAM
#pragma surface surf Lambert noforwardadd
//#pragma surface surf Standard fullforwardshadows
//#pragma surface surf Lambert
#pragma target 3.0

sampler2D _MainTex;
sampler2D _BumpMap;
uniform float4 _Points[100];  // 数组变量
uniform float _Points_Num;  // 数组长度变量
uniform float _Points_Bottom;  // 底面
//float _Z_Range;
//float _X_Range;
//half _TransitLineVal;
fixed4 _Color;

struct Input {
    float2 uv_MainTex;
    float3 worldPos;
};
bool Contains(float3 worldPos)
{
	bool result = false;
	// 遍历
	for (int i=0; i<_Points_Num; i++)
	{
		float4 p4 = _Points[i]; // 索引取值
		// 自定义处理
		if ( (((_Points[i + 1].z <= worldPos.z) && (worldPos.z < _Points[i].z))
                        ||
                         ((_Points[i].z <= worldPos.z) && (worldPos.z < _Points[i + 1].z)))
                          &&
                        (worldPos.x < (_Points[i].x - _Points[i + 1].x) * (worldPos.z - _Points[i + 1].z) / (_Points[i].z - _Points[i + 1].z) + _Points[i + 1].x)
                        )
                {
                result = !result;
            }
	}
	return result;
}
bool rayCasting(float3 worldPos)
{
	float3 p=worldPos;
	float px = p.x;
    float py = p.z;
    bool  flag = false;
	 for (int i = 0, l = 4, j = l - 1; i < l; j = i, i++)
    {
        float sx = _Points[i].x;
        float sy = _Points[i].z;
        float tx = _Points[j].x;
        float ty = _Points[j].z;

      // 点与多边形顶点重合
        if ((sx == px && sy == py) || (tx == px && ty == py))
        {
            return false;
		}
        // 判断线段两端点是否在射线两侧
        if ((sy < py && ty >= py) || (sy >= py && ty < py))
        {
            // 线段上与射线 Y 坐标相同的点的 X 坐标
            float x = sx + (py - sy) * (tx - sx) / (ty - sy);
			// 点在多边形的边上
            if (x == px)  return false;
            // 射线穿过多边形的边界
            if (x > px)  flag = !flag;
        }
    }
    // 射线穿过多边形边界的次数为奇数时点在多边形内
    return flag;// ? 'in' : 'out'

}
bool CheckBottom(float py)
{
	return py>_Points_Bottom;
}
int pnpoly(float3 worldPos)//int nvert, float *vertx, float *verty, float testx, float testy)
{
	 bool inside = false;
    for ( int i = 0, j =3 ; i < 4 ; j = i++ )
    {
        if ( ( _Points[ i ].z > worldPos.z ) != ( _Points[ j ].z > worldPos.z ) &&
             worldPos.x < ( _Points[ j ].x - _Points[ i ].x ) * ( worldPos.z - _Points[ i ].z ) / ( _Points[ j ].z - _Points[ i ].z ) + _Points[ i ].x )
        {
            inside = !inside;
        }
    }

    return inside;
}

void surf (Input IN, inout SurfaceOutput o) 
{
    // (IN.worldPos.y <= _EffectTime+ _TransitLineVal
     //&& IN.worldPos.y >= _BottomValue- _TransitLineVal )
	 //=======================================================
	  // if ((IN.worldPos.x <= _X_Range
		//	 && IN.worldPos.x >= -_X_Range )
		//	 &&
//(
		//	 IN.worldPos.z <= _Z_Range
//&& IN.worldPos.z >= -_Z_Range
		//	 ))
			//if(Contains(IN.worldPos)){
			//if(pnpoly(IN.worldPos))
			if(rayCasting(IN.worldPos)&&CheckBottom(IN.worldPos.y)){
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) ;
            o.Albedo = c.rgb*_Color;
			 //o.Albedo = _Color;
            o.Alpha = c.a;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			}else{discard;}
}
ENDCG
}

FallBack "Mobile/Diffuse"
}
