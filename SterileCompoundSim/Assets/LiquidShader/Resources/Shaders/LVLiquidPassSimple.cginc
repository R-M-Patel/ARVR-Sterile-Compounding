#include "LVLiquidPassBase.cginc"

	    half4 raymarch(float4 vertex, float3 rd, float t0, float t1) {

	        float3 wpos = wsCameraPos + rd * t0;
	        
			float turbulence = (tex2D(_NoiseTex2D, float2(vertex.x, vertex.z) * 0.1 * _Turbulence.x + _Time.xx).g - 0.5) * _Turbulence.x;
			turbulence += sin(vertex.w) *_Turbulence.y;
			turbulence *= 0.05 * _Size.y;
			_LevelPos += turbulence;
			
			// compute level of liquid (t2)
			float delta = length(rd.xz) / rd.y;
			float h = wpos.y - _LevelPos;
			float w = h * delta;
			float t2 = t0 + sqrt(h*h + w*w);
			
			// ray-march smoke
			float tmax = t1, tmin = t0;
			if (wpos.y > _LevelPos) {
				if (rd.y<0) {
					tmax = min(t2,t1);
				}
			} else {
				tmax = -99999.0; 
			}
			half4 sumSmoke = half4(0,0,0,0);
			if (tmax>tmin) {
				float stepSize = (tmax - tmin) / (float)_SmokeRaySteps;
				float4 dir  = float4(rd * stepSize, 0);
				float4 rpos = float4(wsCameraPos + rd * tmin, 0);		
				float4 disp = float4(0, _Time.x * _Turbulence.x * _Size.y * _SmokeSpeed, 0, 0);
				for (int k=_SmokeRaySteps;k>0;k--, rpos += dir) {
					float deep = exp(((_LevelPos - rpos.y)/_Size.y) * _SmokeAtten);
					half4 lc  = _SmokeColor;
					lc.rgb *= lc.aaa;
					lc *= deep;
					sumSmoke += lc * (1.0-sumSmoke.a);
				}
			}
			
			// ray-march liquid
			half4 sum = half4(0,0,0,0);
			tmax = t1, tmin = t0;
			if (wpos.y > _LevelPos) {
				if (rd.y<0) {
					tmin = t2;
					tmax = t1;
					if (t2<t1) sum += 0.1;
				} else {
					tmax = -99999.0;
				}
			} else if (rd.y>0 && t2<t1) sum += 0.1 * _FoamBottom;
			
			if (tmax>tmin) {
				float stepSize = (tmax-tmin) / (float)_LiquidRaySteps;
				float4 dir   = float4(rd * stepSize, 0);
				float4 rpos  = float4(wsCameraPos + rd * tmin, 0);	// does not matter to move to level pos
				float4 disp  = float4(_Time.x * _Turbulence.y, _Time.x * 1.5, _Time.x * _Turbulence.y, 0) * (_Turbulence.y + _Turbulence.x) * _Size.y;
				float4 disp2 = float4(0,_Time.x*2.5* (_Turbulence.y + _Turbulence.x) * _Size.y,0,0);
				for (int k=_LiquidRaySteps;k>0;k--, rpos += dir) {
					float deep = exp(((rpos.y - _LevelPos)/_Size.y) * _DeepAtten);
					
					half4 lc  = _Color1;
					lc.a *= _Color1.a;
					lc.rgb *= lc.aaa;
					lc.rgb *= deep;
					sum += lc * (1.0-sum.a);
					
					lc  = _Color2;
					lc.a *= _Color2.a;
					lc.rgb *= lc.aaa;
					lc.rgb *= deep;
					sum += lc * (1.0-sum.a);
				}
			}
			
			// Final blend
			if (wpos.y>_LevelPos) {
				sum = sum * saturate(1.0 - sumSmoke.a) + sumSmoke;
			} else {
				sum = sumSmoke * saturate(1.0 - sum.a) + sum;
			}
			sum *= _Alpha;
			return sum;
		}

