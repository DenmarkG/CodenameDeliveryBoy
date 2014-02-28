// Shader created with Shader Forge Beta 0.25 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.25;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0;n:type:ShaderForge.SFN_Final,id:1,x:32036,y:32597|diff-468-OUT,spec-527-OUT,normal-553-OUT;n:type:ShaderForge.SFN_Tex2d,id:6,x:32969,y:32038,tex:e992a8110d4af034b8d32ee771bcc370,ntxv:0,isnm:False|TEX-476-TEX;n:type:ShaderForge.SFN_Tex2dAsset,id:64,x:33028,y:33023,ptlb:NORMAL,tex:0dccdbf2692f3de41ba0aeffd2c47b3c;n:type:ShaderForge.SFN_Tex2d,id:66,x:32708,y:33031,tex:0dccdbf2692f3de41ba0aeffd2c47b3c,ntxv:0,isnm:False|TEX-64-TEX;n:type:ShaderForge.SFN_Power,id:268,x:32755,y:32841|VAL-478-A,EXP-269-OUT;n:type:ShaderForge.SFN_Vector1,id:269,x:32960,y:32887,v1:1.5;n:type:ShaderForge.SFN_Lerp,id:468,x:32505,y:32331|A-6-RGB,B-515-RGB,T-658-OUT;n:type:ShaderForge.SFN_Slider,id:472,x:33595,y:32578,ptlb:DETAIL_BLEND,min:0,cur:0.1804516,max:1;n:type:ShaderForge.SFN_Tex2dAsset,id:476,x:34600,y:32585,ptlb:DIFFUSE,tex:e992a8110d4af034b8d32ee771bcc370;n:type:ShaderForge.SFN_Tex2d,id:478,x:33072,y:32671,tex:e992a8110d4af034b8d32ee771bcc370,ntxv:0,isnm:False|TEX-476-TEX;n:type:ShaderForge.SFN_Tex2d,id:515,x:33151,y:32194,ptlb:DETAIL,tex:8ccd30168ff1ae146a7dfc4c7ec8bf97,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Desaturate,id:526,x:32928,y:32340|COL-515-RGB;n:type:ShaderForge.SFN_Lerp,id:527,x:32343,y:32638|A-268-OUT,B-538-OUT,T-658-OUT;n:type:ShaderForge.SFN_Power,id:538,x:32747,y:32452|VAL-526-OUT,EXP-539-OUT;n:type:ShaderForge.SFN_Vector1,id:539,x:32942,y:32486,v1:2;n:type:ShaderForge.SFN_Normalize,id:553,x:32454,y:33019|IN-66-RGB;n:type:ShaderForge.SFN_Multiply,id:658,x:33353,y:32483|A-1055-OUT,B-472-OUT;n:type:ShaderForge.SFN_Tex2d,id:827,x:33991,y:32206,tex:e992a8110d4af034b8d32ee771bcc370,ntxv:0,isnm:False|TEX-476-TEX;n:type:ShaderForge.SFN_OneMinus,id:1047,x:33787,y:32299|IN-827-A;n:type:ShaderForge.SFN_Power,id:1055,x:33608,y:32391|VAL-1047-OUT,EXP-1056-OUT;n:type:ShaderForge.SFN_Vector1,id:1056,x:33845,y:32444,v1:4;proporder:64-476-515-472;pass:END;sub:END;*/

Shader "Shader Forge/BrickTest" {
    Properties {
        _NORMAL ("NORMAL", 2D) = "bump" {}
        _DIFFUSE ("DIFFUSE", 2D) = "white" {}
        _DETAIL ("DETAIL", 2D) = "white" {}
        _DETAILBLEND ("DETAIL_BLEND", Range(0, 1)) = 0.1804516
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
            uniform sampler2D _NORMAL; uniform float4 _NORMAL_ST;
            uniform float _DETAILBLEND;
            uniform sampler2D _DIFFUSE; uniform float4 _DIFFUSE_ST;
            uniform sampler2D _DETAIL; uniform float4 _DETAIL_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_1062 = i.uv0;
                float3 normalLocal = normalize(UnpackNormal(tex2D(_NORMAL,TRANSFORM_TEX(node_1062.rg, _NORMAL))).rgb);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.xyz;
///////// Gloss:
                float gloss = exp2(0.5*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float4 node_515 = tex2D(_DETAIL,TRANSFORM_TEX(node_1062.rg, _DETAIL));
                float node_658 = (pow((1.0 - tex2D(_DIFFUSE,TRANSFORM_TEX(node_1062.rg, _DIFFUSE)).a),4.0)*_DETAILBLEND);
                float node_527 = lerp(pow(tex2D(_DIFFUSE,TRANSFORM_TEX(node_1062.rg, _DIFFUSE)).a,1.5),pow(dot(node_515.rgb,float3(0.3,0.59,0.11)),2.0),node_658);
                float3 specularColor = float3(node_527,node_527,node_527);
                float3 specular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),gloss) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * lerp(tex2D(_DIFFUSE,TRANSFORM_TEX(node_1062.rg, _DIFFUSE)).rgb,node_515.rgb,node_658);
                finalColor += specular;
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
            uniform sampler2D _NORMAL; uniform float4 _NORMAL_ST;
            uniform float _DETAILBLEND;
            uniform sampler2D _DIFFUSE; uniform float4 _DIFFUSE_ST;
            uniform sampler2D _DETAIL; uniform float4 _DETAIL_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float2 node_1063 = i.uv0;
                float3 normalLocal = normalize(UnpackNormal(tex2D(_NORMAL,TRANSFORM_TEX(node_1063.rg, _NORMAL))).rgb);
                float3 normalDirection =  normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
///////// Gloss:
                float gloss = exp2(0.5*10.0+1.0);
////// Specular:
                NdotL = max(0.0, NdotL);
                float4 node_515 = tex2D(_DETAIL,TRANSFORM_TEX(node_1063.rg, _DETAIL));
                float node_658 = (pow((1.0 - tex2D(_DIFFUSE,TRANSFORM_TEX(node_1063.rg, _DIFFUSE)).a),4.0)*_DETAILBLEND);
                float node_527 = lerp(pow(tex2D(_DIFFUSE,TRANSFORM_TEX(node_1063.rg, _DIFFUSE)).a,1.5),pow(dot(node_515.rgb,float3(0.3,0.59,0.11)),2.0),node_658);
                float3 specularColor = float3(node_527,node_527,node_527);
                float3 specular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),gloss) * specularColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                finalColor += diffuseLight * lerp(tex2D(_DIFFUSE,TRANSFORM_TEX(node_1063.rg, _DIFFUSE)).rgb,node_515.rgb,node_658);
                finalColor += specular;
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
