Shader "Unlit/PhaseShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_EdgeColour("Edge Colour", Color) = (1,0,0,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
		_Noise("Noise Texture", 2D) = "white" {}
		_NoiseShiftTex("Noise Shift Texture", 2D) = "white"{}
		_NoiseShift("Noise Shift Amount", Range(-1,1)) = 0
		_WorldScaling("Noise World Scaling", Float) = 1
		_Amount("Amount", Range(0,1)) = 0
		_AmountMin("Amount Minimum", Float) = 0
		_AmountMax("Amount Maximum", Float) = 1
		_Leway("Leway", Range(0,1)) = 0.05
		_EdgeLeway("Edge Leway", Range(0,1)) = 0.07
		_EdgeShift("Edge Shift", Range(0,1)) = 0
		_WorldPos("World Pos", Vector) = (0,0,0,0)
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{

			CGPROGRAM
			#pragma vertex SpriteVert
			#pragma fragment CustomFrag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

#include "UnityCG.cginc"

#ifdef UNITY_INSTANCING_ENABLED

	UNITY_INSTANCING_BUFFER_START(PerDrawSprite)
			// SpriteRenderer.Color while Non-Batched/Instanced.
			UNITY_DEFINE_INSTANCED_PROP(fixed4, unity_SpriteRendererColorArray)
			// this could be smaller but that's how bit each entry is regardless of type
			UNITY_DEFINE_INSTANCED_PROP(fixed2, unity_SpriteFlipArray)
		UNITY_INSTANCING_BUFFER_END(PerDrawSprite)

		#define _RendererColor  UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteRendererColorArray)
		#define _Flip           UNITY_ACCESS_INSTANCED_PROP(PerDrawSprite, unity_SpriteFlipArray)

	#endif // instancing

	CBUFFER_START(UnityPerDrawSprite)
	#ifndef UNITY_INSTANCING_ENABLED
		fixed4 _RendererColor;
		fixed2 _Flip;
	#endif
		float _EnableExternalAlpha;
	CBUFFER_END

		// Material Color.
		fixed4 _Color;

		struct appdata_t
		{
			float4 vertex   : POSITION;
			float4 color    : COLOR;
			float2 texcoord : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f
		{
			float4 vertex   : SV_POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
			float2 worldPos : TEXCOORD1;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
		{
			return float4(pos.xy * flip, pos.z, 1.0);
		}

		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(float, _Amount)
			UNITY_DEFINE_INSTANCED_PROP(float2, _WorldPos)
			UNITY_INSTANCING_BUFFER_END(Props)

		v2f SpriteVert(appdata_t IN)
		{
			v2f OUT;

			UNITY_SETUP_INSTANCE_ID(IN);
			UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

			OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
			OUT.vertex = UnityObjectToClipPos(OUT.vertex);
			OUT.texcoord = IN.texcoord;
			OUT.color = IN.color * _Color * _RendererColor;

			#ifdef PIXELSNAP_ON
			OUT.vertex = UnityPixelSnap(OUT.vertex);
			#endif

			float2 pos = UNITY_ACCESS_INSTANCED_PROP(Props, _WorldPos);
			OUT.worldPos = pos.xy + IN.vertex.xy;

			return OUT;
		}

		sampler2D _MainTex;
		sampler2D _AlphaTex;

		fixed4 SampleSpriteTexture(float2 uv)
		{
			fixed4 color = tex2D(_MainTex, uv);

		#if ETC1_EXTERNAL_ALPHA
			fixed4 alpha = tex2D(_AlphaTex, uv);
			color.a = lerp(color.a, alpha.r, _EnableExternalAlpha);
		#endif

			return color;
		}

			float4 _EdgeColour;
			sampler2D _Noise;
			sampler2D _NoiseShiftTex;
			float4 _NoiseShiftTex_ST;
			float4 _Noise_ST;

			float _AmountMin;
			float _AmountMax;
			float _NoiseShift;
			float _WorldScaling;
			float _Leway;
			float _EdgeLeway;
			float _EdgeShift;

			

			fixed4 CustomFrag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				// sample the texture
				float amount = smoothstep(_AmountMin, _AmountMax, UNITY_ACCESS_INSTANCED_PROP(Props, _Amount));
				float noise = 1-lerp(tex2D(_NoiseShiftTex, i.texcoord.xy).r, tex2D(_Noise, i.worldPos.xy * _WorldScaling).r, _NoiseShift);

				float noiseModifer = smoothstep(amount -_Leway, amount +_Leway, noise -_Leway);
				float edgeAmount = min(amount - _EdgeShift, amount);
				float edgeModifer = smoothstep(edgeAmount - _EdgeLeway, edgeAmount + _EdgeLeway, noise - _EdgeLeway);

				if (UNITY_ACCESS_INSTANCED_PROP(Props, _Amount) >= 1)
					edgeModifer = 0;

				fixed4 c = SampleSpriteTexture(i.texcoord) * i.color;
				c.a = 1-noiseModifer;
				c.rgb = lerp(c.rgb, _EdgeColour, edgeModifer);
				c.rgb *= c.a;
				
				return c;
			}
			ENDCG
		}
	}
}
