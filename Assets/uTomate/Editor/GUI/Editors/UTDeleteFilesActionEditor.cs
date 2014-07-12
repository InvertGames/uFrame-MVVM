//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UTDeleteFilesAction))]
public class UTDeleteFilesActionEditor : UTInspectorBase
{
	public override UTVisibilityDecision IsVisible (System.Reflection.FieldInfo fieldInfo)
	{
		var self = (UTDeleteFilesAction)target;
		switch (fieldInfo.Name) {
		case "baseDirectory":
		case "includes":
		case "excludes":
			if (self.deletionType.UseExpression || self.deletionType.Value == UTDeleteFilesAction.DeletionType.FileSet) {
				return UTVisibilityDecision.Visible;
			} else {
				return UTVisibilityDecision.Invisible;
			}
		case "fileOrFolder":
			if (self.deletionType.UseExpression || self.deletionType.Value == UTDeleteFilesAction.DeletionType.SingleFileOrFolder) {
				return UTVisibilityDecision.Visible;
			} else {
				return UTVisibilityDecision.Invisible;
			}
		}
		
		return base.IsVisible (fieldInfo);
	}
}


