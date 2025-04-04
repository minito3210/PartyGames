Shader "Custom/WaterShaderWithBump"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _ScrollSpeedX ("Scroll Speed X", Float) = 0.1
        _ScrollSpeedY ("Scroll Speed Y", Float) = 0.1
        _BumpScale ("Bump Scale", Float) = 1.0
        _WaveSpeed ("Wave Speed", Float) = 1.0
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
            sampler2D _BumpMap;
            samplerCUBE _CubeMap;
            samplerCUBE _RefractionCube;
            float _ScrollSpeedX;
            float _ScrollSpeedY;
            float _BumpScale;
            float _WaveSpeed;
            float _ReflectionStrength;
            float _RefractionStrength;
            float _Transparency;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // ノーマルマップのスクロール
                float2 scroll = float2(_ScrollSpeedX, _ScrollSpeedY) * _Time.y;
                o.uv += scroll;

                // 法線の計算
                float3 worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                float3 reflDir = reflect(-viewDir, worldNormal);
                float3 refrDir = refract(-viewDir, worldNormal, 1.0/1.33);

                o.worldPos = worldPos;
                o.normal = worldNormal;
                o.viewDir = viewDir;
                o.reflDir = reflDir;
                o.refrDir = refrDir;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // 法線マップから法線情報を取得
                float3 localNormal = UnpackNormalWithScale(tex2D(_BumpMap, i.uv), _BumpScale);
                localNormal = normalize(localNormal);

                // Fresnel 効果の計算
                float fresnel = 1.0 - saturate(dot(normalize(i.viewDir), i.normal));
                fresnel = pow(fresnel, 2.0);

                // 反射の取得
                fixed4 reflectionColor = texCUBE(_CubeMap, i.reflDir);
                
                // 屈折の取得
                fixed4 refractionColor = texCUBE(_RefractionCube, i.refrDir);
                
                // テクスチャ適用
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // 反射と屈折を適用
                col.rgb = lerp(col.rgb, reflectionColor.rgb, _ReflectionStrength);
                col.rgb = lerp(col.rgb, refractionColor.rgb, _RefractionStrength);

                // Fresnel 効果による透明度の調整
                col.a = lerp(_Transparency, 1.0, fresnel);
                
                return col;
            }
            ENDCG
        }
    }
}