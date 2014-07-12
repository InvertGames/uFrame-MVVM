//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UTBakeNavMeshAction))]
public class UTBakeNavMeshActionEditor : UTInspectorBase
{
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		EditorGUILayout.Space ();
		EditorGUILayout.HelpBox ("For convenience, you can load the currently set editor settings into this action, using the button below.", MessageType.None);
		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Load settings")) {
			LoadSettings ();
		}
		EditorGUILayout.EndHorizontal ();
	}
	
	private void LoadSettings ()
	{
		var action = (UTBakeNavMeshAction)target;

		CUUndoUtility.RegisterUndo (action, "Load default nav mesh settings");
		UTBakeNavMeshAction.LoadFromSettings (action);
		GUIUtility.keyboardControl = 0; // unfocus anything
		EditorUtility.SetDirty (action);
	}
	
}

