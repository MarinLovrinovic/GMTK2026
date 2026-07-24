Shader "Custom/SimpleWater"
{
    Properties
    {
        _WaterColor ("Water Color", Color) = (0.2,0.5,0.8,0.5)
        _MainTex ("Normal Texture", 2D) = "bump" {}
        _WaveSpeed ("Wave Speed", Float) = 0.2
        _WaveStrength ("Wave Strength", Float) = 0.03
        _Gloss ("Gloss", Range(8,128)) = 32
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _WaterColor;
            float _WaveSpeed;
            float _WaveStrength;
            float _Gloss;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
            };

            v2f vert(appdata v)
            {
                v2f o;

                float wave =
                    sin(v.vertex.x * 2 + _Time.y * _WaveSpeed * 5) *
                    cos(v.vertex.z * 2 + _Time.y * _WaveSpeed * 5);

                v.vertex.y += wave * _WaveStrength;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                uv.x += sin(_Time.y * _WaveSpeed + uv.y * 8) * 0.01;
                uv.y += cos(_Time.y * _WaveSpeed + uv.x * 8) * 0.01;

                fixed3 normalTex = UnpackNormal(tex2D(_MainTex, uv));

                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 normal = normalize(i.worldNormal + normalTex * 0.3);

                float diffuse = saturate(dot(normal, lightDir));

                fixed3 color = _WaterColor.rgb * (0.4 + diffuse * 0.6);

                return fixed4(color, _WaterColor.a);
            }
            ENDCG
        }
    }

    FallBack "Transparent/Diffuse"
}