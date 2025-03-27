// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.21 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.21;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:2,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32701,y:32821,varname:node_1,prsc:2|custl-1494-OUT,voffset-1499-OUT;n:type:ShaderForge.SFN_ViewVector,id:1486,x:31161,y:33128,varname:node_1486,prsc:2;n:type:ShaderForge.SFN_Color,id:1488,x:31898,y:32427,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7418,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_NormalVector,id:1489,x:31161,y:32978,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:1490,x:31443,y:33059,varname:node_1490,prsc:2,dt:0|A-1489-OUT,B-1486-OUT;n:type:ShaderForge.SFN_Clamp01,id:1491,x:31684,y:33059,varname:node_1491,prsc:2|IN-1490-OUT;n:type:ShaderForge.SFN_Power,id:1492,x:31898,y:33046,cmnt:Glow,varname:node_1492,prsc:2|VAL-1491-OUT,EXP-1930-OUT;n:type:ShaderForge.SFN_Multiply,id:1494,x:32134,y:32730,cmnt:Final Glow,varname:node_1494,prsc:2|A-1488-RGB,B-1501-OUT,C-1492-OUT;n:type:ShaderForge.SFN_Vector1,id:1497,x:31657,y:33230,varname:node_1497,prsc:2,v1:15;n:type:ShaderForge.SFN_NormalVector,id:1498,x:32159,y:33145,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:1499,x:32410,y:33145,varname:node_1499,prsc:2|A-1498-OUT,B-1512-OUT;n:type:ShaderForge.SFN_Slider,id:1501,x:31532,y:32563,ptovrint:False,ptlb:Fall Off,ptin:_FallOff,varname:node_3664,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:2;n:type:ShaderForge.SFN_RemapRange,id:1512,x:32172,y:33332,varname:node_1512,prsc:2,frmn:0,frmx:1,tomn:0.01,tomx:0.03|IN-1520-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1520,x:31933,y:33383,ptovrint:False,ptlb:Size,ptin:_Size,varname:node_8143,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.025;n:type:ShaderForge.SFN_Slider,id:1930,x:31467,y:33378,ptovrint:False,ptlb:Edge,ptin:_Edge,varname:node_1930,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:15;proporder:1488-1520-1501-1930;pass:END;sub:END;*/

Shader "TowerDefenseKit/CrystalHalo_URP" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _Size ("Size", Float ) = 0.025
        _FallOff ("Fall Off", Range(0, 2)) = 1
        _Edge ("Edge", Range(0, 15)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags { "LightMode" = "UniversalForward" }
            Blend One One
            ZWrite Off
            Cull Back
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            struct VertexInput {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };
            struct VertexOutput {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };
            uniform float4 _Color;
            uniform float _Size;
            uniform float _FallOff;
            uniform float _Edge;
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                float3 positionWS = TransformObjectToWorld(v.positionOS.xyz + v.normalOS * (_Size * 0.02 + 0.01));
                o.positionCS = TransformWorldToHClip(positionWS);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.viewDirWS = normalize(GetCameraPositionWS() - positionWS);
                return o;
            }
            half4 frag (VertexOutput i) : SV_Target {
                float3 normalWS = normalize(i.normalWS);
                float3 viewDirWS = normalize(i.viewDirWS);
                float glow = pow(saturate(dot(normalWS, viewDirWS)), _Edge);
                float3 finalColor = _Color.rgb * _FallOff * glow;
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/InternalErrorShader"
}
