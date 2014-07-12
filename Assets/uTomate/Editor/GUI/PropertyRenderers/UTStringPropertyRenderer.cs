//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

/// <summary>
///  Renderer for string values.
/// </summary>
[UTPropertyRenderer(typeof(string), typeof(UTString))]
public class UTStringPropertyRenderer : UTIPropertyRenderer
{
	public void Render (UTFieldWrapper wrapper)
	{
		var lineHeight = 20;
		
		var val = (string)wrapper.Value;
		if (val == null) {
			val = "";
		}
		
		if (wrapper.Label != null) {
			if (wrapper.InspectorHint.displayAs == UTInspectorHint.DisplayAs.TextArea) {
				EditorGUILayout.BeginVertical ();
				EditorGUILayout.LabelField (wrapper.Label);
				wrapper.Value = EditorGUILayout.TextArea (val, GUILayout.Height(wrapper.InspectorHint.lines * lineHeight));
				EditorGUILayout.EndVertical ();
			} else if (wrapper.InspectorHint.displayAs == UTInspectorHint.DisplayAs.Password) {
				wrapper.Value = EditorGUILayout.PasswordField (wrapper.Label, val);
			} else {
				wrapper.Value = EditorGUILayout.TextField (wrapper.Label, val);
			}
		} else {
			if (wrapper.InspectorHint.displayAs == UTInspectorHint.DisplayAs.TextArea) {
				wrapper.Value = EditorGUILayout.TextArea (val,GUILayout.Height(wrapper.InspectorHint.lines * lineHeight));
			} else if (wrapper.InspectorHint.displayAs == UTInspectorHint.DisplayAs.Password) {
				wrapper.Value = EditorGUILayout.PasswordField (val);
			} else {
				wrapper.Value = EditorGUILayout.TextField (val);
			}
		}
		if (wrapper.InspectorHint.displayAs == UTInspectorHint.DisplayAs.FolderSelect ||
					wrapper.InspectorHint.displayAs == UTInspectorHint.DisplayAs.SaveFileSelect ||
					wrapper.InspectorHint.displayAs == UTInspectorHint.DisplayAs.OpenFileSelect) {
			if (GUILayout.Button (UTEditorResources.FolderIcon, UTEditorResources.FolderButtonStyle)) {
				var path = !string.IsNullOrEmpty ((string)wrapper.Value) ? (string)wrapper.Value : UTFileUtils.ProjectRoot;
				var result = "";
				switch (wrapper.InspectorHint.displayAs) {
				case UTInspectorHint.DisplayAs.FolderSelect:
					result = EditorUtility.OpenFolderPanel (wrapper.InspectorHint.caption, path, "");
					break;
				case UTInspectorHint.DisplayAs.OpenFileSelect:
					result = EditorUtility.OpenFilePanel (wrapper.InspectorHint.caption, path, wrapper.InspectorHint.extension);
					break;
				case UTInspectorHint.DisplayAs.SaveFileSelect:
					result = EditorUtility.SaveFilePanel (wrapper.InspectorHint.caption, path, "", wrapper.InspectorHint.extension);
					break;
							
				}
				if (!string.IsNullOrEmpty (result)) {
					wrapper.Value = result;
				}
			}
		}	
	}
}

