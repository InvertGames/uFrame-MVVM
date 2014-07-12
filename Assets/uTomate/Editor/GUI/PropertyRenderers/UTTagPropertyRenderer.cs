// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEditor;
using UnityEngine;

[UTPropertyRenderer(typeof(UTTag))]
public class UTTagPropertyRenderer : UTIPropertyRenderer
{
	public void Render (UTFieldWrapper fieldWrapper)
	{
		if (fieldWrapper.Label != null) {
			fieldWrapper.Value = EditorGUILayout.TagField (fieldWrapper.Label, (string)fieldWrapper.Value);
		} else {
			fieldWrapper.Value = EditorGUILayout.TagField ((string)fieldWrapper.Value);
		}
	}
}

