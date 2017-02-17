Shader "Hidden/Fluvio/FluidEffectComposite" {
Properties {
	_MainTex ("Base (RGBA)", 2D) = "white" {}
}
SubShader{

	Pass{
		 ZTest Always
		 Cull Off
		 ZWrite Off
		 Fog { Mode Off }
		 Tags {"Queue" = "Overlay" }
CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma multi_compile _DEPTHTEX _DEPTHNORMALSTEX
	#include "UnityCG.cginc"

	#if _DEPTHNORMALSTEX
	#define DEPTHTEXNAME _CameraDepthNormalsTexture
	#else
	#define DEPTHTEXNAME _CameraDepthTexture
	#endif
	
	sampler2D _MainTex; half4 _MainTex_TexelSize;	
	uniform sampler2D DEPTHTEXNAME; uniform half4 DEPTHTEXNAME_TexelSize;
	
	uniform sampler2D _BGCameraTex; uniform half4 _BGCameraTex_TexelSize;
	
	uniform sampler2D _FluidTex; uniform half4 _FluidTex_TexelSize;
	uniform float _FluidDepth;
	
	float _FluidThreshold;
	float _FluidSpecular;
	float _FluidSpecularScale;
	float _FluidOpacity;
	float4 _FluidTint;
	
	uniform float _CameraOrtho;
		
	struct v2f {
		float4 pos : POSITION;
		float2 uv : TEXCOORD0;
		float2 uv2 : TEXCOORD1;
	};
	// Inverse lerp
	inline float invlerp(float from, float to, float value)
	{
		if (from < to)
		{
			if (value < from)
				return 0.0f;
			if (value > to)
				return 1.0f;
			value -= from;
			value /= to - from;
			return value;
		}
		else
		{
			if (from <= to)
				return 0.0f;
			if (value < to)
				return 1.0f;
			if (value > from)
				return 0.0f;
			else
				return 1.0 - (value - to) / (from - to);
		}
	}

	v2f vert( appdata_img v )
	{
		v2f o;
		o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		float2 uv = v.texcoord;
		float2 uv2 = v.texcoord;
		#ifdef UNITY_HALF_TEXEL_OFFSET
				uv.y += _MainTex_TexelSize.y;
				uv2.y += _FluidTex_TexelSize.y;
		#endif
		#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					uv.y = 1.0 - uv.y;
				if (_FluidTex_TexelSize.y < 0)
					uv2.y = 1.0 - uv2.y;
		#endif

		o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, uv);
		o.uv2 = MultiplyUV(UNITY_MATRIX_TEXTURE0, uv2);
		
		return o;
	}	
	
	float4 frag (v2f i) : COLOR
	{
		// Get depth texture information
		float sceneDepth;

		float4 sceneColorTex = tex2D(_MainTex, i.uv);
		float4 sceneDepthTex = tex2D(DEPTHTEXNAME, i.uv);

		#if _DEPTHNORMALSTEX
		float3 sceneNormal;
		DecodeDepthNormal(sceneDepthTex, sceneDepth, sceneNormal);
		#else
		sceneDepth = lerp(Linear01Depth(sceneDepthTex.r), sceneDepthTex.r, _CameraOrtho);
		#endif

		// Clip effect based on depth
		clip(sceneDepth - _FluidDepth);

		// Get fluid color and alpha
		float4 color = tex2D(_FluidTex, i.uv2) * float4(_FluidTint.rgb,1);
		color.a = invlerp(min(_FluidThreshold,.999), 1, color.a);

		// Texture behind fluid
		float4 bg = tex2D(_BGCameraTex, i.uv);

		bg = lerp(sceneColorTex, bg, color.a);

		// Fake Specular
		float4 spec = pow(color, _FluidSpecular/max(color.a, 0.01)) * _FluidSpecularScale * _FluidTint.a;

		// Final color
		return lerp(color, bg, (1.0f - color.a * (1.0f - _FluidOpacity))) + spec;
	}
	
ENDCG

	}
}
Fallback Off
}
