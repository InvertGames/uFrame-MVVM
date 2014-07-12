//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UObject = UnityEngine.Object;

/// <summary>
///  Renderer for unity object values.
/// </summary>
[UTPropertyRenderer(typeof(UObject), typeof(UTUnityObjectBase<>))]
public class UTUnityObjectPropertyRenderer : UTIPropertyRenderer
{
	public void Render (UTFieldWrapper wrapper)
	{
		if (wrapper.Label != null) {
			// Tooltips for texture2D selectors are borked, just showing the label here...
			wrapper.Value = EditorGUILayout.ObjectField (wrapper.Label.text, (UObject)wrapper.Value, wrapper.AssetType, false);
		} else {
			wrapper.Value = EditorGUILayout.ObjectField ((UObject)wrapper.Value, wrapper.AssetType, false);
		}
	}
}

