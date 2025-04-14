Shader "Custom/WaterShader"
{
    Properties
    {
        _NormalMap1 ("Normal Map 1", 2D) = "bump" {}
        _NormalMap2 ("Normal Map 2", 2D) = "bump" {}
        _NormalSpeed1 ("Normal Speed 1", Vector) = (0.1, 0.1, 0, 0)
        _NormalSpeed2 ("Normal Speed 2", Vector) = (-0.05, 0.05, 0, 0)
        _NormalStrength ("Normal Strength", Range(0,1)) = 0.5
        _Distortion ("Distortion Amount", Range(0,1)) = 0.05
        _BaseColor ("Base Color", Color) = (0.2, 0.5, 0.7, 0.5)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 screenPos : TEXCOORD1;
            };

            float4 _Time;


            sampler2D _NormalMap1;
            sampler2D _NormalMap2;
            float4 _NormalMap1_ST;
            float4 _NormalMap2_ST;
            float4 _NormalSpeed1;
            float4 _NormalSpeed2;
            float _NormalStrength;
            float _Distortion;
            float4 _BaseColor;

            sampler2D _CameraOpaqueTexture;
            float4 _CameraOpaqueTexture_TexelSize;

            float4x4 UNITY_MATRIX_MVP;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.uv;
                o.screenPos = o.vertex;
                return o;
            }

            float3 UnpackNormalRGorAG (float4 packedNormal)
            {
                float3 normal;
                normal.xy = packedNormal.xy * 2 - 1;
                normal.z = sqrt(1.0 - saturate(dot(normal.xy, normal.xy)));
                return normal;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 scrollUV1 = i.uv + _Time.y * _NormalSpeed1.xy;
                float2 scrollUV2 = i.uv + _Time.y * _NormalSpeed2.xy;

                float3 n1 = UnpackNormalRGorAG(tex2D(_NormalMap1, scrollUV1));
                float3 n2 = UnpackNormalRGorAG(tex2D(_NormalMap2, scrollUV2));
                float3 normal = normalize(n1 + n2) * _NormalStrength;

                float2 offset = normal.xy * _Distortion;

                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                screenUV = screenUV * 0.5 + 0.5; // NDC to UV
                screenUV += offset * _CameraOpaqueTexture_TexelSize.xy;

                float4 bg = tex2D(_CameraOpaqueTexture, screenUV);
                float4 finalColor = lerp(_BaseColor, bg, 0.7);
                finalColor.a = _BaseColor.a;

                return finalColor;
            }
            ENDHLSL
        }
    }
}
