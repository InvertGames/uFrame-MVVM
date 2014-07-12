// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEditor;
using UnityEngine;

[UTPropertyRenderer(typeof(UTLayer))]
public class UTLayerPropertyRenderer : UTIPropertyRenderer
{
	public void Render (UTFieldWrapper fieldWrapper)
	{
		if (fieldWrapper.Label != null) {
			fieldWrapper.Value = EditorGUILayout.LayerField (fieldWrapper.Label, (int)fieldWrapper.Value);
		} else {
			fieldWrapper.Value = EditorGUILayout.LayerField ((int)fieldWrapper.Value);
		}
	}
}

