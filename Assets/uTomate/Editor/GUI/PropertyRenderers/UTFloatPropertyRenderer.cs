//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;

/// <summary>
///  Renderer for float values.
/// </summary>
[UTPropertyRenderer(typeof(float), typeof(UTFloat))]
public class UTFloatPropertyRenderer : UTIPropertyRenderer
{
	public void Render (UTFieldWrapper wrapper)
	{
		var hint = wrapper.InspectorHint;
		if (wrapper.Label != null) {
			if (hint.displayAs == UTInspectorHint.DisplayAs.Slider) {
				wrapper.Value = EditorGUILayout.Slider (wrapper.Label, (float)wrapper.Value, hint.minValue, hint.maxValue);					
			} else {
				wrapper.Value = EditorGUILayout.FloatField (wrapper.Label, (float)wrapper.Value);
			}
		} else {
			if (hint.displayAs == UTInspectorHint.DisplayAs.Slider) {
				wrapper.Value = EditorGUILayout.Slider ((float)wrapper.Value, hint.minValue, hint.maxValue);					
			} else {
				wrapper.Value = EditorGUILayout.FloatField ((float)wrapper.Value);
			}
		}
	}
}

