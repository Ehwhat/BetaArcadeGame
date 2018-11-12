Shader "Unlit/PhaseShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_MainTex2("Second Texture", 2D) = "white" {}
		_Noise("Noise Texture", 2D) = "white" {}
		_Amount("Amount", Range(0,1)) = 0
		_Leway("Leway", Range(0,1)) = 0.05
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
			sampler2D _MainTex2;
			float4 _MainTex2_ST;
			sampler2D _Noise;
			float4 _Noise_ST;
			float _Amount;
			float _Leway;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float noiseModifer = smoothstep(_Amount-_Leway, _Amount+_Leway, tex2D(_Noise, i.uv).r);

				fixed4 col = ((1-noiseModifer) * tex2D(_MainTex, i.uv)) + (noiseModifer * tex2D(_MainTex2, i.uv));
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
