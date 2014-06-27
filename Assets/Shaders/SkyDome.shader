// Shader created with Shader Forge Beta 0.25 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.25;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0;n:type:ShaderForge.SFN_Final,id:1,x:32463,y:32823|diff-7-OUT,emission-2-RGB;n:type:ShaderForge.SFN_Tex2d,id:2,x:33140,y:32637,ptlb:SkyTexture,tex:b8fc6910ea410f84483dbf8e2180600c,ntxv:0,isnm:False|UVIN-353-UVOUT;n:type:ShaderForge.SFN_Add,id:7,x:32809,y:32946|A-2-RGB,B-339-OUT;n:type:ShaderForge.SFN_Tex2d,id:8,x:33702,y:32934,tex:0d451e0424e7f6141b369f988106d7f7,ntxv:0,isnm:False|UVIN-65-UVOUT,TEX-28-TEX;n:type:ShaderForge.SFN_Time,id:19,x:34320,y:33249;n:type:ShaderForge.SFN_Add,id:25,x:33279,y:33054|A-50-OUT,B-51-OUT;n:type:ShaderForge.SFN_Multiply,id:26,x:34125,y:33336|A-19-TSL,B-229-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:28,x:34577,y:33010,ptlb:CloudsTexture,tex:0d451e0424e7f6141b369f988106d7f7;n:type:ShaderForge.SFN_Tex2d,id:33,x:33702,y:33101,tex:0d451e0424e7f6141b369f988106d7f7,ntxv:0,isnm:False|UVIN-142-UVOUT,TEX-28-TEX;n:type:ShaderForge.SFN_Multiply,id:35,x:34123,y:32869|A-37-TSL,B-221-OUT;n:type:ShaderForge.SFN_Time,id:37,x:34320,y:32782;n:type:ShaderForge.SFN_OneMinus,id:50,x:33490,y:33007|IN-8-R;n:type:ShaderForge.SFN_OneMinus,id:51,x:33481,y:33167|IN-33-G;n:type:ShaderForge.SFN_Rotator,id:65,x:33890,y:32759|UVIN-67-UVOUT,ANG-35-OUT;n:type:ShaderForge.SFN_TexCoord,id:67,x:34320,y:32634,uv:0;n:type:ShaderForge.SFN_Rotator,id:142,x:33921,y:33176|UVIN-143-UVOUT,ANG-26-OUT;n:type:ShaderForge.SFN_TexCoord,id:143,x:34320,y:33100,uv:0;n:type:ShaderForge.SFN_Slider,id:221,x:34299,y:32933,ptlb:CloudSpeed_R,min:0,cur:0.06015038,max:1;n:type:ShaderForge.SFN_Slider,id:229,x:34301,y:33422,ptlb:CloudSpeed_G,min:0,cur:0.07518797,max:1;n:type:ShaderForge.SFN_Multiply,id:339,x:33076,y:33089|A-25-OUT,B-340-OUT;n:type:ShaderForge.SFN_Vector1,id:340,x:33260,y:33201,v1:0.75;n:type:ShaderForge.SFN_Rotator,id:353,x:33552,y:32446|UVIN-370-UVOUT,ANG-368-OUT;n:type:ShaderForge.SFN_Time,id:367,x:34028,y:32498;n:type:ShaderForge.SFN_Multiply,id:368,x:33792,y:32540|A-367-TSL,B-369-OUT;n:type:ShaderForge.SFN_Vector1,id:369,x:34028,y:32632,v1:0.1;n:type:ShaderForge.SFN_TexCoord,id:370,x:34028,y:32340,uv:0;proporder:2-28-221-229;pass:END;sub:END;*/

Shader "Shader Forge/SkyDome" {
    Properties {
        _SkyTexture ("SkyTexture", 2D) = "white" {}
        _CloudsTexture ("CloudsTexture", 2D) = "white" {}
        _CloudSpeedR ("CloudSpeed_R", Range(0, 1)) = 0.06015038
        _CloudSpeedG ("CloudSpeed_G", Range(0, 1)) = 0.07518797
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
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
            #pragma exclude_renderers xbox360 ps3 flash 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _SkyTexture; uniform float4 _SkyTexture_ST;
            uniform sampler2D _CloudsTexture; uniform float4 _CloudsTexture_ST;
            uniform float _CloudSpeedR;
            uniform float _CloudSpeedG;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz;
////// Emissive:
                float4 node_367 = _Time + _TimeEditor;
                float node_353_ang = (node_367.r*0.1);
                float node_353_spd = 1.0;
                float node_353_cos = cos(node_353_spd*node_353_ang);
                float node_353_sin = sin(node_353_spd*node_353_ang);
                float2 node_353_piv = float2(0.5,0.5);
                float2 node_353 = (mul(i.uv0.rg-node_353_piv,float2x2( node_353_cos, -node_353_sin, node_353_sin, node_353_cos))+node_353_piv);
                float4 node_2 = tex2D(_SkyTexture,TRANSFORM_TEX(node_353, _SkyTexture));
                float3 emissive = node_2.rgb;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_37 = _Time + _TimeEditor;
                float node_65_ang = (node_37.r*_CloudSpeedR);
                float node_65_spd = 1.0;
                float node_65_cos = cos(node_65_spd*node_65_ang);
                float node_65_sin = sin(node_65_spd*node_65_ang);
                float2 node_65_piv = float2(0.5,0.5);
                float2 node_65 = (mul(i.uv0.rg-node_65_piv,float2x2( node_65_cos, -node_65_sin, node_65_sin, node_65_cos))+node_65_piv);
                float4 node_19 = _Time + _TimeEditor;
                float node_142_ang = (node_19.r*_CloudSpeedG);
                float node_142_spd = 1.0;
                float node_142_cos = cos(node_142_spd*node_142_ang);
                float node_142_sin = sin(node_142_spd*node_142_ang);
                float2 node_142_piv = float2(0.5,0.5);
                float2 node_142 = (mul(i.uv0.rg-node_142_piv,float2x2( node_142_cos, -node_142_sin, node_142_sin, node_142_cos))+node_142_piv);
                finalColor += diffuseLight * (node_2.rgb+(((1.0 - tex2D(_CloudsTexture,TRANSFORM_TEX(node_65, _CloudsTexture)).r)+(1.0 - tex2D(_CloudsTexture,TRANSFORM_TEX(node_142, _CloudsTexture)).g))*0.75));
                finalColor += emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _SkyTexture; uniform float4 _SkyTexture_ST;
            uniform sampler2D _CloudsTexture; uniform float4 _CloudsTexture_ST;
            uniform float _CloudSpeedR;
            uniform float _CloudSpeedG;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_367 = _Time + _TimeEditor;
                float node_353_ang = (node_367.r*0.1);
                float node_353_spd = 1.0;
                float node_353_cos = cos(node_353_spd*node_353_ang);
                float node_353_sin = sin(node_353_spd*node_353_ang);
                float2 node_353_piv = float2(0.5,0.5);
                float2 node_353 = (mul(i.uv0.rg-node_353_piv,float2x2( node_353_cos, -node_353_sin, node_353_sin, node_353_cos))+node_353_piv);
                float4 node_2 = tex2D(_SkyTexture,TRANSFORM_TEX(node_353, _SkyTexture));
                float4 node_37 = _Time + _TimeEditor;
                float node_65_ang = (node_37.r*_CloudSpeedR);
                float node_65_spd = 1.0;
                float node_65_cos = cos(node_65_spd*node_65_ang);
                float node_65_sin = sin(node_65_spd*node_65_ang);
                float2 node_65_piv = float2(0.5,0.5);
                float2 node_65 = (mul(i.uv0.rg-node_65_piv,float2x2( node_65_cos, -node_65_sin, node_65_sin, node_65_cos))+node_65_piv);
                float4 node_19 = _Time + _TimeEditor;
                float node_142_ang = (node_19.r*_CloudSpeedG);
                float node_142_spd = 1.0;
                float node_142_cos = cos(node_142_spd*node_142_ang);
                float node_142_sin = sin(node_142_spd*node_142_ang);
                float2 node_142_piv = float2(0.5,0.5);
                float2 node_142 = (mul(i.uv0.rg-node_142_piv,float2x2( node_142_cos, -node_142_sin, node_142_sin, node_142_cos))+node_142_piv);
                finalColor += diffuseLight * (node_2.rgb+(((1.0 - tex2D(_CloudsTexture,TRANSFORM_TEX(node_65, _CloudsTexture)).r)+(1.0 - tex2D(_CloudsTexture,TRANSFORM_TEX(node_142, _CloudsTexture)).g))*0.75));
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
