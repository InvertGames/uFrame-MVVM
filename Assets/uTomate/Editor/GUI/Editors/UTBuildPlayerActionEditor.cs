//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;

[CustomEditor(typeof(UTBuildPlayerAction))]
public class UTBuildPlayerActionEditor : UTInspectorBase
{
	
	public override UTVisibilityDecision IsVisible (System.Reflection.FieldInfo field)
	{
		var action = (UTBuildPlayerAction)target;
		if (field.Name == "offlineDeployment" || field.Name == "deployOnline" ) {
			var platform =  action.targetPlatform;
			return (platform.UseExpression || platform.Value == BuildTarget.WebPlayer || platform.Value == BuildTarget.WebPlayerStreamed) ?
				UTVisibilityDecision.Visible : UTVisibilityDecision.Invisible;
		}
		
		if (field.Name == "symlinkLibraries" ) {
			var platform =  action.targetPlatform;
			return (platform.UseExpression || platform.Value == BuildTarget.iPhone) ? 
				UTVisibilityDecision.Visible : UTVisibilityDecision.Invisible;
		}
		
		if (field.Name == "includes" || field.Name == "excludes" ) {
			return (action.useScenesFromBuildSettings.UseExpression || !action.useScenesFromBuildSettings.Value) ? 
				UTVisibilityDecision.Visible : UTVisibilityDecision.Invisible;
		}
		
		if (field.Name == "addPlatformExtension") {
			var platform =  action.targetPlatform;
			return (platform.UseExpression || 
				platform.Value == BuildTarget.StandaloneWindows ||
				platform.Value == BuildTarget.StandaloneWindows64 ||
				platform.Value == BuildTarget.StandaloneOSXIntel ||
#if !(UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 )
			        // 4.1+
				platform.Value == BuildTarget.StandaloneOSXUniversal ||
#endif
				platform.Value == BuildTarget.Android
				) ? UTVisibilityDecision.Visible : UTVisibilityDecision.Invisible;
		}
		
		return base.IsVisible(field);
	}
	
}


