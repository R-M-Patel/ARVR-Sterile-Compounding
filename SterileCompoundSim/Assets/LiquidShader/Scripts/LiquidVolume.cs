using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace LiquidVolumeFX
{
	public enum TOPOLOGY
	{
		Sphere = 0,
		Cylinder = 1,
		Cube = 2,
		Irregular = 10
	}

	public enum DETAIL {
		Simple = 0,
		Default = 10,
		DefaultNoFlask = 11,
		BumpTexture = 20,
		Reflections = 30
	}


	[ExecuteInEditMode]
	[HelpURL("http://kronnect.com/taptapgo")]
	[AddComponentMenu("Effects/Liquid Volume")]
	[RequireComponent(typeof(MeshFilter))]
	[DisallowMultipleComponent]
	public class LiquidVolume : MonoBehaviour
	{

		[SerializeField]
		TOPOLOGY _topology = TOPOLOGY.Sphere;
		public TOPOLOGY topology { get { return _topology; } set { if (_topology != value) { _topology = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		DETAIL _detail = DETAIL.Default;
		public DETAIL detail { get { return _detail; } set { if (_detail != value) { _detail = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _level = 0.5f;
		public float level { get { return _level; } set { if (_level != Mathf.Clamp01 (value) ) { _level = Mathf.Clamp01 (value); UpdateMaterialProperties(); } } }

		[SerializeField]
		[ColorUsage(true,true,0,8,0.125f,3f)]
		Color _liquidColor1 = new Color (0, 1, 0, 0.1f);
		public Color liquidColor1 { get { return _liquidColor1; } set { if (_liquidColor1 != value) { _liquidColor1 = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0.1f,4.85f)]
		float _liquidScale1 = 1f;
		public float liquidScale1 { get { return _liquidScale1; } set { if (_liquidScale1 != value) { _liquidScale1 = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[ColorUsage(true,true,0,8,0.125f,3f)]
		Color _liquidColor2 = new Color (1, 0, 0, 0.3f);
		public Color liquidColor2 { get { return _liquidColor2; } set { if (_liquidColor2 != value) { _liquidColor2 = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(2f,4.85f)]
		float _liquidScale2 = 5f;
		public float liquidScale2 { get { return _liquidScale2; } set { if (_liquidScale2 != value) { _liquidScale2 = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _alpha = 1f;
		public float alpha { get { return _alpha; } set { if (_alpha != Mathf.Clamp01(value)) { _alpha = Mathf.Clamp01 (value); UpdateMaterialProperties(); } } }

		[SerializeField]
		bool _ditherShadows = true;
		public bool ditherShadows { get { return _ditherShadows; } set { if (_ditherShadows != value) { _ditherShadows = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _murkiness = 1.0f;
		public float murkiness { get { return _murkiness; } set { if (_murkiness != value) { _murkiness = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1f)]
		float _turbulence1 = 0.5f;
		public float turbulence1 { get { return _turbulence1; } set { if (_turbulence1 != value) { _turbulence1 = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1f)]
		float _turbulence2 = 0.2f;
		public float turbulence2 { get { return _turbulence2; } set { if (_turbulence2 != value) { _turbulence2 = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		float _frecuency = 1f;
		public float frecuency { get { return _frecuency; } set { if (_frecuency != value) { _frecuency = value; UpdateMaterialProperties(); } } }
		
		[SerializeField]
		[Range(0,5f)]
		float _sparklingIntensity = 0.1f;
		public float sparklingIntensity { get { return _sparklingIntensity; } set { if (_sparklingIntensity != value) { _sparklingIntensity = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _sparklingAmount = 0.2f;
		public float sparklingAmount { get { return _sparklingAmount; } set { if (_sparklingAmount != value) { _sparklingAmount = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,10)]
		float _deepObscurance = 2.0f;
		public float deepObscurance { get { return _deepObscurance; } set { if (_deepObscurance != value) { _deepObscurance = value; UpdateMaterialProperties(); } } }
		
		[SerializeField]
		[ColorUsage(true,true,0,8,0.125f,3f)]
		Color _foamColor = new Color (1, 1, 1, 0.65f);
		public Color foamColor { get { return _foamColor; } set { if (_foamColor != value) { _foamColor = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0.01f,1f)]
		float _foamScale = 0.2f;
		public float foamScale { get { return _foamScale; } set { if (_foamScale != value) { _foamScale = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,0.1f)]
		float _foamThickness = 0.04f;
		public float foamThickness { get { return _foamThickness; } set { if (_foamThickness != value) { _foamThickness = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _foamDensity = 0.5f;
		public float foamDensity { get { return _foamDensity; } set { if (_foamDensity != value) { _foamDensity = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(8,100)]
		float _foamWeight = 10f;
		public float foamWeight { get { return _foamWeight; } set { if (_foamWeight != value) { _foamWeight = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _foamTurbulence = 1f;
		public float foamTurbulence { get { return _foamTurbulence; } set { if (_foamTurbulence != value) { _foamTurbulence = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		bool _foamVisibleFromBottom = true;
		public bool foamVisibleFromBottom { get { return _foamVisibleFromBottom; } set { if (_foamVisibleFromBottom != value) { _foamVisibleFromBottom = value; UpdateMaterialProperties(); } } }

		[ColorUsage(true,true,0,8,0.125f,3f)]
		[SerializeField]
		Color _smokeColor = new Color (0.7f, 0.7f, 0.7f, 0.25f);
		public Color smokeColor { get { return _smokeColor; } set { if (_smokeColor != value) { _smokeColor = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0.01f,1f)]
		float _smokeScale = 0.25f;
		public float smokeScale { get { return _smokeScale; } set { if (_smokeScale != value) { _smokeScale = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,10f)]
		float _smokeBaseObscurance = 2.0f;
		public float smokeBaseObscurance { get { return _smokeBaseObscurance; } set { if (_smokeBaseObscurance != value) { _smokeBaseObscurance = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,20f)]
		float _smokeSpeed = 5.0f;
		public float smokeSpeed { get { return _smokeSpeed; } set { if (_smokeSpeed != value) { _smokeSpeed = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _upperLimit = 1f;
		public float upperLimit { get { return _upperLimit; } set { if (_upperLimit != value) { _upperLimit = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[ColorUsage(true,true,0,8,0.125f,3f)]
		Color _flaskTint = new Color (0, 0, 0, 1);
		public Color flaskTint { get { return _flaskTint; } set { if (_flaskTint != value) { _flaskTint = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _flaskThickness = 0.03f;
		public float flaskThickness { get { return _flaskThickness; } set { if (_flaskThickness != value) { _flaskThickness = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _flaskGlossinessExternal = 0.767f;
		public float flaskGlossinessExternal { get { return _flaskGlossinessExternal; } set { if (_flaskGlossinessExternal != value) { _flaskGlossinessExternal = value; UpdateMaterialProperties(); } } }
		
		[SerializeField]
		[Range(0,1)]
		float _flaskGlossinessInternal = 0.5f;
		public float flaskGlossinessInternal { get { return _flaskGlossinessInternal; } set { if (_flaskGlossinessInternal != value) { _flaskGlossinessInternal = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		bool _refractionBlur = true;
		public bool refractionBlur { get { return _refractionBlur; } set { if (_refractionBlur != value) { _refractionBlur = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,1)]
		float _blurIntensity = 0.75f;
		public float blurIntensity { get { return _blurIntensity; } set { if (_blurIntensity != Mathf.Clamp01(value)) { _blurIntensity = Mathf.Clamp01 (value); UpdateMaterialProperties(); } } }

		[SerializeField]
		int _liquidRaySteps = 10;
		public int liquidRaySteps { get { return _liquidRaySteps; } set { if (_liquidRaySteps != value) { _liquidRaySteps = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		int _foamRaySteps = 7;
		public int foamRaySteps { get { return _foamRaySteps; } set { if (_foamRaySteps != value) { _foamRaySteps = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		int _smokeRaySteps = 5;
		public int smokeRaySteps { get { return _smokeRaySteps; } set { if (_smokeRaySteps != value) { _smokeRaySteps = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		Texture2D _bumpMap;
		public Texture2D bumpMap { get { return _bumpMap; } set { if (_bumpMap != value) { _bumpMap = value; UpdateMaterialProperties(); } } }
		
		[SerializeField]
		Texture2D _distortionMap;
		public Texture2D distortionMap { get { return _distortionMap; } set { if (_distortionMap != value) { _distortionMap = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		Texture2D _texture;
		public Texture2D texture { get { return _texture; } set { if (_texture != value) { _texture = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0,10f)]
		float _distortionAmount = 1f;
		public float distortionAmount { get { return _distortionAmount; } set { if (_distortionAmount != value) { _distortionAmount = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		bool _depthAware = false;
		public bool depthAware { get { return _depthAware; } set { if (_depthAware != value) { _depthAware = value; UpdateMaterialProperties(); } } }

	
								[SerializeField]
								bool _ignoreGravity = false;

								public bool ignoreGravity {
												get { return _ignoreGravity; }
												set {
																if (_ignoreGravity != value) {
																				_ignoreGravity = value;
																				UpdateMaterialProperties ();
																}
												}
								}

		
		[SerializeField]
		bool _reactToForces = false;
		
		public bool reactToForces {
			get { return _reactToForces; }
			set {
				if (_reactToForces != value) {
					_reactToForces = value;
					UpdateMaterialProperties ();
				}
			}
		}


								[SerializeField]
								[Range(0,1f)]
		float _textureAlpha = 1f;
		public float textureAlpha { get { return _textureAlpha; } set { if (_textureAlpha != value) { _textureAlpha = value; UpdateMaterialProperties(); } } }
		
		[SerializeField]
		Vector3 _extentsScale = Vector3.one;
		public Vector3 extentsScale { get { return _extentsScale; } set { if (_extentsScale != value) { _extentsScale = value; UpdateMaterialProperties(); } } }
		
		[SerializeField]
		[Range(1,3)]
		int _noiseVariation = 1;
		public int noiseVariation { get { return _noiseVariation; } set { if (_noiseVariation != value) { _noiseVariation = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		bool _allowViewFromInside = false;
		public bool allowViewFromInside { get { return _allowViewFromInside; } set { if (_allowViewFromInside != value) { _allowViewFromInside = value; lastDistanceToCam = -1; CheckInsideOut(); } } }
		
								[SerializeField]
								int _renderQueue = 3001;
								public int renderQueue { get { return _renderQueue; } set { if (_renderQueue != value) { _renderQueue = value; UpdateMaterialProperties(); } } }


								[SerializeField]
								Cubemap _reflectionTexture;

								public Cubemap reflectionTexture {
												get { return _reflectionTexture; }
												set {
																if (_reflectionTexture != value) {
																				_reflectionTexture = value;
																				UpdateMaterialProperties ();
																}
												}
								}

		[SerializeField]
		[Range(1f, 5f)]
		float _physicsMass = 1f;
		public float physicsMass { get { return _physicsMass; } set { if (_physicsMass != value) { _physicsMass = value; UpdateMaterialProperties(); } } }

		[SerializeField]
		[Range(0.0f, 0.2f)]
		float _physicsAngularDamp = 0.02f;
		public float physicsAngularDamp { get { return _physicsAngularDamp; } set { if (_physicsAngularDamp != value) { _physicsAngularDamp = value; UpdateMaterialProperties(); } } }


		// ---- INTERNAL CODE ----
		const string SHADER_KEYWORD_DEPTH_AWARE = "LIQUID_VOLUME_DEPTH_AWARE";
		const string SHADER_KEYWORD_NON_AABB = "LIQUID_VOLUME_NON_AABB";
		const string SHADER_KEYWORD_IGNORE_GRAVITY = "LIQUID_VOLUME_IGNORE_GRAVITY";
		const string SHADER_KEYWORD_SPHERE = "LIQUID_VOLUME_SPHERE";
		const string SHADER_KEYWORD_CUBE = "LIQUID_VOLUME_CUBE";
		const string SHADER_KEYWORD_CYLINDER = "LIQUID_VOLUME_CYLINDER";
		const string SHADER_KEYWORD_IRREGULAR = "LIQUID_VOLUME_IRREGULAR";

		Material liqMatSimple, liqMatDefault, liqMatDefaultNoFlask, liqMatBump, liqMatReflections, liqMatZWriteBack, liqMat;
		MeshFilter mf;
		MeshRenderer mr;
		bool wasRefractionBlur, wasBackBuffer;
		Vector3 lastPosition, lastScale;
		Quaternion lastRotation;
		List<string> shaderKeywords;
		bool camInside;
		float lastDistanceToCam;
		DETAIL currentDetail;
		Vector4 turb;

		// Physics
		Vector3 prevVelocity, prev2Velocity, inertia, lastAvgVelocity;
		float angularVelocity, angularInertia;
		float turbulenceDueForces;
		Quaternion liquidRot;

		float prevThickness;

		void OnEnable ()
		{
			if (!gameObject.activeInHierarchy) return;
			turb.z = 1f;
			turbulenceDueForces = 0f;
			liquidRot = transform.rotation;
			currentDetail = _detail;
			lastPosition = transform.position;
			lastRotation = transform.rotation;
			lastScale = transform.localScale;
			prevThickness = _flaskThickness;
			CleanupCommandBuffer ();
			UpdateMaterialProperties();
		}

		void Reset() {
			// Try to assign propert topology based on mesh
			if (mf==null) return;

			if (mf.sharedMesh.vertexCount == 24) {
				topology = TOPOLOGY.Cube;
			} else if (mf.sharedMesh.bounds.size.y > mf.sharedMesh.bounds.size.x) {
				topology = TOPOLOGY.Cylinder;
			} 
		}


		void OnDestroy ()
		{
			CleanupCommandBuffer ();
			liqMat = null;
			if (liqMatDefault != null) {
				DestroyImmediate (liqMatDefault);
				liqMatDefault = null;
			}
			if (liqMatDefaultNoFlask != null) {
				DestroyImmediate (liqMatDefaultNoFlask);
				liqMatDefaultNoFlask = null;
			}
			if (liqMatSimple != null) {
				DestroyImmediate (liqMatSimple);
				liqMatSimple = null;
			}
			if (liqMatBump != null) {
				DestroyImmediate (liqMatBump);
				liqMatBump = null;
			}
			if (liqMatReflections != null) {
				DestroyImmediate (liqMatReflections);
				liqMatReflections = null;
			}
		}

		public void OnWillRenderObject ()
		{
			var act = gameObject.activeInHierarchy && enabled;
			
			if (act && _depthAware) {
				Camera.current.depthTextureMode |= DepthTextureMode.Depth;
			}
			
			if (act && _allowViewFromInside) {
				CheckInsideOut();
			}
			
			if (!act || (!_refractionBlur && wasRefractionBlur)) {
				LiquidVolume.CleanupRefractionBuffer ();
				wasRefractionBlur = false;
			} else if (_refractionBlur) {
				LiquidVolume.SetupRefractionBuffer ();
				wasRefractionBlur = true;
			}
			
			UpdateAnimations();

			if (!act || (_topology != TOPOLOGY.Irregular && wasBackBuffer)) {
				LiquidVolume.CleanupBackFacesBuffer ();
				wasBackBuffer = false;
			} else if (_topology == TOPOLOGY.Irregular) {
				LiquidVolume.SetupBackFacesBuffer(GetComponent<Renderer>());
				wasBackBuffer = true;
			}
		}

		void UpdateAnimations ()
		{
			// Check proper scale
			switch(topology) {
			case TOPOLOGY.Sphere:
				if (transform.localScale.y!=transform.localScale.x || transform.localScale.z!=transform.localScale.x) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.x, transform.localScale.x);
				break;
			case TOPOLOGY.Cylinder:
				if (transform.localScale.z!=transform.localScale.x) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.x);
				break;
			}

			
			if (liqMat!=null) {
				liqMat.SetMatrix("_Cam2World", Camera.current.cameraToWorldMatrix);

				Vector3 turbDir = Vector3.right;
				Quaternion rot = transform.rotation;
				if (_reactToForces) {
					Quaternion instantRot = transform.rotation;
					float dt = Time.smoothDeltaTime;
					if (Application.isPlaying && dt>0) {
						Vector3 instantVelocity = (transform.position - lastPosition) / dt;
						Vector3 avgVelocity = (prev2Velocity + prevVelocity + instantVelocity) / 3f;
						prev2Velocity = prevVelocity;
						prevVelocity = instantVelocity;

						Vector3 instantAccel = (avgVelocity - lastAvgVelocity);
						lastAvgVelocity = avgVelocity;
						inertia += avgVelocity;

						float accelMag = instantAccel.magnitude;
						float force = Mathf.Max (accelMag / _physicsMass - _physicsAngularDamp, 0f);
						angularInertia += force;
						angularVelocity += angularInertia;
						if (angularVelocity > 0) {
							angularInertia -= Mathf.Abs (angularVelocity) * _physicsMass / 100f;
						} else if (angularVelocity<0) {
							angularInertia += Mathf.Abs (angularVelocity) * _physicsMass / 100f;
						}
						float damp = 1f - _physicsAngularDamp;
						angularInertia *= damp;
						inertia *= damp;

						float mag = Mathf.Clamp(angularVelocity, -90f, 90f);
						turbDir = inertia.normalized;
						Vector3 axis = Vector3.Cross( turbDir, Vector3.down );
						instantRot = Quaternion.AngleAxis(mag, axis);

						float cinematic = Mathf.Abs (angularInertia) + Mathf.Abs (angularVelocity);
						turbulenceDueForces = Mathf.Min (0.5f / _physicsMass, turbulenceDueForces + cinematic / 1000f);
						turbulenceDueForces *= damp;
					} else {
						turbulenceDueForces = 0;
					}

					if (_topology == TOPOLOGY.Sphere) {
						liquidRot = Quaternion.Lerp(liquidRot, instantRot, 0.1f) ;
						rot = liquidRot;
					}
				}
				Matrix4x4 m = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
				liqMat.SetMatrix("_Rot", m.inverse);
				if (_topology != TOPOLOGY.Sphere) {
					float tx = turbDir.x;
					turbDir.x += (turbDir.z - turbDir.x) * 0.25f;
					turbDir.z += (tx - turbDir.z) * 0.25f;
				}
				turb.z = turbDir.x;
				turb.w = turbDir.z;
			}

			if (_reactToForces || transform.position != lastPosition || transform.localScale != lastScale || transform.rotation != lastRotation) {
				UpdateLevels();
			}

		}

		public void UpdateMaterialProperties() {
			if (!gameObject.activeInHierarchy) return;

			switch(_detail) {
			case DETAIL.Simple:
				if (liqMatSimple == null) {
					liqMatSimple = Instantiate (Resources.Load<Material> ("Materials/LiquidVolumeSimple")) as Material;
					liqMatSimple.hideFlags = HideFlags.DontSave;
				}
				liqMat = liqMatSimple;
				break;
			case DETAIL.DefaultNoFlask:
				if (liqMatDefaultNoFlask == null) {
					liqMatDefaultNoFlask = Instantiate (Resources.Load<Material> ("Materials/LiquidVolumeDefaultNoFlask")) as Material;
					liqMatDefaultNoFlask.hideFlags = HideFlags.DontSave;
				}
				liqMat = liqMatDefaultNoFlask;
				break;
			case DETAIL.BumpTexture:
				if (liqMatBump == null) {
					liqMatBump = Instantiate (Resources.Load<Material> ("Materials/LiquidVolumeBump")) as Material;
					liqMatBump.hideFlags = HideFlags.DontSave;
				}
				liqMat = liqMatBump;
				break;
			case DETAIL.Reflections:
				if (liqMatReflections == null) {
					liqMatReflections = Instantiate (Resources.Load<Material> ("Materials/LiquidVolumeReflections")) as Material;
					liqMatReflections.hideFlags = HideFlags.DontSave;
				}
				liqMat = liqMatReflections;
				break;
			default:
				if (liqMatDefault == null) {
					liqMatDefault = Instantiate (Resources.Load<Material> ("Materials/LiquidVolumeDefault")) as Material;
					liqMatDefault.hideFlags = HideFlags.DontSave;
				}
				liqMat = liqMatDefault;
				break;
			}

			if (liqMat == null || Camera.main == null)
				return;

			UpdateLevels();
			if (mr==null) return;
			mr.sharedMaterial = liqMat;

												if (currentDetail != _detail) {
																currentDetail = _detail;
																if (_detail == DETAIL.Reflections) {
																				_flaskTint = Color.white;
																				_flaskGlossinessExternal = 0.1f;
																} else {
																				_flaskTint = new Color (0, 0, 0, 1f);
																}
												}
			liqMat.SetColor ("_Color1", _liquidColor1);
			liqMat.SetColor ("_Color2", _liquidColor2);
			liqMat.SetFloat ("_Alpha", _alpha);

			liqMat.SetFloat ("_Glossiness", _flaskGlossinessExternal);
			liqMat.SetFloat ("_GlossinessInt", _flaskGlossinessInternal * 96f + 1f);
			liqMat.SetFloat ("_Topology", (int)_topology);

			liqMat.SetFloat ("_Muddy", _murkiness);
			liqMat.SetFloat ("_Alpha", _alpha);

			float alphaCombined = _alpha * Mathf.Clamp01 ((_liquidColor1.a + _liquidColor2.a) * 4f);
			if (_ditherShadows) {
				liqMat.SetFloat ("_AlphaCombined", alphaCombined);
			} else {
				liqMat.SetFloat ("_AlphaCombined", alphaCombined > 0 ? 1000f: 0f);
			}

			liqMat.SetFloat ("_SparklingIntensity", _sparklingIntensity * 250.0f);
			liqMat.SetFloat ("_SparklingThreshold", 1.0f - _sparklingAmount);
			liqMat.SetFloat ("_DeepAtten", _deepObscurance);
			liqMat.SetColor ("_SmokeColor", _smokeColor);
			liqMat.SetFloat ("_SmokeAtten", _smokeBaseObscurance);
			liqMat.SetFloat ("_SmokeSpeed", _smokeSpeed);
			liqMat.SetFloat ("_LiquidRaySteps", _liquidRaySteps);
			liqMat.SetFloat ("_SmokeRaySteps", _smokeRaySteps);
			liqMat.SetFloat ("_FlaskBlurIntensity", _blurIntensity * (_refractionBlur ? 1f : 0f));
			liqMat.SetColor ("_FlaskTint", _flaskTint * _flaskTint.a);

			liqMat.SetColor ("_FoamColor", _foamColor);
			liqMat.SetFloat ("_FoamRaySteps", _foamRaySteps);
			liqMat.SetFloat ("_FoamDensity", _foamDensity);
			liqMat.SetFloat ("_FoamWeight", _foamWeight);
			liqMat.SetFloat ("_FoamBottom", _foamVisibleFromBottom ? 1f: 0f);
			liqMat.SetFloat ("_FoamTurbulence", _foamTurbulence);

												liqMat.SetFloat ("_FlaskTexAlpha", _textureAlpha);

												if (_detail == DETAIL.BumpTexture) {
																liqMat.SetTexture ("_BumpMap", _bumpMap);
																liqMat.SetTexture ("_DispMap", _distortionMap);
																liqMat.SetFloat ("_DispAmount", _distortionAmount);
																liqMat.SetTexture ("_FlaskTex", _texture);
												}

												if (_detail == DETAIL.Reflections) {
																if (_reflectionTexture == null) {
																				_reflectionTexture = Resources.Load<Cubemap> ("Textures/Reflections");
																}
																liqMat.SetTexture ("_RefractTex", _reflectionTexture);
												}

			Texture3D tex3d = Resources.Load<Texture3D>("Textures/Noise3D" + _noiseVariation.ToString());
			if (tex3d!=null) {
				liqMat.SetTexture("_NoiseTex", tex3d);
			}

												liqMat.renderQueue = _renderQueue;
			UpdateInsideOut();

			if (_topology== TOPOLOGY.Irregular && prevThickness!=_flaskThickness) {
				prevThickness = _flaskThickness;
				CleanupBackFacesBuffer(); 
			}


		}

		void UpdateLevels() {
			if (liqMat == null) return;

			if (mf==null) {
				mf = GetComponent<MeshFilter>();
				if (mf==null || mf.sharedMesh==null) return;
			}
			if (mr==null) {
				mr = GetComponent<MeshRenderer>();
				if (mr==null) return;
			}

			Vector3 size = new Vector3(mf.sharedMesh.bounds.size.x * transform.lossyScale.x, mf.sharedMesh.bounds.size.y * transform.lossyScale.y, mf.sharedMesh.bounds.size.z * transform.lossyScale.z);
			size.x *= _extentsScale.x;
			size.y *= _extentsScale.y;
			size.z *= _extentsScale.z;
			liqMat.SetVector ("_Size", size);
			float maxWidth = Mathf.Max (size.x, size.z);
			float radius = size.y * 0.5f * (1.0f - _flaskThickness * maxWidth / size.y);

			Vector3 extents;
//			if (_ignoreGravity) {
//				extents = size * 0.5f;
//			} else {
				extents = mr.bounds.extents;
//			}			
			extents.x *= _extentsScale.x;
			extents.y *= _extentsScale.y;
			extents.z *= _extentsScale.z;

			// Compensate levelpos with upperlimit
			float rotationAdjustment;
			if (_upperLimit<1f && !_ignoreGravity) {
				float y1 = transform.TransformPoint(Vector3.up * extents.y).y;
				float y0 = transform.TransformPoint(Vector3.up * (extents.y * _upperLimit)).y;
				rotationAdjustment = Mathf.Max(y0 - y1, 0);
			} else {
				rotationAdjustment = 0;
			}

			float levelPos = transform.position.y - extents.y + extents.y * 2f * _level + rotationAdjustment;
			liqMat.SetFloat ("_LevelPos", levelPos);
			float upperLimit = mf.sharedMesh.bounds.size.y * 0.5f * _extentsScale.y * _upperLimit;
			liqMat.SetFloat ("_UpperLimit", upperLimit);
			float visibleLevel = (_level<=0 || _level>=1f) ? 0f: 1f;
			UpdateTurbulence();
			float foamPos = transform.position.y - extents.y + (rotationAdjustment + extents.y * 2.0f * (_level + _foamThickness)) * visibleLevel;
			liqMat.SetFloat ("_FoamMaxPos", foamPos);
			Vector3 thickness = new Vector3(1.0f - _flaskThickness, (1.0f - _flaskThickness * maxWidth / size.y), (1.0f - _flaskThickness * maxWidth / size.z));
			liqMat.SetVector ("_FlaskThickness", thickness);
			liqMat.SetFloat ("_Radius", size.x * 0.5f * thickness.x);
			liqMat.SetVector ("_Scale", new Vector4 (_smokeScale / radius, _foamScale / radius, _liquidScale1 / radius, _liquidScale2 / radius));
			liqMat.SetVector ("_Center", transform.position);

			if (shaderKeywords==null) {
				shaderKeywords = new List<string>();
			} else {
				shaderKeywords.Clear();
			}

			if (_depthAware) shaderKeywords.Add (SHADER_KEYWORD_DEPTH_AWARE);

			if (_reactToForces && _topology == TOPOLOGY.Sphere) {
				shaderKeywords.Add (SHADER_KEYWORD_IGNORE_GRAVITY);
			} else if (_ignoreGravity) {
				shaderKeywords.Add (SHADER_KEYWORD_IGNORE_GRAVITY);
			} else if (transform.rotation.eulerAngles != Vector3.zero) {
				shaderKeywords.Add (SHADER_KEYWORD_NON_AABB);
			}
			switch(_topology) {
			case TOPOLOGY.Sphere: shaderKeywords.Add (SHADER_KEYWORD_SPHERE); break;
			case TOPOLOGY.Cube: shaderKeywords.Add (SHADER_KEYWORD_CUBE); break;
			case TOPOLOGY.Cylinder: shaderKeywords.Add (SHADER_KEYWORD_CYLINDER); break;
			default: shaderKeywords.Add (SHADER_KEYWORD_IRREGULAR); break;
			}

			liqMat.shaderKeywords = shaderKeywords.ToArray();

			lastPosition = transform.position;
			lastScale = transform.localScale;
			lastRotation = transform.rotation;
		}


		void UpdateTurbulence() {
			if (liqMat==null) return;
			float visibleLevel = 1f; // (_level<=0 || _level>=1f) ? 0.1f: 1f;	// commented out to allow animation even level is 0 or full
			float isInsideContainer = (camInside && _allowViewFromInside) ? 0f: 1f;
			turb.x = _turbulence1 * visibleLevel * isInsideContainer;
			turb.y = Mathf.Max (_turbulence2, turbulenceDueForces) * visibleLevel * isInsideContainer;
			Vector4 shaderTurb = turb;
			shaderTurb.z *= 3.1415927f * _frecuency;
			shaderTurb.w *= 3.1415927f * _frecuency;
			liqMat.SetVector ("_Turbulence", shaderTurb);
		}

		void CheckInsideOut() {
			if (Camera.current==null || mr==null) {
				if (!_allowViewFromInside) UpdateInsideOut();
				return;
			}

			Vector3 currentCamPos = Camera.current.transform.position;
			float currentDistanceToCam = (currentCamPos - transform.position).sqrMagnitude;
			if (currentDistanceToCam==lastDistanceToCam) return;
			lastDistanceToCam = currentDistanceToCam;

			// Check if position is inside container
			bool nowInside = false;
			switch(_topology) {
			case TOPOLOGY.Cube:
				nowInside = PointInAABB(currentCamPos);
				break;
			case TOPOLOGY.Cylinder:
				nowInside = PointInCylinder(currentCamPos);
				break;
			default:
				nowInside = (currentCamPos - transform.position).sqrMagnitude < mr.bounds.size.x * mr.bounds.size.x;
				break;
			}

			if (nowInside!=camInside) {
				camInside = nowInside;
				UpdateInsideOut();
			}
		}


		bool PointInAABB(Vector3 point) {
			point = transform.InverseTransformPoint( point );
			Vector3 ext = mf.sharedMesh.bounds.extents;
			if( point.x < ext.x && point.x > -ext.x && 
			   point.y < ext.y && point.y > -ext.y && 
			   point.z < ext.z && point.z > -ext.z ) {
				return true;
			}
			else {
				return false;
			}
		}

		bool PointInCylinder(Vector3 point) {
			point = transform.InverseTransformPoint( point );
			Vector3 ext = mf.sharedMesh.bounds.extents;
			if( point.x < ext.x && point.x > -ext.x && 
			   point.y < ext.y && point.y > -ext.y && 
			   point.z < ext.z && point.z > -ext.z ) {

				point.y = 0;
				Vector3 currentPos = transform.position;
				currentPos.y = 0;
				return (point - currentPos).sqrMagnitude < ext.x * ext.x;
			}
			return false;
		}


		void UpdateInsideOut() {
			if (liqMat==null) return;
			if (_allowViewFromInside && camInside) {
				liqMat.SetInt("_CullMode", (int)UnityEngine.Rendering.CullMode.Front);
				liqMat.SetInt("_ZTestMode", (int)UnityEngine.Rendering.CompareFunction.Always);
			} else {
				liqMat.SetInt("_CullMode", (int)UnityEngine.Rendering.CullMode.Back);
				liqMat.SetInt("_ZTestMode", (int)UnityEngine.Rendering.CompareFunction.LessEqual);
			}
			UpdateTurbulence();
		}
	
		#region CommandBuffer setup

		private void CleanupCommandBuffer ()
		{
			LiquidVolume.CleanupRefractionBuffer ();
			LiquidVolume.CleanupBackFacesBuffer ();
		}

		static Dictionary<Camera,CommandBuffer> m_CamerasBlur = new Dictionary<Camera,CommandBuffer> ();
		static Material blurMat;

		internal static void CleanupRefractionBuffer ()
		{
			foreach (var cam in m_CamerasBlur) {
				if (cam.Key) {
					cam.Key.RemoveCommandBuffer (CameraEvent.AfterImageEffectsOpaque, cam.Value);
				}
			}
			m_CamerasBlur.Clear ();
			if (blurMat != null) {
				DestroyImmediate (blurMat);
				blurMat = null;
			}
		}
		
		internal static void SetupRefractionBuffer ()
		{
			var cam = Camera.current;
			if (!cam)
				return;
			
			CommandBuffer buf = null;
			if (m_CamerasBlur.ContainsKey (cam))
				return;
			
			if (blurMat == null) {
				blurMat = Instantiate (Resources.Load<Material> ("Materials/LiquidVolumeBlur")) as Material;
				blurMat.hideFlags = HideFlags.DontSave;
			}
			
			buf = new CommandBuffer ();
			buf.name = "Volumetric Liquid Background Blur";
			m_CamerasBlur [cam] = buf;
			
			int screenCopyID = Shader.PropertyToID ("_VLScreenCopyTexture");
			buf.GetTemporaryRT (screenCopyID, -1, -1, 0, FilterMode.Bilinear);
			buf.Blit (BuiltinRenderTextureType.CurrentActive, screenCopyID);
			
			int blurredID = Shader.PropertyToID ("_VLTemp1");
			int blurredID2 = Shader.PropertyToID ("_VLTemp2");
			buf.GetTemporaryRT (blurredID, -2, -2, 0, FilterMode.Bilinear);
			buf.GetTemporaryRT (blurredID2, -2, -2, 0, FilterMode.Bilinear);
			
			buf.Blit (screenCopyID, blurredID2, blurMat, 0);
			buf.ReleaseTemporaryRT (screenCopyID); 
			
			buf.Blit (blurredID2, blurredID, blurMat, 1);
			buf.ReleaseTemporaryRT (blurredID2); 
			
			buf.SetGlobalTexture ("_VLGrabBlurTexture", blurredID);
			
			cam.AddCommandBuffer (CameraEvent.AfterImageEffectsOpaque, buf);

		}

		#endregion

		#region Backbuffer

		static Dictionary<Camera,CommandBuffer> m_CamerasBackBuffer = new Dictionary<Camera,CommandBuffer> ();
		static Material backBufferMat;
		static List<Renderer>renderers = new List<Renderer>();

		internal static void SetupBackFacesBuffer(Renderer renderer) {
			var cam = Camera.current;
			if (!cam)
				return;

			if (!renderers.Contains(renderer)) CleanupBackFacesCameras();

			CommandBuffer buf = null;
			if (m_CamerasBackBuffer.ContainsKey (cam)) {
				return;
			}
			
			if (backBufferMat == null) {
				backBufferMat = new Material(Shader.Find ("LiquidVolume/ZWriteBack")) as Material;
				backBufferMat.hideFlags = HideFlags.DontSave;
			}

			renderers.Add (renderer);
			buf = new CommandBuffer ();
			buf.name = "Volumetric Liquid BackBuffer";
			m_CamerasBackBuffer [cam] = buf;
			
			int backBufferID = Shader.PropertyToID ("_VLBackBufferTexture");
			buf.GetTemporaryRT (backBufferID, -1, -1, 24, FilterMode.Point, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
			buf.SetRenderTarget(backBufferID);
			buf.ClearRenderTarget(true, true, new Color (0.9882353f, 0.4470558f, 0.75f, 0f), 0f);
			renderers.ForEach((Renderer obj) => {
				backBufferMat.SetFloat ("_FlaskThickness", 1.0f - obj.GetComponent<LiquidVolume>().flaskThickness);
				buf.DrawRenderer(obj, backBufferMat);
			});
			cam.AddCommandBuffer (CameraEvent.AfterImageEffectsOpaque, buf);
		}

		internal static void CleanupBackFacesBuffer ()
		{
			CleanupBackFacesCameras();
			if (backBufferMat != null) {
				DestroyImmediate (backBufferMat);
				backBufferMat = null;
			}
			renderers.Clear();
		}

		
		internal static void CleanupBackFacesCameras ()
		{
			foreach (var cam in m_CamerasBackBuffer) {
				if (cam.Key) {
					cam.Key.RemoveCommandBuffer (CameraEvent.AfterImageEffectsOpaque, cam.Value);
				}
			}
			m_CamerasBackBuffer.Clear ();
		}

		#endregion
					
	}
}
