//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;
using UObject = UnityEngine.Object;

[UTActionInfo(actionCategory = "Build")]
[UTDoc(title="Build Asset Bundle", description="Builds an asset bundle.")]
[UTRequiresLicense(UTLicense.UnityPro)]
[UTInspectorGroups(groups=new string[]{"Bundle", "Contents", "Dependencies"})]
[UTDefaultAction]
public class UTBuildAssetBundleAction : UTAction
{
	[UTDoc(description="The type of bundle that should be built.")]
	[UTInspectorHint(group="Bundle", order=0, required=true)]
	public UTBundleType bundleType;
	[UTDoc(description="The target platform for which the asset bundle should be built.")]
	[UTInspectorHint(group="Bundle", order=1, required=true)]
	public UTBuildTarget targetPlatform;
	[UTDoc(description="Forces inclusion of the entire asset.")]
	[UTInspectorHint(group="Bundle", order=2)]
	public UTBool completeAssets;
	[UTDoc(title="No Type Information", description="Do not include type information within the AssetBundle.")]
	[UTInspectorHint(group="Bundle", order=3)]
	public UTBool disableWriteTypeTree;
	[UTDoc(title="Deterministic", description="Builds an asset bundle using a hash for the id of the object stored in the asset bundle.")]
	[UTInspectorHint(group="Bundle", order=4)]
	public UTBool deterministicAssetBundle;
	[UTDoc(title="Uncompressed", description="Builds an uncompressed asset bundle.")]
	[UTInspectorHint(group="Bundle", order=5)]
	public UTBool uncompressedAssetBundle;
	[UTDoc(title="Scenes From Settings", description="If true the list of scenes for the build will be read from Unity's editor build settings. Otherwise you can specify them using the includes/excludes properties.")]
	[UTInspectorHint(group="Contents", order=0)]
	public UTBool useScenesFromBuildSettings;
	[UTDoc(description="Path of the main asset. A specific object that can be conveniently retrieved using AssetBundle.mainAsset. Can contain wildcards but must yield a unique asset name.")]
	[UTInspectorHint(group="Contents", order=1)]
	public UTString mainAsset;
	[UTDoc(description="The assets/scenes that should be included into the asset bundle.")]
	[UTInspectorHint(group="Contents", order=2)]
	public UTString[] includes;
	[UTDoc(description="The assets/scenes that should not be included into the asset bundle.")]
	[UTInspectorHint(group="Contents", order=3)]
	public UTString[] excludes;
	[UTDoc(description="Includes all dependencies.")]
	[UTInspectorHint(group="Contents", order=4)]
	public UTBool collectDependencies;
	[UTDoc(description="If set, PushDependencies will be called before building this bundle which makes this bundle's contents available as dependencies to following bundles.")]
	[UTInspectorHint(group="Dependencies", order=0)]
	public UTBool pushDependencies;
	[UTDoc(description="If set, PopDependencies will be called after building this bundle.")]
	[UTInspectorHint(group="Dependencies", order=1)]
	public UTBool popDependencies;
	[UTDoc(description="If set, PopDependencies will be called for each currently open dependency stack frame after building this bundle.")]
	[UTInspectorHint(group="Dependencies", order=2)]
	public UTBool popAllDependencies;
	[UTDoc(description="Name and path to the output file where to place the generated asset bundle.")]
	[UTInspectorHint(required=true, displayAs=UTInspectorHint.DisplayAs.SaveFileSelect, caption="Select output file.")]
	public UTString outputFileName;
	
	/// <summary>
	/// The open stack frames.
	/// </summary>
	public static int openStackFrames;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theOutputFile = outputFileName.EvaluateIn (context);
		if (string.IsNullOrEmpty (theOutputFile)) {
			throw new UTFailBuildException ("You must specify an output file name.", this);
		}
	
		var theBundleType = bundleType.EvaluateIn (context);
		
		var theFiles = new string[0];
		
		if (theBundleType == UTTypeOfBundle.SimpleAssetBundle || !useScenesFromBuildSettings.EvaluateIn (context)) {
			var theIncludes = EvaluateAll (includes, context);
			var theExcludes = EvaluateAll (excludes, context);
			theFiles = UTFileUtils.CalculateFileset (theIncludes, theExcludes);
		
			UTFileUtils.FullPathToProjectPath (theFiles);
		} else {
			var scenes = EditorBuildSettings.scenes;
			theFiles = Array.ConvertAll<EditorBuildSettingsScene,string> (scenes, scene => scene.path);
		}
		
		if (UTPreferences.DebugMode) {
			foreach (var file in theFiles) {
				Debug.Log ("Including: " + file, this);
			}
		}
		Debug.Log ("Including " + theFiles.Length + " assets/scenes.");
		
		if (theBundleType == UTTypeOfBundle.StreamedScenes && theFiles.Length == 0) {
			throw new UTFailBuildException ("No scenes have been selected. Unable to build a streamed scenes asset bundle with no scenes.", this);
		}
		
		var theMainAsset = "";
		UObject realMainAsset = null;
		UObject[] realAssets = null;
		
		if (theBundleType == UTTypeOfBundle.SimpleAssetBundle) {
			theMainAsset = mainAsset.EvaluateIn (context);
			if (!String.IsNullOrEmpty(theMainAsset)) {
				if (theMainAsset.Contains ("*")) {
					var finalList = UTFileUtils.CalculateFileset (new string[]{theMainAsset}, new string[0]);
					if (finalList.Length != 1) {
						throw new UTFailBuildException ("Main asset wildcard " + theMainAsset + " yielded " +
							finalList.Length + " results but should yield exactly one asset.", this);
					}
					theMainAsset = UTFileUtils.FullPathToProjectPath (finalList [0]);
				}
				
				// now get the real objects for the paths
				realMainAsset = AssetDatabase.LoadMainAssetAtPath (theMainAsset);
				
				if (realMainAsset == null) {
					throw new UTFailBuildException ("Unable to load the main asset " + theMainAsset + " from asset database.", this);
				}
			}

			realAssets = Array.ConvertAll<string,UObject> (theFiles, file => {
				var result = AssetDatabase.LoadMainAssetAtPath (file);
				if (result == null) {
					throw new UTFailBuildException ("Unable to load the asset " + file, this);
				}
				return result;
			});
		}
		
		
		var theBuildTarget = targetPlatform.EvaluateIn (context);
		
		if (pushDependencies.EvaluateIn (context)) {
			UTAssetDependencyStack.Push ();
		}
		
		try {
			UTFileUtils.EnsureParentFolderExists (theOutputFile);
			if (theBundleType == UTTypeOfBundle.StreamedScenes) {
				Debug.Log ("Building streamed scenes asset bundle.");
				var result = BuildPipeline.BuildStreamedSceneAssetBundle (theFiles, theOutputFile, theBuildTarget);
				if (!string.IsNullOrEmpty (result)) {
					throw new UTFailBuildException ("Building streamed scene asset bundle failed. " + result, this);
				}
			} else {
				Debug.Log ("Building asset bundle.");
				var realCollectDependencies = collectDependencies.EvaluateIn (context);
				var realCompleteAssets = completeAssets.EvaluateIn (context);
				var realDisableWriteTypeTree = disableWriteTypeTree.EvaluateIn (context);
				var realDeterministicAssetBundle = deterministicAssetBundle.EvaluateIn (context);
				var realUncompressedAssetBundle = uncompressedAssetBundle.EvaluateIn (context);
		
				var buildOpts = (BuildAssetBundleOptions)0;
#if UNITY_3_5			
				var opts = BuildOptions.None;
#endif
				if (realCollectDependencies) {
					buildOpts |= BuildAssetBundleOptions.CollectDependencies;
				}

				if (realCompleteAssets) {
					buildOpts |= BuildAssetBundleOptions.CompleteAssets;
				}
		
				if (realDisableWriteTypeTree) {
					buildOpts |= BuildAssetBundleOptions.DisableWriteTypeTree;
				}
		
				if (realDeterministicAssetBundle) {
					buildOpts |= BuildAssetBundleOptions.DeterministicAssetBundle;
				}
		
				if (realUncompressedAssetBundle) {
#if UNITY_3_5
					opts = BuildOptions.UncompressedAssetBundle;
#else
					buildOpts |= BuildAssetBundleOptions.UncompressedAssetBundle;
#endif
				}

#if UNITY_3_5
				if (!BuildPipeline.BuildAssetBundle (realMainAsset, realAssets, theOutputFile, buildOpts, theBuildTarget, opts)) {
					throw new UTFailBuildException ("Building asset bundle failed.", this);	
				}
#else
				if (!BuildPipeline.BuildAssetBundle (realMainAsset, realAssets, theOutputFile, buildOpts, theBuildTarget)) {
					throw new UTFailBuildException ("Building asset bundle failed.", this);	
				}
#endif
			}
			Debug.Log ("Built asset bundle at " + theOutputFile);
		} finally {
			if (popAllDependencies.EvaluateIn (context)) {
				UTAssetDependencyStack.PopAll ();
			} else if (popDependencies.EvaluateIn (context)) {
				UTAssetDependencyStack.Pop ();
			}
		}
		yield return "";
		
	}
	
	[MenuItem("Assets/Create/uTomate/Build/Build Asset Bundle", false, 370)]
	public static void AddAction ()
	{
		Create<UTBuildAssetBundleAction> ();
	}
	
	public enum UTTypeOfBundle
	{
		SimpleAssetBundle,
		StreamedScenes
	}
}

