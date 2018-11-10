// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/Noise"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_NoiseColour("Noise Colour", Color) = (1,1,1,1)
		_NoiseIntensity("Noise Intensity", Range(0,1)) = 0.02
		_NoiseScaling("Noise Scaling", Int) = 100

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
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

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

			Pass
			{
				Name "Default"
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"
				#include "UnityUI.cginc"

				#pragma multi_compile __ UNITY_UI_CLIP_RECT
				#pragma multi_compile __ UNITY_UI_ALPHACLIP

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
					float2 texcoord  : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				fixed4 _Color;
				fixed4 _TextureSampleAdd;
				float4 _ClipRect;
				float4 _MainTex_ST;

				fixed4 _NoiseColour;
				float _NoiseIntensity;
				int _NoiseScaling;

				float rand(float2 co, float time)
				{
					return frac((sin(dot(co.xy, float2(12.345 * time, 67.890 * time))) * 12345.67890 + time));
				}

				v2f vert(appdata_t v)
				{
					v2f OUT;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
					OUT.worldPosition = v.vertex;
					OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

					OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

					OUT.color = v.color * _Color;
					return OUT;
				}

				float withinRange(float min, float max, float val) {
					float stepVal = (step(min, val) - step(max, val));
					return val - 2.0*stepVal*val + stepVal;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					float2 alteredTexcoord = IN.texcoord;
					float distortLine = (_Time.w / 4) % 1;
					float distortLine2 = (_Time.w / 6) % 1;

					float distort = (0.08 * withinRange(distortLine - 0.03, distortLine + 0.03, alteredTexcoord.y)) + (0.04 * withinRange(distortLine2 - 0.03, distortLine2 + 0.03, alteredTexcoord.y));
					
					alteredTexcoord.x += distort;

					int2 noiseCoord = IN.texcoord*_NoiseScaling;
					float time = (round(_Time.w/0.3))*0.3;

					half4 color = (tex2D(_MainTex, alteredTexcoord) + _TextureSampleAdd) * IN.color;
					float noise = ((rand(noiseCoord, time) - 0.5) * 2)+ (_NoiseIntensity * _NoiseIntensity);
					color = lerp(color, _NoiseColour, ((noise*_NoiseIntensity)-(1- _NoiseIntensity))*_NoiseColour.a);

					#ifdef UNITY_UI_CLIP_RECT
					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
					#endif

					#ifdef UNITY_UI_ALPHACLIP
					clip(color.a - 0.001);
					#endif

					return color;
				}
			ENDCG
			}
		}
}
