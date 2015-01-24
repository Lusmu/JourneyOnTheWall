// Shader created with Shader Forge Beta 0.23 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.23;sub:START;pass:START;ps:lgpr:1,nrmq:0,limd:0,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,uamb:True,mssp:True,ufog:False,aust:False,igpj:False,qofs:0,lico:1,qpre:1,flbk:,rntp:1,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0;n:type:ShaderForge.SFN_Final,id:1,x:32526,y:32658|emission-3-RGB;n:type:ShaderForge.SFN_Tex2d,id:3,x:32816,y:32754,ptlb:Rendertexture,tex:3d403fe3184a448fa8bc190c7f07f28c,ntxv:0,isnm:False|UVIN-21-OUT;n:type:ShaderForge.SFN_Append,id:21,x:33071,y:32773|A-43-R,B-43-G;n:type:ShaderForge.SFN_Tex2dAsset,id:42,x:33578,y:32784,ptlb:distortion UV,tex:6bd3fbab0f7e4f84f8ca56484b2810b7;n:type:ShaderForge.SFN_Tex2d,id:43,x:33353,y:32746,tex:6bd3fbab0f7e4f84f8ca56484b2810b7,ntxv:0,isnm:False|MIP-113-OUT,TEX-42-TEX;n:type:ShaderForge.SFN_Vector1,id:113,x:33552,y:32705,v1:3;proporder:3-42;pass:END;sub:END;*/

Shader "Custom/dist" {
    Properties {
        _Rendertexture ("Rendertexture", 2D) = "white" {}
        _distortionUV ("distortion UV", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        LOD 200
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers opengl d3d9 xbox360 ps3 flash 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _Rendertexture; uniform float4 _Rendertexture_ST;
            uniform sampler2D _distortionUV; uniform float4 _distortionUV_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float4 uv0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float2 node_122 = i.uv0;
                float4 node_43 = tex2Dlod(_distortionUV,float4(TRANSFORM_TEX(node_122.rg, _distortionUV),0.0,3.0));
                float2 node_21 = float2(node_43.r,node_43.g);
                float3 emissive = tex2D(_Rendertexture,TRANSFORM_TEX(node_21, _Rendertexture)).rgb;
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
