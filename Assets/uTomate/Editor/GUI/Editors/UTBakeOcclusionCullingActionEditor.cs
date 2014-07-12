//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UTBakeOcclusionCullingAction))]
public class UTBakeOcclusionCullingActionEditor : UTInspectorBase
{
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		EditorGUILayout.Space ();
		EditorGUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		if (GUILayout.Button ("Load defaults")) {
			LoadDefaults ();
		}
		EditorGUILayout.EndHorizontal ();
	}
	
	private void LoadDefaults ()
	{
		var action = (UTBakeOcclusionCullingAction)target;

		CUUndoUtility.RegisterUndo (action, "Load default occlusion culling settings");
		UTBakeOcclusionCullingAction.LoadDefaults (action);
		EditorUtility.SetDirty (action);
	}

#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
	public override UTVisibilityDecision IsVisible (System.Reflection.FieldInfo fieldInfo)
	{
		if (fieldInfo.Name == "memoryLimit") {
			var self = (UTBakeOcclusionCullingAction)target;
			return (self.mode.UseExpression || self.mode.Value != StaticOcclusionCullingMode.AutomaticPortals) ? 
				UTVisibilityDecision.Visible : UTVisibilityDecision.Invisible;
		}
		return base.IsVisible (fieldInfo);
	}
#endif

}


