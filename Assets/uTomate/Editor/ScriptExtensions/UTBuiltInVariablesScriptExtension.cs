//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.IO;
using UnityEngine;

[UTScriptExtension(0)]
public class UTBuiltInVariablesScriptExtension : UTIContextAware
{
	
	public UTContext Context {
		set {
			SetupVariables (value);
		}
	}
	
	private void SetupVariables (UTContext context)
	{
		context ["project:root"] = UTFileUtils.ProjectRoot;
		context ["project:assets"] = UTFileUtils.ProjectAssets;
		
		var platform = Environment.OSVersion.Platform;
		if (Application.platform == RuntimePlatform.OSXEditor) {
			platform = PlatformID.MacOSX; // seems to be some bug in Mono returning "Unix" when being on a mac.
		}
		
		context ["os:platform"] = platform.ToString ();
		
		var isUnixLike = platform == PlatformID.MacOSX || platform == PlatformID.Unix;
		context ["os:pathSeparatorType"] = isUnixLike ? "Unix" : "Windows";
		context ["os:pathSeparatorChar"] = isUnixLike ? "/" : "\\";
		
		if (isUnixLike) {
			context ["user:home"] = UTFileUtils.NormalizeSlashes (Environment.GetEnvironmentVariable ("HOME"));
		} else if (platform == PlatformID.Win32NT) {
			context ["user:home"] = UTFileUtils.NormalizeSlashes (Environment.ExpandEnvironmentVariables ("%HOMEDRIVE%%HOMEPATH%"));
		} else {
			Debug.Log ("Unable to detect underlying os. Property 'user:home' is not available.");
		}
		
		context ["user:desktop"] = UTFileUtils.NormalizeSlashes (Environment.GetFolderPath (Environment.SpecialFolder.Desktop));
		
		context ["unity:isUnityPro"] = UTils.IsUnityPro;
		context ["unity:supportsAndroid"] = UTils.IsBuildTargetSupported(UnityEditor.BuildTarget.Android);
		context ["unity:supportsIos"] = UTils.IsBuildTargetSupported(UnityEditor.BuildTarget.iPhone);
		context ["unity:version"] = Application.unityVersion;
		
		context ["utomate:debugMode"] = UTPreferences.DebugMode;
		
		context ["project:picard"] = "Make it so!";	
		context ["project:worf"] = "Torpedos ready, sir!";
		context ["project:bones"] = "I'm a doctor, no game developer!";
	}
	
}

