// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:9935,x:32719,y:32712,varname:node_9935,prsc:2;pass:END;sub:END;*/

Shader "CustomMobile/MobileScanMat_BumpedDiffuse" {
    Properties {
    _MainTex ("Texture", 2D) = "white" {}
    [NoScaleOffset] _BumpMap ("Normalmap", 2D) = "bump" {}
    _EffectTime("EffectTime" , Float) = 0.0
    _BottomValue("BottomValue" , Float) = 0.0
    _Color("LineColor", Color) = (1,1,1,1)
    _TransitLineVal("TransitLineVal",Range(0,0.1)) = 0.02
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
half _EffectTime;
half _BottomValue;
half _TransitLineVal;
fixed4 _Color;

struct Input {
    float2 uv_MainTex;
    float3 worldPos;
};


void surf (Input IN, inout SurfaceOutput o) 
{
    if (IN.worldPos.y <= _EffectTime+ _TransitLineVal
     && IN.worldPos.y >= _BottomValue- _TransitLineVal )
    {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) ;
            if ((IN.worldPos.y >= _EffectTime && IN.worldPos.y <= _EffectTime + _TransitLineVal)||(IN.worldPos.y <= _BottomValue && IN.worldPos.y >= _BottomValue - _TransitLineVal))
            {
                o.Emission = _Color ;
            }
            o.Albedo = c.rgb;
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