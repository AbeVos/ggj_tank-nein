// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/SmoothBlurEffect" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SecondaryTex ("Seconday texture", 2D) = "white" {}

		// Pass 1 - Multiply
		//_multiplyMix ("Multiply mix", Range(0,1)) = 0.75
		//_multiplyMix ("Multiply mix", Range(0,1)) = 0.75

	}


	CGINCLUDE
		#include "UnityCG.cginc"

		sampler2D _MainTex;
		//sampler2D _Bloom;
				
		uniform half4 _MainTex_TexelSize;
		uniform half4 _Parameter;

		struct v2f_tap
		{
			float4 pos : SV_POSITION;
			half2 uv20 : TEXCOORD0;
			half2 uv21 : TEXCOORD1;
			half2 uv22 : TEXCOORD2;
			half2 uv23 : TEXCOORD3;
		};			

		v2f_tap vert4Tap ( appdata_img v )
		{
			v2f_tap o;

			o.pos = UnityObjectToClipPos (v.vertex);
        	o.uv20 = v.texcoord + _MainTex_TexelSize.xy;				
			o.uv21 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h,-0.5h);	
			o.uv22 = v.texcoord + _MainTex_TexelSize.xy * half2(0.5h,-0.5h);		
			o.uv23 = v.texcoord + _MainTex_TexelSize.xy * half2(-0.5h,0.5h);		

			return o; 
		}					
		
		fixed4 fragDownsample ( v2f_tap i ) : COLOR
		{				
			fixed4 color = tex2D (_MainTex, i.uv20);
			color += tex2D (_MainTex, i.uv21);
			color += tex2D (_MainTex, i.uv22);
			color += tex2D (_MainTex, i.uv23);
			return color / 4;
		}
	
		// weight curves

		static const half curve[7] = { 0.0205, 0.0855, 0.232, 0.324, 0.232, 0.0855, 0.0205 };  // gauss'ish blur weights

		static const half4 curve4[7] = { half4(0.0205,0.0205,0.0205,0), half4(0.0855,0.0855,0.0855,0), half4(0.232,0.232,0.232,0),
			half4(0.324,0.324,0.324,1), half4(0.232,0.232,0.232,0), half4(0.0855,0.0855,0.0855,0), half4(0.0205,0.0205,0.0205,0) };

		struct v2f_withBlurCoords8 
		{
			float4 pos : SV_POSITION;
			half4 uv : TEXCOORD0;
			half2 offs : TEXCOORD1;
		};	


		v2f_withBlurCoords8 vertBlurHorizontal (appdata_img v)
		{
			v2f_withBlurCoords8 o;
			o.pos = UnityObjectToClipPos (v.vertex);
			
			o.uv = half4(v.texcoord.xy,1,1);
			o.offs = _MainTex_TexelSize.xy * half2(1.0, 0.0) * _Parameter.x;

			return o; 
		}
		
		v2f_withBlurCoords8 vertBlurVertical (appdata_img v)
		{
			v2f_withBlurCoords8 o;
			o.pos = UnityObjectToClipPos (v.vertex);
			
			o.uv = half4(v.texcoord.xy,1,1);
			o.offs = _MainTex_TexelSize.xy * half2(0.0, 1.0) * _Parameter.x;
			 
			return o; 
		}	

		half4 fragBlur8 ( v2f_withBlurCoords8 i ) : COLOR
		{
			half2 uv = i.uv.xy; 
			half2 netFilterWidth = i.offs;  
			half2 coords = uv - netFilterWidth * 3.0;
			half4 color = 0;
  			for( int l = 0; l < 7; l++ )  
  			{
				half4 tap = tex2D(_MainTex, coords);
				color += tap * curve4[l];
				coords += netFilterWidth;
  			}
			return color;
		}

	ENDCG




	SubShader {
		ZTest Off Cull Off ZWrite Off Blend Off
		Fog { Mode off }  

		// === BLUR ========================================
		// 0 (downscale?)
		Pass {
			CGPROGRAM
			
			#pragma vertex vert4Tap
			#pragma fragment fragDownsample
			#pragma fragmentoption ARB_precision_hint_fastest 
		
			ENDCG
		}

		// 1
		Pass
		{
			ZTest Always
			Cull Off
		
			CGPROGRAM 
		
			#pragma vertex vertBlurVertical
			#pragma fragment fragBlur8
			#pragma fragmentoption ARB_precision_hint_fastest 
		
			ENDCG 
		}	
		
		// 2
		Pass
		{		
			ZTest Always
			Cull Off
				
			CGPROGRAM
		
			#pragma vertex vertBlurHorizontal
			#pragma fragment fragBlur8
			#pragma fragmentoption ARB_precision_hint_fastest 
		
			ENDCG
		}	


		// === PASS 3: MULTIPLY ========================================
		// 3
		Pass {
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 

			#include "UnityCG.cginc"

			uniform sampler2D _SecondaryTex;

			

			float4 frag(v2f_img i) : COLOR {
				fixed4 c1 = tex2D(_MainTex, i.uv);
				
//#if UNITY_UV_STARTS_AT_TOP
//					i.uv.y = 1 - i.uv.y;
//#endif

				fixed4 c2 = tex2D(_SecondaryTex, i.uv);
				
				fixed _multiplyMix = _Parameter;

				//float4 result = c1;
				//result.rgb =
				//	c1.rgb * lerp(	float3(1,1,1), c2.rgb, _multiplyMix);
				
				//lerp
				//(
				//	c1.rgb,
				//	c1.rgb * c2.rgb,
				//	_multiplyMix
				//);

				//c1.rgb * (1-_multiplyMix) + c2.rgb * (_multiplyMix)

				// Grain merge
				// http://docs.gimp.org/en/gimp-concepts-layer-modes.html
				// c1.rgb + c2.rgb - float3(0.5,0.5,0.5)
				
				//lerp
				//(
				//	c1.rgb * c2.rgb,
				//	c1.rgb + c2.rgb - float3(0.5,0.5,0.5),
				//	_multiplyMix
				//)


				// Hardlight
				


				// --- if c2 > 0.5
				//float3(1,1,1) - (	float3(1,1,1) - 2 * (c2.rgb - float3(0.5,0.5,0.5))) * (float3(1,1,1)-c1.rgb)
				// --- if c2 < 0.5
				//2 * c2.rgb * c1.rgb


				fixed3 steps = step(float3(0.5,0.5,0.5),	c2.rgb);
				//result.rgb = 
				//float3 hardlight =
				//	(  steps) *	(	float3(1,1,1) - (	float3(1,1,1) - 2 * (c2.rgb - float3(0.5,0.5,0.5))) * (float3(1,1,1)-c1.rgb) * steps	)
				//	+
				//	(1-steps) * (	2 * c2.rgb * c1.rgb	);

				// Optimised
				fixed3 doubleProduct =  2 * c1.rgb * c2.rgb;
				fixed3 hardlight =
					(  steps) *	(	2 * (c1.rgb + c2.rgb) -  doubleProduct	- 1	)
					+
					(1-steps) * (	doubleProduct	);	


				fixed4 result = fixed4 (	lerp(c1.rgb, hardlight.rgb, _multiplyMix),	c1.a);
				//fixed4 result = fixed4 (	c1.rgb * lerp(	float3(1,1,1), hardlight.rgb, _multiplyMix),	c1.a);
				return result;
			}
			ENDCG
		}
	}
}
