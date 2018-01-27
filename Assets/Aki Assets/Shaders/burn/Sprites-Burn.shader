Shader "Sprites/Burn"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

		// Burn
		_BurnValue("Burn Value", Range(0,1)) = 0
		_BurnTex("Burn Texture", 2D) = "white" {}
		_BurnBorder("Burn Border", Range(0,1)) = 0.05
		//_BurnColor("Burn Color", Color) = (1,0,0,1)

		_BurnRamp("Burn Ramp", 2D) = "white" {}
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				float2 texcoord1  : TEXCOORD1;
			};
			
			fixed4 _Color;

			

			sampler2D _MainTex;
			sampler2D _AlphaTex;

			fixed _BurnValue;
			sampler2D _BurnTex;
			float4 _BurnTex_ST;

			fixed _BurnBorder;
			//fixed4 _BurnColor;

			sampler2D _BurnRamp;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				//OUT.texcoord2 = IN.texcoord2;
				OUT.texcoord1 = TRANSFORM_TEX(IN.texcoord1, _BurnTex);
				OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				// Burn effect
				fixed burn = tex2D(_BurnTex, IN.texcoord1).r;
				if (burn < _BurnValue * (1. + 0.01))
				{
					discard;
				}
				// Red border
				//else if (burn < _BurnValue * (1. + _BurnBorder))
				float borderThreshold = _BurnValue * (1. + _BurnBorder);
				//float borderThreshold = _BurnValue * (1. + 2 * _BurnBorder) - _BurnBorder;
				if (burn < borderThreshold)
				{
					return tex2D(_BurnRamp, float2((burn - _BurnValue) / (borderThreshold-_BurnValue), 0.5));
					//return _BurnColor;
					//return tex2D(_BurnRamp, float2((burn - _BurnValue)/borderThreshold, 0.5));
				}

				// Normal color
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
