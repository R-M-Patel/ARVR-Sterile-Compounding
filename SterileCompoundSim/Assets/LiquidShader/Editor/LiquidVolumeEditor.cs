using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

namespace LiquidVolumeFX {
				[CustomEditor (typeof(LiquidVolume)), CanEditMultipleObjects]
				public class LiquidVolumeEditor : Editor {

		static GUIStyle titleLabelStyle, sectionHeaderStyle;
		static Color titleColor;
		static bool[] expandSection = new bool[6];
		const string SECTION_PREFS = "LiquidVolumeExpandSection";
		static string[] sectionNames = new string[] {
			"Liquid Settings",
			"Foam Settings",
			"Smoke Settings",
			"Flask Settings",
			"Physics",
			"Advanced"
		};
		const int LIQUID_SETTINGS = 0;
		const int FOAM_SETTINGS = 1;
		const int SMOKE_SETTINGS = 2;
		const int FLASK_SETTINGS = 3;
		const int PHYSICS_SETTINGS = 4;
		const int ADVANCED_SETTINGS = 5;
		SerializedProperty topology, detail, depthAware, ignoreGravity, reactToForces;
		SerializedProperty level, liquidColor1, liquidColor2, liquidScale1, liquidScale2, alpha, ditherShadows, murkiness, turbulence1, turbulence2, frecuency;
		SerializedProperty sparklingIntensity, sparklingAmount, deepObscurance;
		SerializedProperty foamColor, foamScale, foamThickness, foamDensity, foamWeight, foamVisibleFromBottom, foamTurbulence;
		SerializedProperty smokeColor, smokeScale, smokeBaseObscurance, smokeSpeed;
		SerializedProperty flaskTint, flaskThickness, flaskGlossinessExternal, flaskGlossinessInternal, refractionBlur, blurIntensity;
		SerializedProperty liquidRaySteps, foamRaySteps, smokeRaySteps, extentsScale, upperLimit, noiseVariation, allowViewFromInside;
		SerializedProperty bumpMap, distortionMap, texture, textureAlpha, distortionAmount, renderQueue;
		SerializedProperty reflectionTexture;
		SerializedProperty physicsMass, physicsAngularDamp;
		MeshRenderer mr;

		void OnEnable ()
		{
			titleColor = EditorGUIUtility.isProSkin ? new Color (0.52f, 0.66f, 0.9f) : new Color (0.12f, 0.16f, 0.4f);
			for (int k=0; k<expandSection.Length; k++) {
				expandSection [k] = EditorPrefs.GetBool (SECTION_PREFS + k, false);
			}
			topology = serializedObject.FindProperty ("_topology");
			detail = serializedObject.FindProperty ("_detail");
			depthAware = serializedObject.FindProperty ("_depthAware");

			level = serializedObject.FindProperty ("_level");
			liquidColor1 = serializedObject.FindProperty ("_liquidColor1");
			liquidColor2 = serializedObject.FindProperty ("_liquidColor2");
			liquidScale1 = serializedObject.FindProperty ("_liquidScale1");
			liquidScale2 = serializedObject.FindProperty ("_liquidScale2");
			alpha = serializedObject.FindProperty ("_alpha");
			ditherShadows = serializedObject.FindProperty ("_ditherShadows");
			murkiness = serializedObject.FindProperty ("_murkiness");
			turbulence1 = serializedObject.FindProperty ("_turbulence1");
			turbulence2 = serializedObject.FindProperty ("_turbulence2");
			frecuency = serializedObject.FindProperty ("_frecuency");
			sparklingIntensity = serializedObject.FindProperty ("_sparklingIntensity");
			sparklingAmount = serializedObject.FindProperty ("_sparklingAmount");
			deepObscurance = serializedObject.FindProperty ("_deepObscurance");

												foamColor = serializedObject.FindProperty ("_foamColor");
												foamScale = serializedObject.FindProperty ("_foamScale");
												foamThickness = serializedObject.FindProperty ("_foamThickness");
												foamDensity = serializedObject.FindProperty ("_foamDensity");
												foamWeight = serializedObject.FindProperty ("_foamWeight");
												foamTurbulence = serializedObject.FindProperty ("_foamTurbulence");
												foamVisibleFromBottom = serializedObject.FindProperty ("_foamVisibleFromBottom");

												smokeColor = serializedObject.FindProperty ("_smokeColor");
												smokeScale = serializedObject.FindProperty ("_smokeScale");
												smokeBaseObscurance = serializedObject.FindProperty ("_smokeBaseObscurance");
												smokeSpeed = serializedObject.FindProperty ("_smokeSpeed");

												flaskTint = serializedObject.FindProperty ("_flaskTint");
												flaskThickness = serializedObject.FindProperty ("_flaskThickness");
												flaskGlossinessExternal = serializedObject.FindProperty ("_flaskGlossinessExternal");
												flaskGlossinessInternal = serializedObject.FindProperty ("_flaskGlossinessInternal");
												refractionBlur = serializedObject.FindProperty ("_refractionBlur");
												blurIntensity = serializedObject.FindProperty ("_blurIntensity");

												liquidRaySteps = serializedObject.FindProperty ("_liquidRaySteps");
												foamRaySteps = serializedObject.FindProperty ("_foamRaySteps");
												smokeRaySteps = serializedObject.FindProperty ("_smokeRaySteps");
												extentsScale = serializedObject.FindProperty ("_extentsScale");
												upperLimit = serializedObject.FindProperty ("_upperLimit");
												noiseVariation = serializedObject.FindProperty ("_noiseVariation");
												allowViewFromInside = serializedObject.FindProperty ("_allowViewFromInside");
												renderQueue = serializedObject.FindProperty ("_renderQueue");

												bumpMap = serializedObject.FindProperty ("_bumpMap");
												distortionMap = serializedObject.FindProperty ("_distortionMap");
												distortionAmount = serializedObject.FindProperty ("_distortionAmount");
												texture = serializedObject.FindProperty ("_texture");
												textureAlpha = serializedObject.FindProperty ("_textureAlpha");

												reflectionTexture = serializedObject.FindProperty ("_reflectionTexture");
												reactToForces = serializedObject.FindProperty ("_reactToForces");
												ignoreGravity = serializedObject.FindProperty ("_ignoreGravity");
												physicsMass = serializedObject.FindProperty ("_physicsMass");
												physicsAngularDamp = serializedObject.FindProperty ("_physicsAngularDamp");
								}

								void OnDestroy () {
												// Save folding sections state
												for (int k = 0; k < expandSection.Length; k++) {
																EditorPrefs.SetBool (SECTION_PREFS + k, expandSection [k]);
												}
								}

								public override void OnInspectorGUI () {
												serializedObject.UpdateIfDirtyOrScript ();

												if (sectionHeaderStyle == null) {
																sectionHeaderStyle = new GUIStyle (EditorStyles.foldout);
												}
												sectionHeaderStyle.normal.textColor = titleColor;
												sectionHeaderStyle.margin = new RectOffset (12, 0, 0, 0);
												sectionHeaderStyle.fontStyle = FontStyle.Bold;

												if (titleLabelStyle == null) {
																titleLabelStyle = new GUIStyle (EditorStyles.label);
												}
												titleLabelStyle.normal.textColor = titleColor;
												titleLabelStyle.fontStyle = FontStyle.Bold;


												EditorGUILayout.Separator ();

												EditorGUILayout.BeginHorizontal ();
												EditorGUILayout.LabelField ("General Settings", titleLabelStyle);
												if (GUILayout.Button ("Help", GUILayout.Width (40))) {
																if (!EditorUtility.DisplayDialog ("Liquid Volume", "To learn more about a property in this inspector move the mouse over the label for a quick description (tooltip).\n\nPlease check README file in the root of the asset for details and contact support.\n\nIf you like Liquid Volume, please rate it on the Asset Store. For feedback and suggestions visit our support forum on kronnect.com.", "Close", "Visit Support Forum")) {
																				Application.OpenURL ("http://kronnect.com/taptapgo");
																}
												}
												EditorGUILayout.EndHorizontal ();

												EditorGUILayout.PropertyField (topology, new GUIContent ("Topology", "Shape of the volume."));
												EditorGUILayout.PropertyField (detail, new GUIContent ("Detail", "Amount of detail of the liquid effect. The 'Simple' setting does not use 3D textures which makes it compatible with mobile."));
												EditorGUILayout.PropertyField (depthAware, new GUIContent ("Depth Aware", "Enabled z-testing inside liquid volume. Useful if volume contains other objects in addition to liquid, don't enable otherwise. 2D objects inside the liquid volume needs to use an opaque cutout shader that writes to z-buffer (Standard Shader CutOut mode is a good option)."));

												int detailed = detail.intValue;

												EditorGUILayout.Separator ();
												expandSection [LIQUID_SETTINGS] = EditorGUILayout.Foldout (expandSection [LIQUID_SETTINGS], sectionNames [LIQUID_SETTINGS], sectionHeaderStyle);

												if (expandSection [LIQUID_SETTINGS]) {
																EditorGUILayout.PropertyField (level, new GUIContent ("Level", "Fill level of the volume."));

																if (detailed >= 10) {
																				EditorGUILayout.PropertyField (liquidColor1, new GUIContent ("Color 1"));
																				EditorGUILayout.PropertyField (liquidScale1, new GUIContent ("Scale 1", "Scale applied to the 1st texture of the liquid."));
																				EditorGUILayout.PropertyField (liquidColor2, new GUIContent ("Color 2"));
																				EditorGUILayout.PropertyField (liquidScale2, new GUIContent ("Scale 2", "Scale applied to the 2nd texture of the liquid."));
																				EditorGUILayout.PropertyField (murkiness, new GUIContent ("Murkiness", "The purity of the liquid. 0 = crystal clear, 1 = full of mud/dirt."));
																}

				EditorGUILayout.PropertyField (alpha, new GUIContent ("Alpha", "Global transparency of the liquid. You can also change transparency for the two liquid colors."));
				EditorGUILayout.PropertyField (ditherShadows, new GUIContent ("Dither Shadow", "Enable to apply a dither to the liquid shadow, simulating partially transparent shadows. For best results enable soft shadows in quality settings."));
				EditorGUILayout.PropertyField (turbulence1, new GUIContent ("Turbulence 1", "Low-amplitude turbulence."));
				EditorGUILayout.PropertyField (turbulence2, new GUIContent ("Turbulence 2", "High-amplitude turbulence."));
				EditorGUILayout.PropertyField (frecuency, new GUIContent ("Frecuency", "Frecuency of the turbulence. Increase to produce shorter waves."));

																if (detailed >= 10) {
																				EditorGUILayout.PropertyField (sparklingIntensity, new GUIContent ("Sparkling Intensity", "Brightness of the sparkling / glitter particles."));
																				EditorGUILayout.PropertyField (sparklingAmount, new GUIContent ("Sparkling Amount", "Amount of sparkling / glitter particles."));
																}
			
																EditorGUILayout.PropertyField (deepObscurance, new GUIContent ("Deep Obscurance", "Makes the bottom of the liquid darker."));

																if (detailed == 0) {
																				EditorGUILayout.PropertyField (foamVisibleFromBottom, new GUIContent ("Visible From Bottom", "If foam is visible through liquid when container is viewed from bottom."));
																}

												}

												if (detailed >= 10) {
																EditorGUILayout.Separator ();
																expandSection [FOAM_SETTINGS] = EditorGUILayout.Foldout (expandSection [FOAM_SETTINGS], sectionNames [FOAM_SETTINGS], sectionHeaderStyle);
			
																if (expandSection [FOAM_SETTINGS]) {
																				EditorGUILayout.PropertyField (foamColor, new GUIContent ("Color"));
																				EditorGUILayout.PropertyField (foamScale, new GUIContent ("Scale", "Scale applied to the texture used for the foam."));
																				EditorGUILayout.PropertyField (foamThickness, new GUIContent ("Thickness"));
																				EditorGUILayout.PropertyField (foamDensity, new GUIContent ("Density"));
																				EditorGUILayout.PropertyField (foamWeight, new GUIContent ("Weight", "The greater the value the denser the foam at the bottom line with the liquid."));
																				EditorGUILayout.PropertyField (foamTurbulence, new GUIContent ("Turbulence", "Multiplier to liquid turbulence that affects foam. Set this to zero to produce a static foam."));
																				EditorGUILayout.PropertyField (foamVisibleFromBottom, new GUIContent ("Visible From Bottom", "If foam is visible through liquid when container is viewed from bottom."));
																}
												}

			
												EditorGUILayout.Separator ();
												expandSection [SMOKE_SETTINGS] = EditorGUILayout.Foldout (expandSection [SMOKE_SETTINGS], sectionNames [SMOKE_SETTINGS], sectionHeaderStyle);

												if (expandSection [SMOKE_SETTINGS]) {
																EditorGUILayout.PropertyField (smokeColor, new GUIContent ("Color"));
																if (detailed >= 10) {
																				EditorGUILayout.PropertyField (smokeScale, new GUIContent ("Scale", "Scale applied to the texture used for the smoke."));
																				EditorGUILayout.PropertyField (smokeSpeed, new GUIContent ("Speed"));
																}
																EditorGUILayout.PropertyField (smokeBaseObscurance, new GUIContent ("Base Obscurance", "Makes the smoke darker at the base."));
												}

												if (detailed != (int)DETAIL.DefaultNoFlask) {
																EditorGUILayout.Separator ();
																expandSection [FLASK_SETTINGS] = EditorGUILayout.Foldout (expandSection [FLASK_SETTINGS], sectionNames [FLASK_SETTINGS], sectionHeaderStyle);

																if (expandSection [FLASK_SETTINGS]) {
																				EditorGUILayout.PropertyField (flaskTint, new GUIContent ("Tint", "Tint color applied to the crystal."));
																				EditorGUILayout.PropertyField (flaskThickness, new GUIContent ("Thickness", "Crystal thinkness."));
																				EditorGUILayout.PropertyField (flaskGlossinessExternal, new GUIContent ("Glossiness External", "The glossiness of the external face of the crystal."));
																				if (detailed != 30) {
																								EditorGUILayout.PropertyField (flaskGlossinessInternal, new GUIContent ("Glossiness Internal", "The glossiness of the internal face of the crystal."));
																				} else {
																								EditorGUILayout.PropertyField (reflectionTexture, new GUIContent ("Reflections", "Assign a cubemap texture for the reflections effect."));
																								EditorGUILayout.PropertyField (textureAlpha, new GUIContent ("Alpha"));
																				}
																				if (detailed == 20) {
																								EditorGUILayout.PropertyField (texture, new GUIContent ("Texture", "Assign a texture for the liquid container."));
																								EditorGUILayout.PropertyField (textureAlpha, new GUIContent ("Texture Alpha"));
																								EditorGUILayout.PropertyField (bumpMap, new GUIContent ("Bump Map", "Assign a normal map for the liquid container."));
																								EditorGUILayout.PropertyField (distortionMap, new GUIContent ("Distortion Map", "Assign a displacement map in this slot for the crystal distortion."));
																								EditorGUILayout.PropertyField (distortionAmount, new GUIContent ("Distortion Amount"));
																				}
																				EditorGUILayout.PropertyField (refractionBlur, new GUIContent ("Refraction Blur", "Blurs background visible through the flask."));
																				if (refractionBlur.boolValue) {
																								EditorGUILayout.PropertyField (blurIntensity);
																				}
																}
												}

												EditorGUILayout.Separator ();
												expandSection [PHYSICS_SETTINGS] = EditorGUILayout.Foldout (expandSection [PHYSICS_SETTINGS], sectionNames [PHYSICS_SETTINGS], sectionHeaderStyle);
												if (expandSection [PHYSICS_SETTINGS]) {
																EditorGUILayout.PropertyField (reactToForces, new GUIContent ("React to Forces", "When enabled, liquid will move inside the flask trying to reflect external forces."));
																GUI.enabled = reactToForces.boolValue;
																EditorGUILayout.PropertyField (physicsMass, new GUIContent ("Mass", "A greater mass will make liquid more static."));
																EditorGUILayout.PropertyField (physicsAngularDamp, new GUIContent ("Angular Damp", "The amount of friction of the liquid with the flask which determines the speed at which the liquid returns to normal position after applying a force."));
																GUI.enabled = !reactToForces.boolValue;
																EditorGUILayout.PropertyField (ignoreGravity, new GUIContent ("Ignore Gravity", "When enabled, liquid will rotate with flask. False by default, which means liquid will stay at bottom of the flask."));
																GUI.enabled = true;
												}

												EditorGUILayout.Separator ();
												expandSection [ADVANCED_SETTINGS] = EditorGUILayout.Foldout (expandSection [ADVANCED_SETTINGS], sectionNames [ADVANCED_SETTINGS], sectionHeaderStyle);

												if (expandSection [ADVANCED_SETTINGS]) {
																EditorGUILayout.PropertyField (smokeRaySteps, new GUIContent ("Smoke Ray Steps", "Number of samples per pixel used to build the smoke color."));
																EditorGUILayout.PropertyField (liquidRaySteps, new GUIContent ("Liquid Ray Steps", "Number of samples per pixel used to build the liquid color."));
																if (detailed >= 1) {
																				EditorGUILayout.PropertyField (foamRaySteps, new GUIContent ("Foam Ray Steps", "Number of samples per pixel used to build the foam color."));
																				EditorGUILayout.PropertyField (noiseVariation, new GUIContent ("Noise Variation", "Choose between 3 different 3D textures."));
																}
																EditorGUILayout.PropertyField (upperLimit, new GUIContent ("Upper Limit", "Upper limit for liquid, foam and smoke with respect to flask size."));
																EditorGUILayout.PropertyField (extentsScale, new GUIContent ("Extents Scale", "Optional and additional multiplier applied to the current size of the mesh. Used to adjust levels on specific models that require this."));
																EditorGUILayout.BeginHorizontal ();
																if (GUILayout.Button ("Bake Current Transform")) {
																				if (EditorUtility.DisplayDialog ("Bake Current Transform", "Current transform (rotation and scale) will transferred to mesh itself. Do you want to continue?", "Ok", "Cancel")) {
																								foreach (LiquidVolume lv in targets) {
																												BakeRotation (lv);
																								}
																				}
																}
																if (GUILayout.Button ("Center Pivot")) {
																				if (EditorUtility.DisplayDialog ("Center Mesh Pivot", "Vertices will be displaced so pivot is relocated at its center. Do you want to continue?", "Ok", "Cancel")) {
																								foreach (LiquidVolume lv in targets) {
																												CenterPivot (lv);
																								}
																				}
																}
																EditorGUILayout.EndHorizontal ();
																EditorGUILayout.PropertyField (allowViewFromInside, new GUIContent ("Allow View From Inside", "Allows the liquid to be visible when camera enters the container. This is an experimental feature and some options like turbulence are not available when camera is inside the container."));
																EditorGUILayout.PropertyField (renderQueue, new GUIContent ("Render Queue", "Liquid Volume renders at Transparent+1 queue (which equals to 3001). You may change this to 3000 to render as a normal transparent object or use another value if needed."));
												}

			
												EditorGUILayout.Separator ();


												if (serializedObject.ApplyModifiedProperties ()) {
																foreach (LiquidVolume lv in targets) {
																				lv.UpdateMaterialProperties ();
																}
												}
								}

								#region Mesh tools

								public void BakeRotation (LiquidVolume lv) {

												if (PrefabUtility.GetPrefabObject (lv.gameObject) != null) {
																PrefabUtility.DisconnectPrefabInstance (lv.gameObject);
												}

												MeshFilter mf = lv.GetComponent<MeshFilter> ();
												string meshPath = AssetDatabase.GetAssetPath (mf.sharedMesh);

												Mesh mesh = Instantiate<Mesh> (mf.sharedMesh) as Mesh;
												Vector3[] vertices = mesh.vertices;

												for (int k = 0; k < vertices.Length; k++) {
																vertices [k] = lv.transform.TransformVector (vertices [k]);
												}
												mesh.vertices = vertices;
												mesh.RecalculateBounds ();
												mf.sharedMesh = mesh;

												SaveMeshAsset (mesh, meshPath);

												lv.transform.localRotation = Quaternion.Euler (0, 0, 0);
												lv.transform.localScale = Vector3.one;
								}

								public void CenterPivot (LiquidVolume lv) {

												if (PrefabUtility.GetPrefabObject (lv.gameObject) != null) {
																PrefabUtility.DisconnectPrefabInstance (lv.gameObject);
												}

												MeshFilter mf = lv.GetComponent<MeshFilter> ();
												string meshPath = AssetDatabase.GetAssetPath (mf.sharedMesh);

												Mesh mesh = Instantiate<Mesh> (mf.sharedMesh) as Mesh;
												Vector3[] vertices = mesh.vertices;

												Vector3 midPoint = Vector3.zero;
												for (int k = 0; k < vertices.Length; k++) {
																midPoint += vertices [k];
												}
												midPoint /= vertices.Length;
												for (int k = 0; k < vertices.Length; k++) {
																vertices [k] -= midPoint;
												}
												mesh.vertices = vertices;
												mesh.RecalculateBounds ();
												mf.sharedMesh = mesh;

												SaveMeshAsset (mesh, meshPath);

												lv.transform.localPosition += midPoint;
								}

								void SaveMeshAsset (Mesh mesh, string originalMeshPath) {
												if (originalMeshPath == null)
																return;
												string newPath = Path.ChangeExtension (originalMeshPath, null);
												AssetDatabase.CreateAsset (mesh, newPath + "_centered");
												AssetDatabase.SaveAssets ();
												AssetDatabase.Refresh ();
								}


								#endregion


				}

}
