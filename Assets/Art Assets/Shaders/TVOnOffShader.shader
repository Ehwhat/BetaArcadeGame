Shader "Hidden/TVOnOffShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TVTex("TV Masking Texture", 2D) = "white" {}
		_Amount("Masking Cutout Amount", Range(0,1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _TVTex;
			float _Amount;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				float redAmount = (_Amount *4)-1;
				float redFade = 1-tex2D(_TVTex, i.uv).r;
				float redTransition = pow(max(redFade + redAmount, 0), 20);

				float greenAmount = (_Amount * 4)-2.5;
				float greenFade = 1-tex2D(_TVTex, i.uv).g;
				float greenTransition = pow(max(greenFade + greenAmount, 0), 1);


				col.rgb = (lerp(col.rgb+ _Amount * _Amount, 0, clamp(redTransition, 0, 1)) + lerp(col.rgb+ _Amount * _Amount, 0, clamp(greenTransition, 0, 1)))/2;
				return col;
			}
			ENDCG
		}
	}
}
