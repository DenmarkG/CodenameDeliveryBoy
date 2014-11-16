// Shader created with Shader Forge Beta 0.36 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.36;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32719,y:32712|diff-32-OUT,emission-32-OUT;n:type:ShaderForge.SFN_Cubemap,id:2,x:33583,y:32271,ptlb:DawnDusk,ptin:_DawnDusk,cube:e7a5e215a83eb0442bc87412c0055faf,pvfc:0;n:type:ShaderForge.SFN_Lerp,id:3,x:33256,y:32409|A-2-RGB,B-10-RGB,T-8-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8,x:33587,y:32620,ptlb:TimeOfDay,ptin:_TimeOfDay,glob:False,v1:1;n:type:ShaderForge.SFN_Cubemap,id:10,x:33583,y:32447,ptlb:Day,ptin:_Day,cube:7e219a07195b133469f8ca2d57b76eb4,pvfc:0;n:type:ShaderForge.SFN_Lerp,id:18,x:33262,y:32715|A-2-RGB,B-20-RGB,T-8-OUT;n:type:ShaderForge.SFN_Cubemap,id:20,x:33587,y:32768,ptlb:Night,ptin:_Night,cube:27690b32bc9e56f47be16ed1e2cdf162,pvfc:0;n:type:ShaderForge.SFN_Lerp,id:32,x:33032,y:32535|A-3-OUT,B-18-OUT,T-35-OUT;n:type:ShaderForge.SFN_ValueProperty,id:35,x:33262,y:32608,ptlb:DayNight,ptin:_DayNight,glob:False,v1:1;proporder:2-10-20-8-35;pass:END;sub:END;*/

Shader "Shader Forge/TransitionalSkybox" {
    Properties {
        _DawnDusk ("DawnDusk", Cube) = "_Skybox" {}
        _Day ("Day", Cube) = "_Skybox" {}
        _Night ("Night", Cube) = "_Skybox" {}
        _TimeOfDay ("TimeOfDay", Float ) = 1
        _DayNight ("DayNight", Float ) = 1
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
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform samplerCUBE _DawnDusk;
            uniform float _TimeOfDay;
            uniform samplerCUBE _Day;
            uniform samplerCUBE _Night;
            uniform float _DayNight;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb;
////// Emissive:
                float4 node_2 = texCUBE(_DawnDusk,viewReflectDirection);
                float3 node_32 = lerp(lerp(node_2.rgb,texCUBE(_Day,viewReflectDirection).rgb,_TimeOfDay),lerp(node_2.rgb,texCUBE(_Night,viewReflectDirection).rgb,_TimeOfDay),_DayNight);
                float3 emissive = node_32;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * node_32;
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
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform samplerCUBE _DawnDusk;
            uniform float _TimeOfDay;
            uniform samplerCUBE _Day;
            uniform samplerCUBE _Night;
            uniform float _DayNight;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_2 = texCUBE(_DawnDusk,viewReflectDirection);
                float3 node_32 = lerp(lerp(node_2.rgb,texCUBE(_Day,viewReflectDirection).rgb,_TimeOfDay),lerp(node_2.rgb,texCUBE(_Night,viewReflectDirection).rgb,_TimeOfDay),_DayNight);
                finalColor += diffuseLight * node_32;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
