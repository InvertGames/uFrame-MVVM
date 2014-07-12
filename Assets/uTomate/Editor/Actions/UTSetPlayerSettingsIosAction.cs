//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

[UTActionInfo(actionCategory = "Build")]
[UTDoc(title="Set iOS Player Settings", description="Sets the player settings for iOS builds.")]
[UTInspectorGroups(groups=new string[]{"Resolution & Presentation", "Splash Image", "Icon", "Identification", "Configuration", "Optimization"})]
[UTDefaultAction]
public class UTSetPlayerSettingsIosAction : UTAction,UTICanLoadSettingsFromEditor
{
	[UTDoc(description="Default screen orientation for mobiles.")]
	[UTInspectorHint(group="Resolution & Presentation", order=1)]
	public UTUIOrientation defaultOrientation;
	[UTDoc(description="Should status bar be hidden?")]
	[UTInspectorHint(group="Resolution & Presentation", order=2)]
	public UTBool statusBarHidden;
	[UTDoc(description="The status bar style to use.")]
	[UTInspectorHint(group="Resolution & Presentation", order=3)]
	public UTiOSStatusBarStyle statusBarStyle;
	[UTDoc(description="Use 32-bit display buffer?")]
	[UTInspectorHint(group="Resolution & Presentation", order=4)]
	public UTBool use32BitDisplayBuffer;
	[UTDoc(description="The type of activity indicator to be shown when the application loads.", title="Activity Indicator")]
	[UTInspectorHint(group="Resolution & Presentation", order=5)]
	public UTiOSShowActivityIndicatorOnLoading showActivityIndicatorOnLoading;
	//
	[UTDoc(description="The mobile splash image.")]
	[UTInspectorHint(group="Splash Image", order=1)]
	[UTRequiresLicense(UTLicense.IosPro)]
	public UTTexture2D mobileSplashScreen;
	[UTDoc(title="iPhone 3.5\"/Retina", description="High resolution iPhone 3.5\" splash image.")]
	[UTInspectorHint(group="Splash Image", order=2)]
	[UTRequiresLicense(UTLicense.IosPro)]
	public UTTexture2D highResIphone;
	[UTDoc(title="iPhone 4\"/Retina", description="High resolution iPhone 4\" splash image.")]
	[UTInspectorHint(group="Splash Image", order=3)]
	[UTRequiresLicense(UTLicense.IosPro)]
	public UTTexture2D highResIphoneTall;
	[UTDoc(title="iPad Portrait", description="iPad portrait splash image.")]
	[UTInspectorHint(group="Splash Image", order=4)]
	[UTRequiresLicense(UTLicense.IosPro)]
	public UTTexture2D iPadPortrait;
	[UTDoc(title="High Res. iPad Portrait", description="High resolution iPad portrait splash image.")]
	[UTInspectorHint(group="Splash Image", order=5)]
	[UTRequiresLicense(UTLicense.IosPro)]
	public UTTexture2D highResIpadPortrait;
	[UTDoc(title="iPad Landscape",description="iPad landscape splash image.")]
	[UTInspectorHint(group="Splash Image", order=6)]
	[UTRequiresLicense(UTLicense.IosPro)]
	public UTTexture2D iPadLandscape;
	[UTDoc(title="High Res. iPad Landscape",description="High resolution iPad landscape splash image.")]
	[UTInspectorHint(group="Splash Image", order=7)]
	[UTRequiresLicense(UTLicense.IosPro)]
	public UTTexture2D highResIpadLandscape;

	//
	[UTInspectorHint(group="Icon", order=1)]
	[UTDoc(description="Icon is prerendered?")]
	public UTBool prerenderedIcon;
	//
	[UTDoc(description="Application bundle identifier.")]
	[UTInspectorHint(group="Identification", order=1, required=true)]
	public UTString bundleIdentifier;
	[UTDoc(description="Application bundle version.")]
	[UTInspectorHint(group="Identification", order=2, required=true)]
	public UTString bundleVersion;
	//
	[UTDoc(description="The target device to build for.")]
	[UTInspectorHint(group="Configuration", order=1)]
	public UTiOSTargetDevice targetDevice;
#if UNITY_3_5	
	[UTDoc(description="Targeted platform.")]
	[UTInspectorHint(group="Configuration", order=2)]
	public UTiOSTargetPlatform targetPlatform;
#endif

#if !UNITY_3_5
	[UTDoc(description="Target GLES graphics.", title="Target GLES graphics")]
	[UTInspectorHint(group="Configuration", order=2)]
	public UTTargetGlesGraphics targetGlesGraphics;
#endif	
	
	[UTDoc(description="Targeted resolution.")]
	[UTInspectorHint(group="Configuration", order=3)]
	public UTiOSTargetResolution targetResolution;
	[UTDoc(description="Accelerometer frequency in Hertz. Valid values are 0, 15, 30, 60 and 100.")]
	[UTInspectorHint(group="Configuration", order=4)]
	public UTInt accelerometerFrequency;
	[UTInspectorHint(group="Configuration", order=5)]
	[UTDoc(description="Application requires persistent WiFi?")]
	public UTBool requiresPersistentWifi;
	[UTDoc(description="Application should exit when suspended to background?")]
	[UTInspectorHint(group="Configuration", order=6)]
	public UTBool exitOnSuspend;
	//
	[UTDoc(description=".NET API compatibility level.")]
	[UTInspectorHint(group="Optimization", order=1)]
	public UTApiCompatibilityLevel apiCompatibilityLevel;
	[UTDoc(description="Additional AOT compilation options.")]
	[UTInspectorHint(group="Optimization", order=2)]
	public UTString aotCompilationOptions;
	[UTDoc(description="Active iOS SDK version used for build")]
	[UTInspectorHint(group="Optimization", order=3)]
	public UTiOSSdkVersion sdkVersion;
	[UTDoc(description="Deployment minimal version of iOS.")]
	[UTInspectorHint(group="Optimization", order=4)]
	public UTiOSTargetOsVersion targetOsVersion;
	[UTDoc(description="Script calling optimization level.")]
	[UTInspectorHint(group="Optimization", order=5)]
	public UTScriptCallOptimizationLevel scriptCallOptimizationLevel;
	[UTDoc(description="Should unused Mesh components be excluded from game build?")]
	[UTInspectorHint(group="Optimization", order=6)]
	public UTBool optimizeMeshData;
#if UNITY_3_5	
	[UTDoc(description="Outputs profiling information about Resources.UnloadUnusedAssets,")]
	[UTInspectorHint(group="Optimization", order=7)]
	public UTDebugUnloadMode debugUnloadMode;
#endif 
	
	
	public override IEnumerator Execute (UTContext context)
	{
		if (UTPreferences.DebugMode) {
			Debug.Log ("Modifying iOS player settings.", this);
		}
		var theBundleIdentifier = bundleIdentifier.EvaluateIn (context);
		if (string.IsNullOrEmpty (theBundleIdentifier)) {
			throw new UTFailBuildException ("You need to specify the bundle identifier.", this);
		}
		
		var theBundleVersion = bundleVersion.EvaluateIn (context);
		if (string.IsNullOrEmpty (theBundleVersion)) {
			throw new UTFailBuildException ("You need to specify the bundle version.", this);
		}
		
		var theFrequency = accelerometerFrequency.EvaluateIn(context);
		if (theFrequency != 0 && theFrequency != 15 && theFrequency != 30 && theFrequency != 60 && theFrequency != 100) {
			throw new UTFailBuildException("Invalid accelerometer frequency. Valid values for accelerometer frequencies are 0, 15, 30, 60 and 100.", this);
		}
		
		PlayerSettings.defaultInterfaceOrientation = defaultOrientation.EvaluateIn (context);
		PlayerSettings.statusBarHidden = statusBarHidden.EvaluateIn (context);
		PlayerSettings.iOS.statusBarStyle = statusBarStyle.EvaluateIn (context);
		PlayerSettings.use32BitDisplayBuffer = use32BitDisplayBuffer.EvaluateIn (context);
		PlayerSettings.iOS.showActivityIndicatorOnLoading = showActivityIndicatorOnLoading.EvaluateIn (context);
		
		UTPlayerSettingsExt extSettings = new UTPlayerSettingsExt();
		extSettings.MobileSplashScreen = mobileSplashScreen.EvaluateIn(context);
		extSettings.IPhoneHighResSplashScreen = highResIphone.EvaluateIn(context);
		extSettings.IPhoneTallHighResSplashScreen = highResIphoneTall.EvaluateIn(context);
		extSettings.IPadPortraitSplashScreen = iPadPortrait.EvaluateIn(context);
		extSettings.IPadLandscapeSplashScreen = iPadLandscape.EvaluateIn(context);
		extSettings.IPadHighResPortraitSplashScreen = highResIpadPortrait.EvaluateIn(context);
		extSettings.IPadHighResLandscapeSplashScreen = highResIpadLandscape.EvaluateIn(context);
		extSettings.Apply();
		
		PlayerSettings.iOS.prerenderedIcon = prerenderedIcon.EvaluateIn (context);
		

		PlayerSettings.bundleIdentifier = theBundleIdentifier;
		PlayerSettings.bundleVersion = theBundleVersion;

		PlayerSettings.iOS.targetDevice = targetDevice.EvaluateIn (context);
#if UNITY_3_5		
		PlayerSettings.iOS.targetPlatform = targetPlatform.EvaluateIn (context);
#endif
#if !UNITY_3_5
		PlayerSettings.targetGlesGraphics = targetGlesGraphics.EvaluateIn(context);
#endif
		PlayerSettings.iOS.targetResolution = targetResolution.EvaluateIn (context);
		PlayerSettings.accelerometerFrequency = theFrequency;
		PlayerSettings.iOS.requiresPersistentWiFi = requiresPersistentWifi.EvaluateIn (context);
		PlayerSettings.iOS.exitOnSuspend = exitOnSuspend.EvaluateIn (context);
		
		PlayerSettings.apiCompatibilityLevel = apiCompatibilityLevel.EvaluateIn(context);
		PlayerSettings.aotOptions = aotCompilationOptions.EvaluateIn(context);
		PlayerSettings.iOS.sdkVersion = sdkVersion.EvaluateIn (context);
		PlayerSettings.iOS.targetOSVersion = targetOsVersion.EvaluateIn (context);
		PlayerSettings.iOS.scriptCallOptimization = scriptCallOptimizationLevel.EvaluateIn (context);
		PlayerSettings.stripUnusedMeshComponents = optimizeMeshData.EvaluateIn(context);
#if UNITY_3_5
		PlayerSettings.debugUnloadMode = debugUnloadMode.EvaluateIn(context);
#endif		
		
		if (UTPreferences.DebugMode) {
			Debug.Log ("iOS player settings modified.", this);
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Build/Set iOS Player Settings",  false, 250)]
	public static void AddAction ()
	{
		var result = Create<UTSetPlayerSettingsIosAction> ();
		result.LoadSettings ();
	}
	
	
	/// <summary>
	/// Loads current player settings.
	/// </summary>
	public void LoadSettings ()
	{
		
		defaultOrientation.Value = PlayerSettings.defaultInterfaceOrientation;
		defaultOrientation.UseExpression = false;
		
		statusBarHidden.Value = PlayerSettings.statusBarHidden;
		statusBarHidden.UseExpression = false;
		
		statusBarStyle.Value = PlayerSettings.iOS.statusBarStyle;
		statusBarStyle.UseExpression = false;

		use32BitDisplayBuffer.Value = PlayerSettings.use32BitDisplayBuffer;
		use32BitDisplayBuffer.UseExpression = false;

		showActivityIndicatorOnLoading.Value = PlayerSettings.iOS.showActivityIndicatorOnLoading;
		showActivityIndicatorOnLoading.UseExpression = false;
		
		UTPlayerSettingsExt extSettings = new UTPlayerSettingsExt();
		
		mobileSplashScreen.Value = extSettings.MobileSplashScreen;
		mobileSplashScreen.UseExpression = false;
		
		highResIphone.Value = extSettings.IPhoneHighResSplashScreen;
		highResIphone.UseExpression = false;
		
		highResIphoneTall.Value = extSettings.IPhoneTallHighResSplashScreen;
		highResIphoneTall.UseExpression = false;
		
		iPadPortrait.Value =  extSettings.IPadPortraitSplashScreen;
		iPadPortrait.UseExpression = false;
		
		iPadLandscape.Value = extSettings.IPadLandscapeSplashScreen;
		iPadLandscape.UseExpression = false;
		
		highResIpadPortrait.Value = extSettings.IPadHighResPortraitSplashScreen;
		highResIpadPortrait.UseExpression = false;
		
		highResIpadLandscape.Value = extSettings.IPadHighResLandscapeSplashScreen;
		highResIpadLandscape.UseExpression = false;
		
		prerenderedIcon.Value = PlayerSettings.iOS.prerenderedIcon;
		prerenderedIcon.UseExpression = false;

		bundleIdentifier.Value = PlayerSettings.bundleIdentifier;
		bundleIdentifier.UseExpression = false;
		
		bundleVersion.Value = PlayerSettings.bundleVersion;
		bundleVersion.UseExpression = false;
		
		targetDevice.Value = PlayerSettings.iOS.targetDevice;
		targetDevice.UseExpression = false;
		
#if UNITY_3_5
		targetPlatform.Value = PlayerSettings.iOS.targetPlatform;
		targetPlatform.UseExpression = false;
#endif		
		
#if !UNITY_3_5
		targetGlesGraphics.Value = PlayerSettings.targetGlesGraphics;
		targetGlesGraphics.UseExpression = false;
#endif
		
		targetResolution.Value = PlayerSettings.iOS.targetResolution;
		targetResolution.UseExpression = false;
		
		accelerometerFrequency.Value = PlayerSettings.accelerometerFrequency;
		accelerometerFrequency.UseExpression = false;

		requiresPersistentWifi.Value = PlayerSettings.iOS.requiresPersistentWiFi;
		requiresPersistentWifi.UseExpression = false;
		
		exitOnSuspend.Value = PlayerSettings.iOS.exitOnSuspend;
		exitOnSuspend.UseExpression = false;
		
		apiCompatibilityLevel.Value = PlayerSettings.apiCompatibilityLevel;
		apiCompatibilityLevel.UseExpression = false;
		
		aotCompilationOptions.Value = PlayerSettings.aotOptions;
		aotCompilationOptions.UseExpression = false;
		
		sdkVersion.Value = PlayerSettings.iOS.sdkVersion;
		sdkVersion.UseExpression = false;
		
		targetOsVersion.Value = PlayerSettings.iOS.targetOSVersion;
		targetOsVersion.UseExpression = false;
		
		scriptCallOptimizationLevel.Value = PlayerSettings.iOS.scriptCallOptimization;
		scriptCallOptimizationLevel.UseExpression = false;
		
		optimizeMeshData.Value = PlayerSettings.stripUnusedMeshComponents;
		optimizeMeshData.UseExpression = false;

#if UNITY_3_5
		debugUnloadMode.Value = PlayerSettings.debugUnloadMode;
		debugUnloadMode.UseExpression = false;
#endif		
	}
	
	public string LoadSettingsUndoText {
		get {
			return "Load iOS specific player settings";
		}
	}
}
