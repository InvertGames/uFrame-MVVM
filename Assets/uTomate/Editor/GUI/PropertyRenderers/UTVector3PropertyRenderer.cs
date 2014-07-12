// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEditor;
using UnityEngine;

[UTPropertyRenderer(typeof(UTVector3), typeof(Vector3))]
public class UTVector3PropertyRenderer : UTIPropertyRenderer
{
	public void Render (UTFieldWrapper fieldWrapper)
	{
		EditorGUILayout.BeginHorizontal();
		if (fieldWrapper.Label != null) {
			EditorGUILayout.PrefixLabel(fieldWrapper.Label);
		} else {
			EditorGUILayout.PrefixLabel(" ");
		}
		
		Vector3 val = (Vector3) fieldWrapper.Value;
		GUILayout.Label("X");
		val.x = EditorGUILayout.FloatField(val.x);
		GUILayout.Label("Y");
		val.y = EditorGUILayout.FloatField(val.y);
		GUILayout.Label("Z");
		val.z = EditorGUILayout.FloatField(val.z);
		fieldWrapper.Value = val;
		EditorGUILayout.EndHorizontal();
		
	}
}

