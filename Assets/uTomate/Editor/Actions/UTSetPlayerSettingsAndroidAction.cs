//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[UTActionInfo(actionCategory = "Build", sinceUTomateVersion="1.1.0")]
[UTDoc(title="Set Android Player Settings", description="Sets the player settings for Android builds.")]
[UTInspectorGroups(groups=new string[]{"Resolution & Presentation", "Splash Image", "Identification", "Configuration", "Optimization", "Publishing Settings"})]
[UTDefaultAction]
public class UTSetPlayerSettingsAndroidAction : UTAction,UTICanLoadSettingsFromEditor
{
	[UTDoc(description="Default screen orientation for mobiles.")]
	[UTInspectorHint(group="Resolution & Presentation", order=1)]
	public UTUIOrientation defaultOrientation;
	[UTDoc(description="Should status bar be hidden?")]
	[UTInspectorHint(group="Resolution & Presentation", order=2)]
	public UTBool statusBarHidden;
	[UTDoc(description="Use 32-bit display buffer?")]
	[UTInspectorHint(group="Resolution & Presentation", order=3)]
	public UTBool use32BitDisplayBuffer;
	
	[UTDoc(description="Use 24-bit depth buffer?")]
	[UTInspectorHint(group="Resolution & Presentation", order=4)]
	public UTBool use24BitDepthBuffer;
	
	[UTDoc(description="The type of activity indicator to be shown when the application loads.", title="Activity Indicator")]
	[UTInspectorHint(group="Resolution & Presentation", order=5)]
	public UTAndroidShowActivityIndicatorOnLoading showActivityIndicatorOnLoading;
	//
	[UTDoc(description="The mobile splash image.")]
	[UTInspectorHint(group="Splash Image", order=1)]
	[UTRequiresLicense(UTLicense.AndroidPro)]
	public UTTexture2D mobileSplashScreen;	
	
	[UTDoc(description="Scaling operation to apply on the android splash image.")]
	[UTInspectorHint(group="Splash Image", order=2)]
	[UTRequiresLicense(UTLicense.AndroidPro)]
	public UTAndroidSplashScreenScale splashScaling;

	//
	[UTDoc(description="Application bundle identifier.")]
	[UTInspectorHint(group="Identification", order=1, required=true)]
	public UTString bundleIdentifier;
	[UTDoc(description="Application bundle versions.")]
	[UTInspectorHint(group="Identification", order=2, required=true)]
	public UTString bundleVersion;
	[UTDoc(description="Bundle version code")]
	[UTInspectorHint(group="Identification", order=3, required=false)]
	public UTInt bundleVersionCode;
	[UTDoc(description="Minium Android API level.", title="Minimum API Level")]
	[UTInspectorHint(group="Identification", order=4, required=false)]
	public UTAndroidSdkVersions minimumApiLevel;
	
	//
	[UTDoc(description="The target device to build for.", title="Device Filter")]
	[UTInspectorHint(group="Configuration", order=1)]
	public UTAndroidTargetDevice targetDevice;
#if UNITY_3_5	
	[UTDoc(description="Target graphics.", title="Graphics Level")]
	[UTInspectorHint(group="Configuration", order=2)]
	public UTAndroidTargetGraphics targetGraphics;
#endif

#if !UNITY_3_5
	[UTDoc(description="Target GLES graphics.", title="Target GLES graphics")]
	[UTInspectorHint(group="Configuration", order=2)]
	public UTTargetGlesGraphics targetGlesGraphics;
#endif	
	
	[UTDoc(description="Preferred install location.")]
	[UTInspectorHint(group="Configuration", order=3)]
	public UTAndroidPreferredInstallLocation installLocation;
	[UTDoc(description="Force internet permission?")]
	[UTInspectorHint(group="Configuration", order=4)]
	public UTBool forceInternetPermission;
	[UTInspectorHint(group="Configuration", order=5)]
	[UTDoc(description="Force SD Card write permission?")]
	public UTBool forceSDCardPermission;
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
	//
	[UTDoc(description="Path to the keystore to use.")]
	[UTInspectorHint(group="Publishing Settings", order=1, displayAs=UTInspectorHint.DisplayAs.OpenFileSelect)]
	public UTString keyStore;
	[UTDoc(description="Password for the key store.")]
	[UTInspectorHint(group="Publishing Settings", order=2, displayAs=UTInspectorHint.DisplayAs.Password)]
	public UTString keyStorePassword;
	[UTDoc(description="Alias of the key to use")]
	[UTInspectorHint(group="Publishing Settings", order=3)]
	public UTString keyAlias;
	[UTDoc(description="Password for the key to use.")]
	[UTInspectorHint(group="Publishing Settings", order=4, displayAs=UTInspectorHint.DisplayAs.Password)]
	public UTString keyPassword;
	
	public override IEnumerator Execute (UTContext context)
	{
		if (UTPreferences.DebugMode) {
			Debug.Log ("Modifying Android player settings.", this);
		}
		var theBundleIdentifier = bundleIdentifier.EvaluateIn (context);
		if (string.IsNullOrEmpty (theBundleIdentifier)) {
			throw new UTFailBuildException ("You need to specify the bundle identifier.", this);
		}
		
		var theBundleVersion = bundleVersion.EvaluateIn (context);
		if (string.IsNullOrEmpty (theBundleVersion)) {
			throw new UTFailBuildException ("You need to specify the bundle version.", this);
		}
		
		var theKeyStore = keyStore.EvaluateIn(context);
		if (!string.IsNullOrEmpty(theKeyStore) && !File.Exists(theKeyStore)) {
			throw new UTFailBuildException("The specified keystore does not exist.", this);
		}
		
		PlayerSettings.defaultInterfaceOrientation = defaultOrientation.EvaluateIn (context);
		PlayerSettings.statusBarHidden = statusBarHidden.EvaluateIn (context);
		PlayerSettings.use32BitDisplayBuffer = use32BitDisplayBuffer.EvaluateIn (context);
		PlayerSettings.Android.use24BitDepthBuffer = use24BitDepthBuffer.EvaluateIn(context);
		PlayerSettings.Android.showActivityIndicatorOnLoading = showActivityIndicatorOnLoading.EvaluateIn (context);
		
		UTPlayerSettingsExt extPlayerSettings = new UTPlayerSettingsExt();
		extPlayerSettings.MobileSplashScreen = mobileSplashScreen.EvaluateIn(context);
		extPlayerSettings.Apply();
		PlayerSettings.Android.splashScreenScale = splashScaling.EvaluateIn(context);
		
		PlayerSettings.bundleIdentifier = theBundleIdentifier;
		PlayerSettings.bundleVersion = theBundleVersion;
		PlayerSettings.Android.bundleVersionCode = bundleVersionCode.EvaluateIn(context);
		PlayerSettings.Android.minSdkVersion = minimumApiLevel.EvaluateIn(context);

		PlayerSettings.Android.targetDevice = targetDevice.EvaluateIn (context);
#if UNITY_3_5		
		PlayerSettings.Android.targetGraphics = targetGraphics.EvaluateIn (context);
#endif
#if !UNITY_3_5
		PlayerSettings.targetGlesGraphics = targetGlesGraphics.EvaluateIn(context);
#endif
		PlayerSettings.Android.preferredInstallLocation = installLocation.EvaluateIn (context);
		PlayerSettings.Android.forceInternetPermission = forceInternetPermission.EvaluateIn(context);
		PlayerSettings.Android.forceSDCardPermission = forceSDCardPermission.EvaluateIn (context);
		
		PlayerSettings.apiCompatibilityLevel = apiCompatibilityLevel.EvaluateIn(context);
		PlayerSettings.stripUnusedMeshComponents = optimizeMeshData.EvaluateIn(context);
#if UNITY_3_5
		PlayerSettings.debugUnloadMode = debugUnloadMode.EvaluateIn(context);
#endif		
		
		PlayerSettings.Android.keystoreName = theKeyStore;
		PlayerSettings.Android.keystorePass = keyStorePassword.EvaluateIn(context);
		
		PlayerSettings.Android.keyaliasName = keyAlias.EvaluateIn(context);
		PlayerSettings.Android.keyaliasPass = keyPassword.EvaluateIn(context);
		
		if (UTPreferences.DebugMode) {
			Debug.Log ("Android player settings modified.", this);
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Build/Set Android Player Settings",  false, 270)]
	public static void AddAction ()
	{
		var result = Create<UTSetPlayerSettingsAndroidAction> ();
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
		
		use32BitDisplayBuffer.Value = PlayerSettings.use32BitDisplayBuffer;
		use32BitDisplayBuffer.UseExpression = false;
		
		use24BitDepthBuffer.Value = PlayerSettings.Android.use24BitDepthBuffer;
		use24BitDepthBuffer.UseExpression = false;
		
		showActivityIndicatorOnLoading.Value = PlayerSettings.Android.showActivityIndicatorOnLoading;
		showActivityIndicatorOnLoading.UseExpression = false;
		
		UTPlayerSettingsExt extPlayerSettings = new UTPlayerSettingsExt();
		mobileSplashScreen.Value = extPlayerSettings.MobileSplashScreen;
		mobileSplashScreen.UseExpression = false;
		
		splashScaling.Value = PlayerSettings.Android.splashScreenScale;
		splashScaling.UseExpression = false;
		
		bundleIdentifier.Value = PlayerSettings.bundleIdentifier;
		bundleIdentifier.UseExpression = false;
		
		bundleVersion.Value = PlayerSettings.bundleVersion;
		bundleVersion.UseExpression = false;
		
		bundleVersionCode.Value = PlayerSettings.Android.bundleVersionCode;
		bundleVersionCode.UseExpression = false;
		
		minimumApiLevel.Value = PlayerSettings.Android.minSdkVersion;
		minimumApiLevel.UseExpression = false;
		
		targetDevice.Value = PlayerSettings.Android.targetDevice;
		targetDevice.UseExpression = false;
		
#if UNITY_3_5
		targetGraphics.Value = PlayerSettings.Android.targetGraphics;
		targetGraphics.UseExpression = false;
#endif		
		
#if !UNITY_3_5
		targetGlesGraphics.Value = PlayerSettings.targetGlesGraphics;
		targetGlesGraphics.UseExpression = false;
#endif
		
		installLocation.Value = PlayerSettings.Android.preferredInstallLocation;
		installLocation.UseExpression = false;
		
		forceInternetPermission.Value = PlayerSettings.Android.forceInternetPermission;
		forceInternetPermission.UseExpression = false;

		forceSDCardPermission.Value = PlayerSettings.Android.forceSDCardPermission;
		forceSDCardPermission.UseExpression = false;
		
		apiCompatibilityLevel.Value = PlayerSettings.apiCompatibilityLevel;
		apiCompatibilityLevel.UseExpression = false;
				
		optimizeMeshData.Value = PlayerSettings.stripUnusedMeshComponents;
		optimizeMeshData.UseExpression = false;

#if UNITY_3_5
		debugUnloadMode.Value = PlayerSettings.debugUnloadMode;
		debugUnloadMode.UseExpression = false;
#endif		
		keyStore.Value = PlayerSettings.Android.keystoreName;
		keyStore.UseExpression = false;
		
		keyStorePassword.Value = PlayerSettings.Android.keystorePass;
		keyStorePassword.UseExpression = false;
		
		keyAlias.Value = PlayerSettings.Android.keyaliasName;
		keyAlias.UseExpression = false;
		
		keyPassword.Value = PlayerSettings.Android.keyaliasPass;
		keyPassword.UseExpression = false;
		
		
		
	}
	
	public string LoadSettingsUndoText {
		get {
			return "Load Android specific player settings";
		}
	}
}
