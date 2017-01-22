Shader "Custom/PixelatedBlur" 
{
	Properties 
	{
	}

	Category 
	{

		Tags 
		{ 
			"Queue"="Transparent" 
			"RenderType"="Transparent" 
		}


		SubShader 
		{

			GrabPass 
			{
				"_BackgroundTexture"
			}
		
			Pass 
			{
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct appdata_t 
				{
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;
				};

				struct v2f 
				{
					float4 vertex : SV_POSITION;
					float4 uvgrab : TEXCOORD0;
				};

				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
					#else
					float scale = 1.0;
					#endif
					o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
					o.uvgrab.zw = o.vertex.zw;
					return o;
				}

				sampler2D _BackgroundTexture;

				half4 frag (v2f i) : SV_Target
				{
					float4 uv = UNITY_PROJ_COORD(i.uvgrab);
					uv.x = round(uv.x * 8.0) / 8;
					uv.y = round(uv.y * 8.0) / 8;

					half4 col = tex2Dproj(_BackgroundTexture, uv);
					return col;
				}
				ENDCG
			}
		}

	}

}
