Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _WaveSpeed ("Wave Speed", Float) = 1.0
        _WaveScale ("Wave Scale", Float) = 0.1
        _WaveFrequency ("Wave Frequency", Float) = 1.0
        _ReflectionStrength ("Reflection Strength", Range(0, 1)) = 0.5
        _RefractionStrength ("Refraction Strength", Range(0, 1)) = 0.5
        _Transparency ("Transparency", Range(0, 1)) = 0.5
        _CubeMap ("Reflection Cube Map", Cube) = "" {}
        _RefractionCube ("Refraction Cube Map", Cube) = "" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float3 viewDir : TEXCOORD3;
                float3 reflDir : TEXCOORD4;
                float3 refrDir : TEXCOORD5;
            };

            sampler2D _MainTex;
            sampler2D _NormalMap;
            samplerCUBE _CubeMap;
            samplerCUBE _RefractionCube;
            float _WaveSpeed;
            float _WaveScale;
            float _WaveFrequency;
            float _ReflectionStrength;
            float _RefractionStrength;
            float _Transparency;

            float _WaveTime;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                // 波の高さを計算 (時間と位置に基づく波の動き)
                float wave = sin(v.vertex.x * _WaveFrequency + _WaveTime) * _WaveScale;
                v.vertex.y += wave; // 頂点の高さに波の影響を加える

                // ワールド空間での位置と法線
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));

                // 視点と反射、屈折方向の計算
                float3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                float3 reflDir = reflect(-viewDir, worldNormal);
                float3 refrDir = refract(-viewDir, worldNormal, 1.0/1.33);

                // 計算結果を返す
                o.worldPos = worldPos;
                o.normal = worldNormal;
                o.viewDir = viewDir;
                o.reflDir = reflDir;
                o.refrDir = refrDir;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Fresnel 効果の計算（視点と法線ベクトルの角度に基づく反射強度）
                float fresnel = 1.0 - saturate(dot(normalize(i.viewDir), i.normal));
                fresnel = pow(fresnel, 2.0); // 反射強度を調整するために2乗します

                // 反射の取得
                fixed4 reflectionColor = texCUBE(_CubeMap, i.reflDir);
                
                // 屈折の取得
                fixed4 refractionColor = texCUBE(_RefractionCube, i.refrDir);
                
                // ベーステクスチャの適用
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 反射と屈折を適用
                col.rgb = lerp(col.rgb, reflectionColor.rgb, _ReflectionStrength);
                col.rgb = lerp(col.rgb, refractionColor.rgb, _RefractionStrength);

                // Fresnel効果による透明度調整
                col.a = lerp(_Transparency, 1.0, fresnel);

                return col;
            }
            ENDCG
        }
    }
}
