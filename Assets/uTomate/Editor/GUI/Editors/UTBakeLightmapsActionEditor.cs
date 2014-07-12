//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UTBakeLightmapsAction))]
public class UTBakeLightmapsActionEditor : UTInspectorBase {
	public override UTVisibilityDecision IsVisible (System.Reflection.FieldInfo fieldInfo)
	{
		var action = target as UTBakeLightmapsAction;
		var settingsVisible = action.useSettingsFromScene.UseExpression || !action.useSettingsFromScene.Value;
		switch(fieldInfo.Name) {
		case "whatToBake":
		case "useSettingsFromScene":
			return UTVisibilityDecision.Visible;
		default: // all other fields are settings
			return settingsVisible ? UTVisibilityDecision.Visible : UTVisibilityDecision.Invisible;
		}
	}
}


