//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UTCopyFilesAction))]
public class UTCopyFilesActionEditor : UTInspectorBase
{
	
	public override UTVisibilityDecision IsVisible (System.Reflection.FieldInfo fieldInfo)
	{
		if (fieldInfo.Name == "onlyIfNewer") {
			var self = (UTCopyFilesAction)target;
			return (self.overwriteExisting.UseExpression || self.overwriteExisting.Value) ? 
				UTVisibilityDecision.Visible : UTVisibilityDecision.Invisible;
		}
		return base.IsVisible (fieldInfo);
	}
}


