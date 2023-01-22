﻿Shader "Unlit/UVtest"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float mod(float x, float y)
			{
				return x - y * floor(x / y);
			}
			float2 mod(float2 xy, float m) {
				return float2(mod(xy.x,m),mod(xy.y,m));
			}
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				return fixed4(mod(i.uv,1),0,1);
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
            }
            ENDCG
        }
    }
}
