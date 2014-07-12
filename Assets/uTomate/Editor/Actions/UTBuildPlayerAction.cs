//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;

[UTActionInfo(actionCategory = "Build")]
[UTDoc(title="Build Unity Player", description="Builds a player for the desired target platform.")]
[UTRequiresLicense(UTLicense.UnityPro)]
[UTInspectorGroups(groups=new string[]{"Player", "Dependencies"})]
[UTDefaultAction]
public class UTBuildPlayerAction : UTAction
{
	[UTDoc(description="The target platform for which the player should be build.")]
	[UTInspectorHint(group="Player", order=0, required=true)]
	public UTBuildTarget targetPlatform;
	[UTDoc(title="Scenes From Settings", description="If true the list of scenes for the build will be read from Unity's editor build settings. Otherwise you can specify them using the includes/excludes properties.")]
	[UTInspectorHint(group="Player", order=1)]
	public UTBool useScenesFromBuildSettings;
	[UTDoc(description="The scenes to include into the build.")]
	[UTInspectorHint(group="Player", order=2)]
	public UTString[] includes;
	[UTDoc(description="The scenes to exclude from the build.")]
	[UTInspectorHint(group="Player", order=3)]
	public UTString[] excludes;
	[UTDoc(description="Should this be a development build?")]
	[UTInspectorHint(group="Player", order=4)]
	public UTBool developmentBuild;
	[UTDoc(description="Should the build player be run after build?")]
	[UTInspectorHint(group="Player", order=5)]
	public UTBool runTheBuiltPlayer;
	[UTDoc(description="Show the build player in explorer/finder after build?")]
	[UTInspectorHint(group="Player", order=6)]
	public UTBool showTheBuiltPlayer;
	[UTDoc(description="Should the scenes be built in a way that they can be accessed with the WWW class?")]
	[UTInspectorHint(group="Player", order=7)]
	public UTBool buildStreamedScenes;
	[UTDoc(description="Should external modifications to the player be accepted? If not the player will be overwritten when building it.")]
	[UTInspectorHint(group="Player", order=8)]
	public UTBool acceptExternalModifications;
	[UTDoc(description="Web-Player, only: Should UnityObject.js be copied alongside Web Player so it wouldn't have to be downloaded from internet?")]
	[UTInspectorHint(group="Player", order=9)]
	public UTBool offlineDeployment;
	[UTDoc(description="Should the built player try to connect to the profiler in the editor when starting?")]
	[UTInspectorHint(group="Player", order=10)]
	public UTBool connectWithProfiler;
	[UTDoc(description="Should the player be debuggable?")]
	[UTInspectorHint(group="Player", order=11)]
	public UTBool allowDebugging;
	[UTDoc(description="iOS targets, only: should runtime libraries be symlinked when generating iOS XCode project. (Faster iteration time)?")]
	[UTInspectorHint(group="Player", order=12)]
	public UTBool symlinkLibraries;
	[UTDoc(description="Don't compress the data when creating the asset bundle?")]
	[UTInspectorHint(group="Player", order=13)]
	public UTBool uncompressedAssetBundle;
	[UTDoc(description="Web-Player, only: Publish Web Player online?")]
	[UTInspectorHint(group="Player", order=14)]
	public UTBool deployOnline;
	
	[UTDoc(description="If set, PushDependencies will be called before building this player.")]
	[UTInspectorHint(group="Dependencies", order=1)]
	public UTBool pushDependencies;
	[UTDoc(description="If set, PopDependencies will be called after building this player.")]
	[UTInspectorHint(group="Dependencies", order=2)]
	public UTBool popDependencies;
	[UTDoc(description="If set, PopDependencies will be called for each currently open dependency stack frame after building this player.")]
	[UTInspectorHint(group="Dependencies", order=3)]
	public UTBool popAllDependencies;

	[UTDoc(description="The output file name of the player. Make sure you add the correct extension for the player type (e.g. .app, .exe, etc.) or tick the 'Add Platform Extension' checkbox.")]
	[UTInspectorHint(required=true, order=0, displayAs=UTInspectorHint.DisplayAs.SaveFileSelect, caption="Select output file.")]
	public UTString outputFileName;
	[UTDoc(description="Should the platform specific extension (e.g. .app, .exe) be appended to the output file name?")]
	[UTInspectorHint(order=1)]
	public UTBool addPlatformExtension;

	public override IEnumerator Execute (UTContext context)
	{
		var theOutput = outputFileName.EvaluateIn (context);
		if (string.IsNullOrEmpty (theOutput)) {
			throw new UTFailBuildException ("You must specify an output file name.", this);
		}
		
		if (theOutput.StartsWith (Application.dataPath)) {
			throw new UTFailBuildException ("Building a player inside the assets folder will break the build. Please place it somewhere else.", this);
		}
		
		
		var theTarget = targetPlatform.EvaluateIn (context);
		
		if (addPlatformExtension.EvaluateIn(context)) {
			theOutput += GetPlatformExtension(theTarget);
			UTFileUtils.EnsureParentFolderExists(theOutput);
		}

#if UNITY_4_1
		// workaround for UNITY_4_1 offering StandaloneOSXUniversal but not being able to build for it.
		if (theTarget == BuildTarget.StandaloneOSXUniversal) {
			theTarget = BuildTarget.StandaloneOSXIntel;
		}
#endif

		var useBuildSettings = useScenesFromBuildSettings.EvaluateIn (context);
		
		string[] scenes;
		if (!useBuildSettings) {
			// get them from includes/excludes
			var theIncludes = EvaluateAll (includes, context);
			var theExcludes = EvaluateAll (excludes, context);
			
			var fileSet = UTFileUtils.CalculateFileset (theIncludes, theExcludes);
			if (fileSet.Length == 0) {
				throw new UTFailBuildException ("The file set yielded no scenes to include into the player.", this);
			}
			
			scenes = fileSet;
		} else {
			var scenesFromEditor = EditorBuildSettings.scenes;
			if (scenesFromEditor.Length == 0) {
				throw new UTFailBuildException ("There are no scenes set up in the editor build settings.", this);
			}
			var active = Array.FindAll (scenesFromEditor, scene => scene.enabled);
			scenes = Array.ConvertAll (active, scene => scene.path);
		}
		
		if (UTPreferences.DebugMode) {
			foreach (var entry in scenes) {
				Debug.Log ("Adding scene: " + entry, this);
			}
		}
		
		BuildOptions buildOptions = BuildOptions.None;
		if (developmentBuild.EvaluateIn (context)) {
			buildOptions |= BuildOptions.Development;
		}
		
		if (runTheBuiltPlayer.EvaluateIn (context)) {
			buildOptions |= BuildOptions.AutoRunPlayer;
		}
		
		if (showTheBuiltPlayer.EvaluateIn (context)) {
			buildOptions |= BuildOptions.ShowBuiltPlayer;
		}
		
		if (buildStreamedScenes.EvaluateIn (context)) {
			buildOptions |= BuildOptions.BuildAdditionalStreamedScenes;
		}
		
		if (acceptExternalModifications.EvaluateIn (context)) {
			buildOptions |= BuildOptions.AcceptExternalModificationsToPlayer;
		}
	
		if (connectWithProfiler.EvaluateIn (context)) {
			buildOptions |= BuildOptions.ConnectWithProfiler;
		}
		
		if (allowDebugging.EvaluateIn (context)) {
			buildOptions |= BuildOptions.AllowDebugging;
		}
		
		if (uncompressedAssetBundle.EvaluateIn (context)) {
			buildOptions |= BuildOptions.UncompressedAssetBundle;
		}
		
		if (theTarget == BuildTarget.WebPlayer || theTarget == BuildTarget.WebPlayerStreamed) {
			if (offlineDeployment.EvaluateIn (context)) {
				buildOptions |= BuildOptions.WebPlayerOfflineDeployment;
			}
			if (deployOnline.EvaluateIn (context)) {
				buildOptions |= BuildOptions.DeployOnline;
			}
		}
	
		if (theTarget == BuildTarget.iPhone) {
			if (symlinkLibraries.EvaluateIn (context)) {
				buildOptions |= BuildOptions.SymlinkLibraries;
			}
		}
		
		
		Debug.Log ("Building " + ObjectNames.NicifyVariableName (theTarget.ToString ()) + " player including " + scenes.Length + " scenes to " + theOutput);
		yield return "";
		if (pushDependencies.EvaluateIn (context)) {
			UTAssetDependencyStack.Push ();
		}
		try {
			// build the player.
		    var result = BuildPipeline.BuildPlayer (scenes, theOutput, theTarget, buildOptions);
			if (!string.IsNullOrEmpty(result)) {
				throw new UTFailBuildException("Building the player failed. " + result, this);
			}
		}
		finally {
			if (popAllDependencies.EvaluateIn (context)) {
				UTAssetDependencyStack.PopAll ();
			} else if (popDependencies.EvaluateIn (context)) {
				UTAssetDependencyStack.Pop ();
			}
		}
	}
	
	private string GetPlatformExtension (BuildTarget target)
	{
		switch (target) {
		case BuildTarget.FlashPlayer: 
				return ".swf";
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneWindows64:
			return ".exe";
		case BuildTarget.StandaloneOSXIntel:
#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1  )
		// UNITY_4_1 or later
		case BuildTarget.StandaloneOSXUniversal:
#endif
			return ".app";
		case BuildTarget.Android:
			return ".apk";
		default: return "";	
		}
	}
	
	[MenuItem("Assets/Create/uTomate/Build/Build Unity Player", false, 380)]
	public static void AddAction ()
	{
		Create<UTBuildPlayerAction> ();
	}

	
}

