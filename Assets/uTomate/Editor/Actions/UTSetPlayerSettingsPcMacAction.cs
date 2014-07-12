//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using _I_PlayerSettings = UnityEditor.PlayerSettings;
using _I_AspectRatio= UnityEditor.AspectRatio;
using _I_IEnumerator = System.Collections.IEnumerator;
using _I_Debug = UnityEngine.Debug;
using _I_MenuItem = UnityEditor.MenuItem;

#if UNITY_3_5
[UTDoc(title="Set PC and Mac Standalone Player Settings", description="Sets the player settings for PC and Mac standalone builds.")]
#endif
#if !UNITY_3_5
[UTDoc(title="Set PC, Mac and Linux Standalone Player Settings", description="Sets the player settings for PC, Mac and Linux standalone builds.")]
#endif
[UTActionInfo(actionCategory = "Build")]
[UTInspectorGroups(groups=new string[]{"Resolution & Presentation", "Aspect Ratios", "Splash Image", "Identification", "Rendering", "Optimization"})]
[UTDefaultAction]
public class UTSetPlayerSettingsPcMacAction : UTAction,UTICanLoadSettingsFromEditor
{
	[UTDoc(description="Default screen width of the standalone player window.")]
	[UTInspectorHint(group="Resolution & Presentation", order=1)]
	public UTInt defaultScreenWidth;
	[UTDoc(description="Default screen height of the standalone player window.")]
	[UTInspectorHint(group="Resolution & Presentation", order=2)]
	public UTInt defaultScreenHeight;
	[UTDoc(description="Continue running when application loses focus?")]
	[UTInspectorHint(group="Resolution & Presentation", order=3)]
	public UTBool runInBackground;
	[UTDoc(description="If enabled, the game will default to fullscreen mode.")]
	[UTInspectorHint(group="Resolution & Presentation", order=4)]
	public UTBool defaultIsFullscreen;
	[UTDoc(description="Defines if fullscreen games should darken secondary display.")]
	[UTInspectorHint(group="Resolution & Presentation", order=5)]
	public UTBool captureSingleScreen;
	[UTDoc(description="Behaviour of the resolution dialog on game launch.")]
	[UTInspectorHint(group="Resolution & Presentation", order=6)]
	public UTResolutionDialogSetting resolutionDialog;
	[UTDoc(description="Write a debug file with logging information?")]
	[UTInspectorHint(group="Resolution & Presentation", order=7)]
	public UTBool usePlayerLog;
	[UTDoc(description="Enable Mac App Store validation?")]
	[UTInspectorHint(group="Resolution & Presentation", order=8)]	
	public UTBool macAppStoreValidation;
#if !UNITY_3_5 	
	[UTDoc(description="Fullscreen mode to use on Macs.")]
	[UTInspectorHint(group="Resolution & Presentation", order=9)]
	public UTMacFullscreenMode macFullscreenMode;
#endif	
	
	//
	[UTDoc(description="Support 4:3 aspect ratio", title="4:3")]
	[UTInspectorHint(group="Aspect Ratios", order=1)]	
	public UTBool support4by3;
	[UTDoc(description="Support 5:4 aspect ratio", title="5:4")]
	[UTInspectorHint(group="Aspect Ratios", order=2)]	
	public UTBool support5by4;
	[UTDoc(description="Support 16:10 aspect ratio", title="16:10")]
	[UTInspectorHint(group="Aspect Ratios", order=3)]	
	public UTBool support16by10;
	[UTDoc(description="Support 16:9 aspect ratio", title="16:9")]
	[UTInspectorHint(group="Aspect Ratios", order=4)]	
	public UTBool support16by9;
	[UTDoc(description="Support other aspect ratios", title="Other")]
	[UTInspectorHint(group="Aspect Ratios", order=5)]	
	public UTBool supportOther;
	
	// 
	[UTDoc(description="The splash image for the resolution selection dialog.")]
	[UTInspectorHint(group="Splash Image", order=1)]
	[UTRequiresLicense(UTLicense.UnityPro)]
	public UTTexture2D splashImage;
	
	//
#if !UNITY_3_5	
	[UTDoc(description="The rendering path to use")]
	[UTInspectorHint(group="Rendering", order=1)]
	public UTRenderingPath renderingPath;
#endif	
	[UTDoc(description="The color space for lightmaps.")]
	[UTInspectorHint(group="Rendering", order=2)]
	public UTColorSpace colorSpace;
	
#if !UNITY_3_5	
	[UTDoc(description="Use Direct3D 11?", title="Use Direct3D 11")]
	[UTInspectorHint(group="Rendering", order=3)]
	public UTBool useDirect3D11;
#endif 	

	//
	[UTDoc(description=".NET API compatibility level.")]
	[UTInspectorHint(group="Optimization", order=1)]
	public UTApiCompatibilityLevel apiCompatibilityLevel;
	[UTDoc(description="Should unused Mesh components be excluded from game build?")]
	[UTInspectorHint(group="Optimization", order=6)]
	public UTBool optimizeMeshData;
#if UNITY_3_5	
	[UTDoc(description="Outputs profiling information about Resources.UnloadUnusedAssets,")]
	[UTInspectorHint(group="Optimization", order=7)]
	public UTDebugUnloadMode debugUnloadMode;
#endif 
	
	public override _I_IEnumerator Execute (UTContext context)
	{
		if (UTPreferences.DebugMode) {
			_I_Debug.Log ("Modifying PC/Mac player settings.", this);
		}
	
		_I_PlayerSettings.defaultScreenWidth = defaultScreenWidth.EvaluateIn (context);
		_I_PlayerSettings.defaultScreenHeight = defaultScreenHeight.EvaluateIn (context);
		_I_PlayerSettings.runInBackground = runInBackground.EvaluateIn (context);
		_I_PlayerSettings.defaultIsFullScreen = defaultIsFullscreen.EvaluateIn (context);
		_I_PlayerSettings.captureSingleScreen = captureSingleScreen.EvaluateIn (context);
		_I_PlayerSettings.displayResolutionDialog = resolutionDialog.EvaluateIn (context);
		_I_PlayerSettings.usePlayerLog = usePlayerLog.EvaluateIn (context);
		_I_PlayerSettings.useMacAppStoreValidation = macAppStoreValidation.EvaluateIn (context);
#if !UNITY_3_5
		_I_PlayerSettings.macFullscreenMode = macFullscreenMode.EvaluateIn(context);
#endif

		_I_PlayerSettings.SetAspectRatio (_I_AspectRatio.Aspect4by3, support4by3.EvaluateIn (context));
		_I_PlayerSettings.SetAspectRatio (_I_AspectRatio.Aspect5by4, support5by4.EvaluateIn (context));
		_I_PlayerSettings.SetAspectRatio (_I_AspectRatio.Aspect16by10, support16by10.EvaluateIn (context));
		_I_PlayerSettings.SetAspectRatio (_I_AspectRatio.Aspect16by9, support16by9.EvaluateIn (context));
		_I_PlayerSettings.SetAspectRatio (_I_AspectRatio.AspectOthers, supportOther.EvaluateIn (context));
		
		_I_PlayerSettings.resolutionDialogBanner = splashImage.EvaluateIn(context);

#if !UNITY_3_5
		_I_PlayerSettings.renderingPath = renderingPath.EvaluateIn(context);
#endif
		_I_PlayerSettings.colorSpace = colorSpace.EvaluateIn(context);
		
#if !UNITY_3_5
		_I_PlayerSettings.useDirect3D11 = useDirect3D11.EvaluateIn(context);
#endif
		
		_I_PlayerSettings.apiCompatibilityLevel = apiCompatibilityLevel.EvaluateIn (context);
		_I_PlayerSettings.stripUnusedMeshComponents = optimizeMeshData.EvaluateIn (context);
#if UNITY_3_5
		_I_PlayerSettings.debugUnloadMode = debugUnloadMode.EvaluateIn(context);
#endif		
		
		if (UTPreferences.DebugMode) {
			_I_Debug.Log ("PC/Mac player settings modified.", this);
		}
		
		yield return "";
	}
	
#if UNITY_3_5	
	[_I_MenuItem("Assets/Create/uTomate/Build/Set PC and Mac Standalone Player Settings",  false, 240)]
#endif
#if !UNITY_3_5
	[_I_MenuItem("Assets/Create/uTomate/Build/Set PC, Mac and Linux Standalone Player Settings",  false, 240)]
#endif
	public static void AddAction ()
	{
		var result = Create<UTSetPlayerSettingsPcMacAction> ();
		result.LoadSettings ();
	}
	
	
	/// <summary>
	/// Loads current player settings.
	/// </summary>
	public void LoadSettings ()
	{
		
		defaultScreenWidth.Value = _I_PlayerSettings.defaultScreenWidth;
		defaultScreenWidth.UseExpression = false;
		
		defaultScreenHeight.Value = _I_PlayerSettings.defaultScreenHeight;
		defaultScreenHeight.UseExpression = false;
		
		runInBackground.Value = _I_PlayerSettings.runInBackground;
		runInBackground.UseExpression = false;
		
		defaultIsFullscreen.Value = _I_PlayerSettings.defaultIsFullScreen;
		defaultIsFullscreen.UseExpression = false;
		
		captureSingleScreen.Value = _I_PlayerSettings.captureSingleScreen;
		captureSingleScreen.UseExpression = false;
		
		resolutionDialog.Value = _I_PlayerSettings.displayResolutionDialog;
		resolutionDialog.UseExpression = false;
		
		usePlayerLog.Value = _I_PlayerSettings.usePlayerLog;
		usePlayerLog.UseExpression = false;
		
		macAppStoreValidation.Value = _I_PlayerSettings.useMacAppStoreValidation;
		macAppStoreValidation.UseExpression = false;

#if !UNITY_3_5
		macFullscreenMode.Value = _I_PlayerSettings.macFullscreenMode;
		macFullscreenMode.UseExpression = false;
#endif
		
		support4by3.Value = _I_PlayerSettings.HasAspectRatio (_I_AspectRatio.Aspect4by3);
		support4by3.UseExpression = false;

		support5by4.Value = _I_PlayerSettings.HasAspectRatio (_I_AspectRatio.Aspect5by4);
		support5by4.UseExpression = false;
		
		support16by10.Value = _I_PlayerSettings.HasAspectRatio (_I_AspectRatio.Aspect16by10);
		support16by10.UseExpression = false;
	
		support16by9.Value = _I_PlayerSettings.HasAspectRatio (_I_AspectRatio.Aspect16by9);
		support16by9.UseExpression = false;

		supportOther.Value = _I_PlayerSettings.HasAspectRatio (_I_AspectRatio.AspectOthers);
		supportOther.UseExpression = false;

		splashImage.Value = _I_PlayerSettings.resolutionDialogBanner;
		splashImage.UseExpression = false;
		
#if !UNITY_3_5
		renderingPath.Value = _I_PlayerSettings.renderingPath;
		renderingPath.UseExpression = false;
#endif 
		colorSpace.Value = _I_PlayerSettings.colorSpace;
		colorSpace.UseExpression = false;

#if !UNITY_3_5
		useDirect3D11.Value = _I_PlayerSettings.useDirect3D11;
		useDirect3D11.UseExpression = false;
#endif 
		
		apiCompatibilityLevel.Value = _I_PlayerSettings.apiCompatibilityLevel;
		apiCompatibilityLevel.UseExpression = false;
				
		optimizeMeshData.Value = _I_PlayerSettings.stripUnusedMeshComponents;
		optimizeMeshData.UseExpression = false;

#if UNITY_3_5
		debugUnloadMode.Value = _I_PlayerSettings.debugUnloadMode;
		debugUnloadMode.UseExpression = false;
#endif		
	}
	
	public string LoadSettingsUndoText {
		get {
#if UNITY_3_5
			return "Load PC/Mac specific player settings";
#endif
#if !UNITY_3_5
			return "Load PC/Mac/Linux specific player settings";
#endif
		}
	}
}
