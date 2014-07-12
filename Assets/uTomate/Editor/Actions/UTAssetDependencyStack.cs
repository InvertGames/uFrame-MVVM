//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Helper object for managing the asset dependency stack.
/// </summary>
public class UTAssetDependencyStack
{
	private UTAssetDependencyStack ()
	{
	}
	
	private const string SettingName = "uTomate.dependencyStackDepth";
	
	public static void Push() {
		var stackDepth = EditorPrefs.GetInt(SettingName, 0);
		BuildPipeline.PushAssetDependencies();
		if (UTPreferences.DebugMode) {
			Debug.Log ("Pushed dependencies. Stack height is now " + (stackDepth+1));
		}
		EditorPrefs.SetInt(SettingName, stackDepth+1);
	}
	
	public static void Pop() {
		var stackDepth = EditorPrefs.GetInt (SettingName, 0);
		if (stackDepth > 0) {
			BuildPipeline.PopAssetDependencies();
			if (UTPreferences.DebugMode) {
				Debug.Log ("Popped dependencies. Stack height is now " + (stackDepth-1));
			}
			EditorPrefs.SetInt(SettingName, stackDepth-1);
		}
		else {
			throw new UTFailBuildException("There were more pop dependencies than pushdependencies. Make sure your push/pop dependencies calls are balanced!", null);
		}
	}
	
	
	public static void PopAll() {
		var stackDepth = EditorPrefs.GetInt (SettingName, 0);
		while(stackDepth > 0 ) {
			BuildPipeline.PopAssetDependencies();
			stackDepth--;
		}
		if (UTPreferences.DebugMode) {
			Debug.Log ("Popped all remaining dependencies. Stack height is now 0.");
		}
		EditorPrefs.SetInt(SettingName, 0);
	}
	
	[MenuItem("Assets/Reset Asset Dependency Stack", false, 720)]
	public static void Reset() {
		Debug.Log ("Trying to reset the asset dependency stack.");
		PopAll();
		EditorPrefs.SetInt(SettingName,0);
		Debug.Log ("Asset dependency stack reset.");
	}
}

