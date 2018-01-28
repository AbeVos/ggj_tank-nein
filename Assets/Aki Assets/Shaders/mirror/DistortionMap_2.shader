// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// unlit, vertex colour, alpha blended
// cull off

Shader "tk2d/Distortion Map (2)" 
{
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_DistortionTex("Distortion", 2D) = "bump" {}
		_Distortion("Distortion Strength", Range(-1,1)) = 1
	}
	
	SubShader
	{
		Tags {"Queue"="Transparent+100" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off Lighting Off Cull Off Fog { Mode Off } Blend SrcAlpha OneMinusSrcAlpha
		LOD 110

		GrabPass{ "_GrabTexture2" }
		
		Pass 
		{
			CGPROGRAM
			#pragma vertex vert_vct
			#pragma fragment frag_mult 
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _GrabTexture2;


			sampler2D _DistortionTex;
			float _Distortion;

			struct vin_vct 
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f_vct
			{
				float4 vertex : SV_POSITION;
				//fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float4 uvgrab : TEXCOORD1;
			};

			v2f_vct vert_vct(vin_vct v)
			{
				v2f_vct o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.color = v.color;
				o.texcoord = v.texcoord;
				o.uvgrab = ComputeGrabScreenPos(o.vertex);
				return o;
			}

			fixed4 frag_mult(v2f_vct i) : SV_Target
			{
				half4 bump = tex2D(_DistortionTex, i.texcoord);
				//half2 distortion = UnpackNormal(bump).rg;
				half2 distortion = bump.rg *2. - 1.;

				i.uvgrab.xy += distortion * _Distortion;

				fixed4 col = tex2Dproj(_GrabTexture2, UNITY_PROJ_COORD(i.uvgrab));
				return col;
			}
			ENDCG
		} 
	}
}
