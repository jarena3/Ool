// Shader created with Shader Forge v1.10 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.10;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,nrsp:0,limd:1,spmd:1,grmd:0,uamb:True,mssp:True,bkdf:False,rprd:False,enco:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:0,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1799,x:33346,y:32571,varname:node_1799,prsc:2|diff-2874-RGB,normal-5797-RGB,emission-7264-OUT;n:type:ShaderForge.SFN_Vector1,id:6327,x:32596,y:32978,varname:node_6327,prsc:2,v1:3;n:type:ShaderForge.SFN_NormalVector,id:6316,x:32577,y:32708,prsc:2,pt:True;n:type:ShaderForge.SFN_Fresnel,id:8203,x:32827,y:32944,varname:node_8203,prsc:2|NRM-6316-OUT,EXP-6327-OUT;n:type:ShaderForge.SFN_Tex2d,id:2874,x:32741,y:32528,ptovrint:False,ptlb:node_2874,ptin:_node_2874,varname:node_2874,prsc:2,tex:021b2d0006356d842984bbbb8f5ee9ee,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:9689,x:32527,y:33353,ptovrint:False,ptlb:node_9689,ptin:_node_9689,varname:node_9689,prsc:2,tex:8bef1cbbbd44cf64ca9f11244faa4b9e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:7264,x:33021,y:33107,varname:node_7264,prsc:2|A-5668-OUT,B-8203-OUT;n:type:ShaderForge.SFN_Color,id:8324,x:32569,y:33145,ptovrint:False,ptlb:node_8324,ptin:_node_8324,varname:node_8324,prsc:2,glob:False,c1:1,c2:0.8896551,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:5668,x:32808,y:33236,varname:node_5668,prsc:2|A-8324-RGB,B-9689-RGB;n:type:ShaderForge.SFN_Tex2d,id:5797,x:32936,y:32713,ptovrint:False,ptlb:node_5797,ptin:_node_5797,varname:node_5797,prsc:2,tex:fd73f311683fae046af2326de8c3ce77,ntxv:3,isnm:True;proporder:2874-9689-8324-5797;pass:END;sub:END;*/

Shader "Shader Forge/Ice" {
    Properties {
        _node_2874 ("node_2874", 2D) = "white" {}
        _node_9689 ("node_9689", 2D) = "white" {}
        _node_8324 ("node_8324", Color) = (1,0.8896551,0,1)
        _node_5797 ("node_5797", 2D) = "bump" {}
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
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _node_2874; uniform float4 _node_2874_ST;
            uniform sampler2D _node_9689; uniform float4 _node_9689_ST;
            uniform float4 _node_8324;
            uniform sampler2D _node_5797; uniform float4 _node_5797_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _node_5797_var = UnpackNormal(tex2D(_node_5797,TRANSFORM_TEX(i.uv0, _node_5797)));
                float3 normalLocal = _node_5797_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float4 _node_2874_var = tex2D(_node_2874,TRANSFORM_TEX(i.uv0, _node_2874));
                float3 diffuseColor = _node_2874_var.rgb;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float4 _node_9689_var = tex2D(_node_9689,TRANSFORM_TEX(i.uv0, _node_9689));
                float node_6327 = 3.0;
                float3 emissive = ((_node_8324.rgb*_node_9689_var.rgb)*pow(1.0-max(0,dot(normalDirection, viewDirection)),node_6327));
/// Final Color:
                float3 finalColor = diffuse + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _node_2874; uniform float4 _node_2874_ST;
            uniform sampler2D _node_9689; uniform float4 _node_9689_ST;
            uniform float4 _node_8324;
            uniform sampler2D _node_5797; uniform float4 _node_5797_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _node_5797_var = UnpackNormal(tex2D(_node_5797,TRANSFORM_TEX(i.uv0, _node_5797)));
                float3 normalLocal = _node_5797_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _node_2874_var = tex2D(_node_2874,TRANSFORM_TEX(i.uv0, _node_2874));
                float3 diffuseColor = _node_2874_var.rgb;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
