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

[UTActionInfo(actionCategory = "Build")]
[UTDoc(title="Set Build Scenes", description="Sets the list of scenes in the build settings. Scenes will be searched in current project.")]
[UTDefaultAction]
public class UTSetBuildScenes : UTAction
{
	[UTDoc(description="The scenes to include.")]
	[UTInspectorHint(order=0)]
	public UTString[] includes;
	[UTDoc(description="The scenes to exclude.")]
	[UTInspectorHint(order=1)]
	public UTString[] excludes;
	[UTDoc(description="If set to true, the scenes will be appended to the existing list of scenes, otherwise the existing list will be replaced.")]
	[UTInspectorHint(order=2)]
	public UTBool append;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theRealIncludes = EvaluateAll (includes, context);
		var theRealExcludes = EvaluateAll (excludes, context);
		
		var fileSet = UTFileUtils.CalculateFileset (theRealIncludes, theRealExcludes);
		UTFileUtils.FullPathToProjectPath(fileSet);
		
		
		if (fileSet.Length == 0) {
			Debug.LogWarning ("The list of scenes is empty.");
		}
		
		
		var doAppend = append.EvaluateIn (context);
		EditorBuildSettingsScene[] sceneArray;
		var offset = 0;
		if (doAppend) {
			if (fileSet.Length > 0) {
				var oldArray = EditorBuildSettings.scenes;
				var newArray = new EditorBuildSettingsScene[oldArray.Length + fileSet.Length];
				Array.Copy (oldArray, newArray, oldArray.Length);
				offset = oldArray.Length;
				sceneArray = newArray;
			}
			else {
				sceneArray = new EditorBuildSettingsScene[0];
			}
		} else {
			sceneArray = new EditorBuildSettingsScene[fileSet.Length];
		}
		
		for (int i=0; i< fileSet.Length; i++) {
			var scene = new EditorBuildSettingsScene (fileSet [i], true);
			sceneArray [offset + i] = scene;
		}
		
		EditorBuildSettings.scenes = sceneArray;
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Build/Set Build Scenes", false, 220)]
	public static void AddAction ()
	{
		Create<UTSetBuildScenes> ();
	}

}

