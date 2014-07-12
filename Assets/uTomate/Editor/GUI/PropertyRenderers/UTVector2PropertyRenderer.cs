// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEditor;
using UnityEngine;

[UTPropertyRenderer(typeof(UTVector2), typeof(Vector2))]
public class UTVector2PropertyRenderer : UTIPropertyRenderer
{
	public void Render (UTFieldWrapper fieldWrapper)
	{
		EditorGUILayout.BeginHorizontal();
		if (fieldWrapper.Label != null) {
			EditorGUILayout.PrefixLabel(fieldWrapper.Label);
		} else {
			EditorGUILayout.PrefixLabel(" ");
		}
		
		Vector2 val = (Vector2) fieldWrapper.Value;
		GUILayout.Label("X");
		val.x = EditorGUILayout.FloatField(val.x);
		GUILayout.Label("Y");
		val.y = EditorGUILayout.FloatField(val.y);
		fieldWrapper.Value = val;
		EditorGUILayout.EndHorizontal();
		
	}
}

