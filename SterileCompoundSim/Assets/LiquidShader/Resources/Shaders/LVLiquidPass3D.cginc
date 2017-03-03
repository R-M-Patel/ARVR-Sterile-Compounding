#include "LVLiquidPassBase.cginc"

		sampler3D _NoiseTex;
		float _SparklingIntensity, _SparklingThreshold;
		
		half4 raymarch(float4 vertex, float3 rd, float t0, float t1) {

	        float3 wpos = wsCameraPos + rd * t0;
	        
			float turbulence = (tex2D(_NoiseTex2D, float2(vertex.x, vertex.z) * 0.1.xx * _Turbulence.xx + _Time.xx).g - 0.5) * _Turbulence.x;
			turbulence += sin(vertex.w) * _Turbulence.y;
			turbulence *= 0.05 * _Size.y * _FoamTurbulence;
			_LevelPos += turbulence;
			_FoamMaxPos += turbulence;
			
			// compute levels of liquid (t2) & foam (t3)
			float delta = length(rd.xz) / rd.y;
			float h = wpos.y - _LevelPos;
			float w = h * delta;
			float t2 = t0 + sqrt(h*h + w*w);
						
			// compute foam level (t3)
			float hf = wpos.y - _FoamMaxPos;
			float wf = hf * delta;
			float t3 = t0 + sqrt(hf*hf + wf*wf);				
			
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
					half n = tex3Dlod(_NoiseTex, (rpos - disp) * _Scale.x).r;
					half4 lc  = half4(_SmokeColor.rgb, n * _SmokeColor.a);
					lc.rgb *= lc.aaa;
					lc *= deep;
					sumSmoke += lc * (1.0-sumSmoke.a);
				}
			}
			
			// ray-march foam
			tmax = min(t3,t1), tmin = t0;
			if (wpos.y > _FoamMaxPos) {
				if (rd.y>=0) {
					tmax = -99999.0;
				} else {
					tmin = tmax;
					tmax = min(t2, t1);
				}
			} else if (wpos.y < _LevelPos) {
				if (rd.y<0) {
					tmax = -99999.0;
				} else {
					tmin = min(t2,t1);
					tmax *= _FoamBottom;
				}
			} else if (rd.y<0) {
				tmax = min(t2, t1);
			}
			half4 sumFoam  = half4(0,0,0,0);
			if (tmax>tmin) {
				float stepSize =  (tmax - tmin) / (float)_FoamRaySteps;
				float4 dir  = float4(rd * stepSize, 0);
				float4 rpos = float4(wsCameraPos + rd * tmin, 0);
				float4 disp = float4(_Time.x, 0, _Time.x, 0) * _Turbulence.x * _Radius * _FoamTurbulence;
				float foamThickness = _FoamMaxPos - _LevelPos;
				for (int k=_FoamRaySteps;k>0;k--, rpos += dir) {
					float h = saturate( (rpos.y - _LevelPos) / foamThickness );
					float n = saturate(tex3Dlod(_NoiseTex, (rpos - disp) * _Scale.y ).r + _FoamDensity);
					if (n>h) {
						half4 lc  = half4(_FoamColor.rgb, n-h);
						lc.a   *= _FoamColor.a;
						lc.rgb *= lc.aaa;
						float deep = saturate((rpos.y-_LevelPos) * _FoamWeight / foamThickness);
						lc *= deep;
						sumFoam += lc * (1.0 - sumFoam.a);
					}
				}
				sumFoam *= 1.0 + _FoamDensity;
			}	
			
			// ray-march liquid
			tmax = min(t2,t1), tmin = t0;
			if (wpos.y > _LevelPos) {
				if (rd.y<0) {
					tmin = t2;
					tmax = t1;
				} else {
					tmax = -99999.0;
				}
			}
			half4 sum = half4(0,0,0,0);
			if (tmax>tmin) {
				float stepSize = (tmax-tmin) / (float)_LiquidRaySteps;
				float4 dir   = float4(rd * stepSize, 0);
				float4 rpos  = float4(wsCameraPos + rd * tmin, 0);	// does not matter to move to level pos
				float4 disp  = float4(_Time.x * _Turbulence.y, _Time.x * 1.5, _Time.x * _Turbulence.y, 0) * (_Turbulence.y + _Turbulence.x) * _Size.y;
				float4 disp2 = float4(0,_Time.x*2.5* (_Turbulence.y + _Turbulence.x) * _Size.y,0,0);
				for (int k=_LiquidRaySteps;k>0;k--, rpos += dir) {
					float deep = exp(((rpos.y - _LevelPos)/_Size.y) * _DeepAtten);
					half n = tex3Dlod(_NoiseTex, (rpos - disp) * _Scale.z).r;
					half4 lc  = half4(_Color1.rgb, (1.0 - _Muddy) + n * _Muddy);
					lc.a *= _Color1.a;
					lc.rgb *= lc.aaa;
					lc.rgb *= deep;
					sum += lc * (1.0-sum.a);
					
					n =  tex3Dlod(_NoiseTex, (rpos - disp2) * _Scale.w ).r;
					lc  = half4(_Color2.rgb + max(n-_SparklingThreshold, 0) * _SparklingIntensity, (1.0 - _Muddy) + n * _Muddy);
					lc.a *= _Color2.a;
					lc.rgb *= lc.aaa;
					lc.rgb *= deep;
					sum += lc * (1.0-sum.a);
				}
			}

			// Final blend
			if (wpos.y>_LevelPos) {
				float4 lfoam = sumFoam * (1.0 - sumSmoke.a);
				float4 liquid = sum * (1.0 - lfoam.a);
				sum = sumSmoke + lfoam + liquid;
			} else {
				float4 lfoam = sumFoam * (1.0 - sum.a);
				float4 lsmoke = sumSmoke * (1.0 - lfoam.a);
				sum = sum + lsmoke + lfoam;
			}
			sum *= _Alpha;
			return sum;
		}
