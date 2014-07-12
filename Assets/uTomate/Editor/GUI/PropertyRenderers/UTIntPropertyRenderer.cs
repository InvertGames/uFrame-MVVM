//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;

/// <summary>
///  Renderer for int values.
/// </summary>
[UTPropertyRenderer(typeof(int), typeof(UTInt))]
public class UTIntPropertyRenderer : UTIPropertyRenderer
{
	public void Render (UTFieldWrapper wrapper)
	{
		var hint = wrapper.InspectorHint;
		if (wrapper.Label != null) {
			if (hint.displayAs == UTInspectorHint.DisplayAs.Slider) {
				wrapper.Value = EditorGUILayout.IntSlider (wrapper.Label, (int)wrapper.Value, (int)hint.minValue, (int)hint.maxValue);					
			} else {
				wrapper.Value = EditorGUILayout.IntField (wrapper.Label, (int)wrapper.Value);
			}
		} else {
			if (hint.displayAs == UTInspectorHint.DisplayAs.Slider) {
				wrapper.Value = EditorGUILayout.IntSlider ((int)wrapper.Value, (int)hint.minValue, (int)hint.maxValue);					
			} else {
				wrapper.Value = EditorGUILayout.IntField ((int)wrapper.Value);
			}
		}
	}
}

