//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UTBuildAssetBundleAction))]
public class UTBuildAssetBundleActionEditor : UTInspectorBase
{
	
	public override UTVisibilityDecision IsVisible (System.Reflection.FieldInfo fieldInfo)
	{
		var theAction = (UTBuildAssetBundleAction)target;
		switch (fieldInfo.Name) {
		case "mainAsset":
		case "collectDependencies":
		case "completeAssets":
		case "disableWriteTypeTree":
		case "deterministicAssetBundle":
			if (theAction.bundleType.UseExpression || theAction.bundleType.Value == UTBuildAssetBundleAction.UTTypeOfBundle.SimpleAssetBundle) {
				return UTVisibilityDecision.Visible;
			} else {
				return UTVisibilityDecision.Invisible;
			}
		case "useScenesFromBuildSettings":
			if (theAction.bundleType.UseExpression ||  theAction.bundleType.Value == UTBuildAssetBundleAction.UTTypeOfBundle.StreamedScenes) {
				return UTVisibilityDecision.Visible;
			}
			else {
				return UTVisibilityDecision.Invisible;
			}
		case "includes":
		case "excludes":	
			// includes/excludes fields are visible for simple asset bundles and for streamed scene bundles if useScenesFromSettings is false.
			if(theAction.bundleType.UseExpression || theAction.bundleType.Value == UTBuildAssetBundleAction.UTTypeOfBundle.SimpleAssetBundle ||
				theAction.useScenesFromBuildSettings.UseExpression || theAction.useScenesFromBuildSettings.Value == false) {
				return UTVisibilityDecision.Visible;
			}
			else {
				return UTVisibilityDecision.Invisible;
			}
		}
		
		return base.IsVisible (fieldInfo);
	}
}


