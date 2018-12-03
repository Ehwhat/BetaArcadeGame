Shader "Unlit/TankShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _Color("Tint", Color) = (1,1,1,1)
		_EdgeColour("Edge Colour", Color) = (1,0,0,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
		_Amount("Amount", Range(0,1)) = 0
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
			float3 worldPos : TEXCOORD1;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
		{
			return float4(pos.xy * flip, pos.z, 1.0);
		}

		UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(float, _Amount)
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
			OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex);

			#ifdef PIXELSNAP_ON
			OUT.vertex = UnityPixelSnap(OUT.vertex);
			#endif

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

		float SinWave(float x,float amplitude, float length, float speed){
			return sin(((x+ (_Time.x * speed)) * length))*amplitude;
		}

		fixed4 CustomFrag(v2f i) : SV_Target
		{
			UNITY_SETUP_INSTANCE_ID(i);

			float2 coord = i.worldPos.xy*5;
			float2 grid = abs(frac(coord - 0.5) - 0.5) / fwidth(coord*2);
			float l = min(grid.x, grid.y);
			float p = 1.0 - min(l, 1.0);
			float4 gridColour = float4(_EdgeColour.xyz, p);

		float sinWave = (SinWave(i.texcoord.x + 6, 0.1, 15, 4) + SinWave(i.texcoord.x + 32, 0.06, 2, 20) + SinWave(i.texcoord.x + 9, 0.02, 1, 40))/3;
		float sinWave2 = (SinWave(i.texcoord.x + 12, 0.1, 15, 4) + SinWave(i.texcoord.x + 2, 0.06, 2, 20) + SinWave(i.texcoord.x +18, 0.02, 1, 40)) / 3;
			float gridValue = smoothstep(_Amount+0.15, _Amount + 0.2, i.texcoord.y + sinWave2);
			float edgeValue = smoothstep(_Amount-0.02, _Amount-0.01, i.texcoord.y + sinWave);
			float value = smoothstep(_Amount, _Amount + 0.1, i.texcoord.y + sinWave);

			fixed4 c = SampleSpriteTexture(i.texcoord) * i.color;
			c = lerp(lerp(lerp(c, _EdgeColour, edgeValue*c.a), gridColour*c.a, value), float4(_EdgeColour.xyz, 0), gridValue);
			c.rgb *= c.a;
				
			return c;
		}
		ENDCG
		}
	}
}
