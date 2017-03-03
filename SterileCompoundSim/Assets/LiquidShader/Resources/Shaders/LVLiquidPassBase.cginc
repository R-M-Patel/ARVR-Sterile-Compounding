
		struct Input {
			float3 realPos;
			float4 vertex;
			#if LIQUID_VOLUME_DEPTH_AWARE || LIQUID_VOLUME_IRREGULAR
			float4 screenPos;
			#endif
		};

		sampler2D _NoiseTex2D;
		#if LIQUID_VOLUME_DEPTH_AWARE
		sampler2D _CameraDepthTexture;
		float4x4 _Cam2World;
		#endif
		
		#if LIQUID_VOLUME_IRREGULAR
		sampler2D _VLBackBufferTexture;
		#endif
		
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _FoamColor;
		float _FoamMaxPos;
		int _FoamRaySteps;
		float _FoamDensity;
		float _FoamBottom;
		float _FoamTurbulence;
		float _LevelPos;
		float3 _FlaskThickness;
		float3 _Size;
		float3 _Center;
		float _Muddy;
		half _Alpha;
		float4 _Turbulence;
		float _DeepAtten;
		fixed4 _SmokeColor;
		float _SmokeAtten;
		int _SmokeRaySteps;
		float _SmokeSpeed;
		int _LiquidRaySteps;
		half _GlossinessInt;
		float _FoamWeight;
		float4 _Scale;
		float _Radius;
		float4x4 _Rot;
		float _UpperLimit;
		float3 wsCameraPos;

		half4 LightingWrappedSpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
    	    half3 h = normalize (lightDir + viewDir);
			half NdotL = dot (s.Normal, lightDir);
        	half diff = max (0, NdotL * 0.5 + 0.5);
	        float nh = max (0, dot (s.Normal, h));
    	    float spec = pow (nh, _GlossinessInt);
	        half4 c;
    	    c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
        	c.a = s.Alpha;
        	return c;
    	}

		half4 LightingSimple (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
	        half4 c;
    	    c.rgb = s.Albedo * _LightColor0.rgb * atten;
        	c.a = s.Alpha;
        	return c;
    	}
		
		void intSphere(float3 rd, out float t0, out float t1) {
			float3  d = wsCameraPos - _Center;
		    float   b = dot(rd, d);
    		float   c = dot(d,d) - _Radius * _Radius;
    		float   t = sqrt(b*b - c);
	        t0 = -b-t;
			t1 = -b+t;
	    }
	    
	    void intCylinder(float3 rd, out float t0, out float t1) {
			#if LIQUID_VOLUME_NON_AABB
   			rd = mul((float3x3)_Rot, rd);
   			#endif
			float3  d = wsCameraPos - _Center;
	    	#if LIQUID_VOLUME_NON_AABB
			d = mul((float3x3)_Rot, d);
   			#endif
			float   a = dot(rd.xz, rd.xz);
			float   b = dot(rd.xz, d.xz);
			float   c = dot(d.xz,d.xz) - _Radius * _Radius;
			float   t = sqrt(b*b-a*c);
	        t0 = (-b-t)/a;
			t1 = (-b+t)/a;
			
			// cylinder cap
			float sy = _Size.y * 0.5 * _FlaskThickness.y;
			float h = abs(d.y) - sy;
			if (h>0) {
				float rdl = dot(rd.xz / rd.y, rd.xz / rd.y);
				float tc = h * sqrt (1.0 + rdl);
				t0 = max(t0, tc);
			}
			
			h = sign(rd.y) * -d.y + sy;
			if (h>0) {
				float rdl = dot(rd.xz / rd.y, rd.xz / rd.y);
				float tc = h * sqrt (1.0 + rdl);
				t1 = min(t1, tc);
			}
		}
		
		void intBox(float3 rd, out float t0, out float t1) {
	    	#if LIQUID_VOLUME_NON_AABB
			rd = mul((float3x3)_Rot, rd);
			#endif
			float3 ro = wsCameraPos - _Center;
	    	#if LIQUID_VOLUME_NON_AABB
			ro = mul((float3x3)_Rot, ro);
			#endif

		    float3 invR   = 1.0 / rd;
		    float3 boxmin = - _Radius;
		    float3 boxmax = + _Radius;
    		float3 tbot   = invR * (boxmin - ro);
    		float3 ttop   = invR * (boxmax - ro);
			float3 tmin   = min (ttop, tbot);
			float3 tmax   = max (ttop, tbot);
			float2 tt0    = max (tmin.xx, tmin.yz);
			t0  = max(tt0.x, tt0.y);
			tt0 = min (tmax.xx, tmax.yz);
			t1  = min(tt0.x, tt0.y);	
		}
  	    
  	    
		void vert(inout appdata_base v, out Input o) {
		 	UNITY_INITIALIZE_OUTPUT(Input,o);
			o.vertex = v.vertex;
			o.vertex.w = ( dot(o.vertex.xz, _Turbulence.zw) + _Time.w ) * 4.0;
			v.vertex.xyz *= _FlaskThickness;
			o.realPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	        #if LIQUID_VOLUME_IGNORE_GRAVITY
   			o.realPos = mul((float3x3)_Rot, o.realPos - _Center) + _Center;
   			#endif
		}
		
		half4 raymarch(float4 vertex, float3 rd, float t0, float t1); // forward declaration

		
		void surf (Input IN, inout SurfaceOutput o) {

	        if (IN.vertex.y > _UpperLimit) return;

	        #if LIQUID_VOLUME_IGNORE_GRAVITY
   			wsCameraPos = mul((float3x3)_Rot, _WorldSpaceCameraPos - _Center) + _Center;
			#else
			wsCameraPos = _WorldSpaceCameraPos;
   			#endif

			float t0, t1;
			float3 rd = IN.realPos - wsCameraPos;
			float dist = length(rd);
			rd /= dist;
			#if LIQUID_VOLUME_SPHERE
			intSphere(rd, t0, t1);
			#elif LIQUID_VOLUME_CUBE
			intBox(rd, t0, t1);			
			#elif LIQUID_VOLUME_CYLINDER
			intCylinder(rd, t0, t1);
			#else
			t0 = dist;
			t1 = 1e5;
			#endif
			
			t0 = max(0,t0);	// needed if camera is inside container

			#if LIQUID_VOLUME_DEPTH_AWARE || LIQUID_VOLUME_IRREGULAR
			float2 uv = IN.screenPos.xy / IN.screenPos.w;
			#endif

			#if LIQUID_VOLUME_DEPTH_AWARE
			float  vz = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
			float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
        	float3 vpos = float3((uv * 2 - 1) / p11_22, -1) * vz;
        	float4 wpos = mul(_Cam2World, float4(vpos, 1));
			float z = distance(_WorldSpaceCameraPos, wpos.xyz);
			t1 = min(t1, z);
			#endif
			
			#if LIQUID_VOLUME_IRREGULAR
			float4 backDistRGBA = tex2D(_VLBackBufferTexture, uv);
			float backDist = (1.0 / DecodeFloatRGBA(backDistRGBA)) - 1.0;
			t1 = min(t1, backDist);
			#endif

			half4 co = raymarch(IN.vertex,rd,t0,t1);

			o.Albedo = co.rgb;
			o.Alpha = co.a;
		}
