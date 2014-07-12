//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;

/// <summary>
///  Renderer for boolean values.
/// </summary>
[UTPropertyRenderer(typeof(bool), typeof(UTBool))]
public class UTBoolPropertyRenderer : UTIPropertyRenderer 
{
	public void Render (UTFieldWrapper fieldWrapper)
	{
		if (fieldWrapper.Label != null) {
			fieldWrapper.Value = EditorGUILayout.Toggle (fieldWrapper.Label, (bool)fieldWrapper.Value);
		} else {
			fieldWrapper.Value = EditorGUILayout.Toggle ((bool)fieldWrapper.Value);
		}
	}
}

