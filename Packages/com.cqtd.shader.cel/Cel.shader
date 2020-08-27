Shader "Unlit/Cel"
{
    Properties
    {
        [HideInInspector] _MainTex("Main Texture", 2D) = "white" {}
        [HideInInspector] _LUTMap ("LUT Tex", 2D) = "white" {}
        [HideInInspector] _BumpMap("Normal", 2D) = "bump" {}
        [HideInInspector] _Color("Color", Color) = (1,1,1,1)
        
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness("Outline", Range(0, 1)) = 0.1
        
        _SpecGloss ("Spec Gloss", Range(0, 1)) = 0.5
        _SpecMultiplier ("Specular Multipier", Range(0, 5)) = 1
        _SpecPower ("Specular Power", Range(0, 1000)) = 500
        
        _SmoothStepParam1 ("Smooth Step Config 1", Range(0, 0.01)) = 0.005
        _SmoothStepParam2 ("SmoothStep Config 2", Range(0, 0.1)) = 0.01
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        // outline
        cull front
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_fog
            
            #include "UnityCG.cginc"
            
            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            half _OutlineThickness;
            float4 _OutlineColor;
            
            v2f vert(appdata_t input) {
                v2f output;
                
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                float3 fNormalizedNormal = normalize(input.normal);
                float3 fOutlinePosition = input.vertex + fNormalizedNormal * _OutlineThickness * 0.1f;
                
                output.vertex = UnityObjectToClipPos(fOutlinePosition);
                
                UNITY_TRANSFER_FOG(output, output.vertex);
                
                return output;
            }
            
            float4 frag(v2f i) : SV_Target 
            {
                float4 col = _OutlineColor;
                UNITY_APPLY_FOG(i.fogCoord, color);
                return col;
            }

            ENDCG
        }
        
        cull back
        // shadow caster
        Pass {
            Tags {"LightMode"="ShadowCaster"}
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_ShadowCaster
            
            #include "UnityCG.cginc"
            
            struct v2f {
                V2F_SHADOW_CASTER;
            };
            
            v2f vert(appdata_base v) {
                v2f o;
                TRANSFER_SHADOW_CASTER_NORMALOFFSET(o);
                return o;
            }
            
            float4 frag(v2f i) : SV_Target {
                SHADOW_CASTER_FRAGMENT(i)
            }
            
            ENDCG
        }
        
        // cel shade
        cull back
        CGPROGRAM                        
        #pragma surface surf CelShade
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_LUTMap;
            float2 uv_BumpMap;
        };
        
        sampler2D _MainTex;
        sampler2D _LUTMap;
        sampler2D _BumpMap;
        
        float4 _Color;
        fixed _SpecGloss;
        fixed _SmoothStepParam1;
        fixed _SmoothStepParam2;
        fixed _SpecMultiplier;
        fixed _SpecPower;
        
        struct SurfaceOutputCustom 
        {
            fixed3 Albedo;
            fixed3 Normal;
            fixed3 Emission;
            half Specular;
            fixed Gloss;
            fixed Alpha;
            
            float3 LUT;
        };
        
        void surf(Input IN, inout SurfaceOutputCustom o)
        {
            float4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = mainTex.rgb;
            o.Alpha = 1.0f;
            
            float4 lutTex = tex2D(_LUTMap, IN.uv_LUTMap);
            o.LUT = lutTex.rgb;
            
            float3 normalTex = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Normal = normalTex;
            o.Gloss = _SpecGloss;
        }
        
        float4 LightingCelShade(SurfaceOutputCustom s, float3 lightDir, float3 viewDir, float atten)
        {
            float3 bandedDiffuse;
            float nDotL = dot(s.Normal, lightDir) * 0.5f + 0.5f;
            
            bandedDiffuse = tex2D(_LUTMap, float2(nDotL, 0.5f)).rgb;
            
            float3 specColor;
            float3 halfVector = normalize(lightDir + viewDir);
            float hDotN = saturate(dot(halfVector, s.Normal));
            float poweredHDotN = pow(hDotN, _SpecPower);
            
            float specSmooth = smoothstep(_SmoothStepParam1, _SmoothStepParam2, poweredHDotN);
            specColor = specSmooth * _SpecMultiplier;
            
            float4 final;
            final.rgb = (s.Albedo * _Color + specColor) * bandedDiffuse * _LightColor0.rgb * atten;
            final.a = s.Alpha;
            
            return final; 
        }
        
        ENDCG
    }
    
    FallBack "Unlit/Texture"
    CustomEditor "CelShaderGUI"
}
