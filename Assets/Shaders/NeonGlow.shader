// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:830,x:33359,y:32523,varname:node_830,prsc:2|emission-7358-OUT;n:type:ShaderForge.SFN_Lerp,id:7358,x:33116,y:32627,varname:node_7358,prsc:2|A-3699-RGB,B-9384-OUT,T-1016-OUT;n:type:ShaderForge.SFN_Color,id:3699,x:32822,y:32522,ptovrint:False,ptlb:BaseColor,ptin:_BaseColor,varname:node_3699,prsc:2,glob:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:9384,x:32810,y:32705,varname:node_9384,prsc:2|A-9948-RGB,B-5937-OUT;n:type:ShaderForge.SFN_Color,id:9948,x:32549,y:32712,ptovrint:False,ptlb:GlowColor,ptin:_GlowColor,varname:node_9948,prsc:2,glob:False,c1:1,c2:0.3970588,c3:0.3970588,c4:1;n:type:ShaderForge.SFN_ValueProperty,id:5937,x:32542,y:32912,ptovrint:False,ptlb:GlowIntensity,ptin:_GlowIntensity,varname:node_5937,prsc:2,glob:False,v1:2.23;n:type:ShaderForge.SFN_Multiply,id:1016,x:33111,y:32908,varname:node_1016,prsc:2|A-1394-OUT,B-6694-OUT;n:type:ShaderForge.SFN_Slider,id:6694,x:32954,y:33117,ptovrint:False,ptlb:Slider,ptin:_Slider,varname:node_6694,prsc:2,min:0,cur:1.74359,max:3;n:type:ShaderForge.SFN_OneMinus,id:1394,x:32862,y:32932,varname:node_1394,prsc:2|IN-4499-OUT;n:type:ShaderForge.SFN_Fresnel,id:4499,x:32646,y:33001,varname:node_4499,prsc:2|NRM-3154-OUT,EXP-289-OUT;n:type:ShaderForge.SFN_Vector1,id:289,x:32680,y:33208,varname:node_289,prsc:2,v1:1;n:type:ShaderForge.SFN_NormalVector,id:3154,x:32435,y:33012,prsc:2,pt:False;proporder:3699-9948-5937-6694;pass:END;sub:END;*/

Shader "Shader Forge/NeonGlow" {
    Properties {
        _BaseColor ("BaseColor", Color) = (0.5,0.5,0.5,1)
        _GlowColor ("GlowColor", Color) = (1,0.3970588,0.3970588,1)
        _GlowIntensity ("GlowIntensity", Float ) = 2.23
        _Slider ("Slider", Range(0, 3)) = 1.74359
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _BaseColor;
            uniform float4 _GlowColor;
            uniform float _GlowIntensity;
            uniform float _Slider;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 emissive = lerp(_BaseColor.rgb,(_GlowColor.rgb*_GlowIntensity),((1.0 - pow(1.0-max(0,dot(i.normalDir, viewDirection)),1.0))*_Slider));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
