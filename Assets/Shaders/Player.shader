﻿Shader "Sprites/Player"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite", 2D) = "white" {}

	}


	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"RenderType" = "TransparentCutOut"
			"IgnoreProjector" = "True"
			"PreviewType" = "Plane"
			"DisableBatching" = "True"
			"ForceNoShadowCasting" = "True"
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

			struct Input
			{
				float2 uv_MainTex;
			};

			fixed4 _ColorTint;
			

			struct appdata
			{
				float4 vertex	: POSITION;
				float4 color	: COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex	: SV_POSITION;
				fixed4 color	: COLOR;
				float2 texcoord : TEXCOORD0;
			};


			sampler2D _MainTex;


			v2f vert(appdata IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;

				float4 pos = OUT.vertex;
				float2 hpc = _ScreenParams.xy * 0.5f;
				float2 pixelPos = round((pos.xy / pos.w) * hpc);
				pos.xy = pixelPos / hpc * pos.w;
				OUT.vertex = pos;

				return OUT;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, IN.texcoord);
				c.rgb *= c.a;
				return c;
			}

			ENDCG
		}

	}


}