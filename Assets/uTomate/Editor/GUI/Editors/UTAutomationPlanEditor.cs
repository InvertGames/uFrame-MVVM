//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UTAutomationPlan))]
public class UTAutomationPlanEditor : Editor{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();
		EditorGUILayout.Space();
		
		EditorGUILayout.HelpBox("You can directly run this plan from here.", MessageType.None);
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Run this plan")) {
			UTomate.Run(target as UTAutomationPlan);
		}
		EditorGUILayout.EndHorizontal();
	}
}

