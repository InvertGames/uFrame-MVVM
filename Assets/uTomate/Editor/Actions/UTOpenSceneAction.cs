//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;

[UTActionInfo(actionCategory = "Scene Manipulation")]
[UTDoc(title="Open Scene", description="Opens the given scene. Any changes in the currently open scene will be discarded.")]
[UTDefaultAction]
public class UTOpenSceneAction : UTAction
{
	
	[UTInspectorHint(required=true, displayAs=UTInspectorHint.DisplayAs.OpenFileSelect, caption="Select scene to open.", extension="unity")]
	[UTDoc(description="The scene to be opened. Can contain wildcards but must yield a unique scene name. Path must be inside of $project.root.")]
	public UTString scene;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theScene = scene.EvaluateIn(context);
		
		if (theScene.Contains("*")) {
			var finalList = UTFileUtils.CalculateFileset(new string[]{theScene}, new string[0]);
			if (finalList.Length != 1) {
				throw new UTFailBuildException("Scene wildcard " + theScene + " yielded " +
					finalList.Length + " results but should yield exactly one scene.", this);
			}
			UTFileUtils.FullPathToProjectPath(finalList);
			theScene = finalList[0];
		}
		
		theScene = UTFileUtils.FullPathToProjectPath(theScene);

		if (UTPreferences.DebugMode) {
			Debug.Log("Opening scene: " + theScene, this);
		}
		var result = EditorApplication.OpenScene(theScene);
		if (!result) {
			throw new UTFailBuildException("Scene " + theScene + " could not be opened.", this);
		}
		yield return "";
	}

	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Open Scene", false, 280)]
	public static void AddAction() {
		Create<UTOpenSceneAction>();
	}
}

