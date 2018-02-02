Shader "CustomMobile/OcclutoinMaterial"
{
	 Properties {
    _MainTex ("Texture", 2D) = "white" {}
    [NoScaleOffset] _BumpMap ("Normalmap", 2D) = "bump" {}
	_Points("points" , float3[]) 
    _Z_Range("Z_Range" , Float) = 0.0
    _X_Range("X_Range" , Float) = 0.0
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
#pragma target 3.0

sampler2D _MainTex;
sampler2D _BumpMap;
uniform float4 _Points[100];  // 数组变量
float _Z_Range;
float _X_Range;
//half _TransitLineVal;
fixed4 _Color;

struct Input {
    float2 uv_MainTex;
    float3 worldPos;
};
void Contains

void surf (Input IN, inout SurfaceOutput o) 
{
    // (IN.worldPos.y <= _EffectTime+ _TransitLineVal
     //&& IN.worldPos.y >= _BottomValue- _TransitLineVal )
	   if ((IN.worldPos.x <= _X_Range
			 && IN.worldPos.x >= -_X_Range )
			 &&
			 (
			 IN.worldPos.z <= _Z_Range
			 && IN.worldPos.z >= -_Z_Range
			 ))
    {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) ;
            //if ((IN.worldPos.y >= _EffectTime && IN.worldPos.y <= _EffectTime + _TransitLineVal)||(IN.worldPos.y <= _BottomValue && IN.worldPos.y >= _BottomValue - _TransitLineVal))
            //{
				//o.Emission = _Color ;
            //}
            o.Albedo = c.rgb*_Color;
            o.Alpha = c.a;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
    }else{
        discard;
    }
}
ENDCG
}

FallBack "Mobile/Diffuse"
}
