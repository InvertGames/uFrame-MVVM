//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;

/// <summary>
///  Renderer for enum values.
/// </summary>
[UTPropertyRenderer(typeof(Enum), typeof(UTEnum<>))]
public class UTEnumPropertyRenderer : UTIPropertyRenderer
{
	public void Render (UTFieldWrapper wrapper)
	{
		if (!wrapper.InspectorHint.multiSelect) {
			if (wrapper.Label != null) {
				wrapper.Value = EditorGUILayout.EnumPopup (wrapper.Label, (Enum)wrapper.Value);
			} else {
				wrapper.Value = EditorGUILayout.EnumPopup ((Enum)wrapper.Value);
			}
		} else {
			if (wrapper.Label != null) {
				wrapper.Value = EditorGUILayout.EnumMaskField (wrapper.Label, (Enum)wrapper.Value);
			} else {
				wrapper.Value = EditorGUILayout.EnumMaskField ((Enum)wrapper.Value);
			}
			
		}

	}
}

