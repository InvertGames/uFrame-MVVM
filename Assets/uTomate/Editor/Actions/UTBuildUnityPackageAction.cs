//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[UTActionInfo(actionCategory = "Import & Export")]
[UTDoc(title="Export Unity Package", description="Exports a unity package.")]
[UTDefaultAction]
public class UTBuildUnityPackageAction : UTAction
{
	
	[UTDoc(description="Files to include")]
	[UTInspectorHint(order=0)]
	public UTString[] includes;
	[UTDoc(description="Files to exclude")]
	[UTInspectorHint(order=1)]
	public UTString[] excludes;
	[UTDoc(description="Should dependencies of the selected files be included in the output package?")]
	[UTInspectorHint(order=2)]
	public UTBool includeDependencies;
	[UTDoc(description= "Which project settings should be included in the output package?")]
	[UTInspectorHint(order=3)]
	public UTIncludeProjectSettingsMode includeProjectSettings;
	[HideInInspector]
	public UTBool includeLibraryAssets;
	[UTDoc(description="Settings to include")]
	[UTInspectorHint(order=4)]
	public UTString[] settingsIncludes;
	[UTDoc(description="Setting to exclude")]
	[UTInspectorHint(order=5)]
	public UTString[] settingsExcludes;
	[UTDoc(description="Filename where to put the unity package. If it's relative it will be put in the assets folder of the current package.")]
	[UTInspectorHint(order=6, displayAs=UTInspectorHint.DisplayAs.SaveFileSelect, caption="Select output file.", required=true)]
	public UTString outputFileName;


	public void OnEnable() {
		if (CreatedWithActionVersion == "1.0") {
			if (includeLibraryAssets.UseExpression) {
				includeProjectSettings.UseExpression = true;
				includeProjectSettings.Expression = "(" +includeLibraryAssets.Expression + ") ? 'All' : 'None'";
				Debug.LogWarning("The 'Build Unity Package' action '" + name + "' has been migrated. Please check, if " +
					"the expression in the 'Include Project Settings' field is still correct. You can highlight the migrated " +
				                 "action by clicking on this message.", this);
			}
			else {
				includeProjectSettings.UseExpression = false;
				includeProjectSettings.Value = includeLibraryAssets.Value ? IncludeProjectSettingsMode.All : IncludeProjectSettingsMode.None;
			}
			CreatedWithActionVersion = ActionVersion;
			EditorUtility.SetDirty(this);
		} 
	}

	public override IEnumerator Execute (UTContext context)
	{
		var theOutputFileName = outputFileName.EvaluateIn (context);
		
		if (string.IsNullOrEmpty (theOutputFileName)) {
			throw new UTFailBuildException ("Output file name must be set",this);
		}
		
		if (!theOutputFileName.EndsWith (".unitypackage")) {
			Debug.LogWarning ("Output filename should end with .unitypackage.", this);
		}
		
		var theIncludes = EvaluateAll (includes, context); 
		var theExcludes = EvaluateAll (excludes, context);
		
		var fileList = UTFileUtils.CalculateFileset (theIncludes, theExcludes);
		UTFileUtils.FullPathToProjectPath(fileList);

		var theFinalList = new List<string>();
		theFinalList.AddRange(fileList);

		var doIncludeProjectSettings = includeProjectSettings.EvaluateIn(context);

		if (doIncludeProjectSettings == IncludeProjectSettingsMode.Some) {
			var theSettingsIncludes = EvaluateAll(settingsIncludes, context);
			var theSettingsExcludes = EvaluateAll(settingsExcludes, context);

			var settingsFileList = UTFileUtils.CalculateFileset(UTFileUtils.ProjectSettings, theSettingsIncludes, theSettingsExcludes, UTFileUtils.FileSelectionMode.Files);
			UTFileUtils.FullPathToProjectPath(settingsFileList);
			theFinalList.AddRange(settingsFileList);
		}

		if (theFinalList.Count == 0 && doIncludeProjectSettings != IncludeProjectSettingsMode.All) {
			throw new UTFailBuildException ("There is nothing to export.", this);
		}
		
		if (UTPreferences.DebugMode) {
			foreach (var entry in theFinalList) {
				Debug.Log ("Exporting: " + entry, this);
			}
		} 

		Debug.Log ("Exporting " + theFinalList.Count + " files.", this);
		
		ExportPackageOptions flags = ExportPackageOptions.Default;
		
		var doIncludeDependencies = includeDependencies.EvaluateIn (context);
		if (doIncludeDependencies) {
			flags = flags | ExportPackageOptions.IncludeDependencies;
		}
		
		if (doIncludeProjectSettings == IncludeProjectSettingsMode.All) {
			flags = flags | ExportPackageOptions.IncludeLibraryAssets;
		}
		
		UTFileUtils.EnsureParentFolderExists(theOutputFileName);
		AssetDatabase.ExportPackage (theFinalList.ToArray(), theOutputFileName, flags);
		Debug.Log ("Package exported to " + theOutputFileName, this);
		yield return "";
	}

	[MenuItem("Assets/Create/uTomate/Import + Export/Export Unity Package", false, 280)]
	public static void AddAction ()
	{
		Create<UTBuildUnityPackageAction> ();
	}

	public enum IncludeProjectSettingsMode {
		None,
		All,
		Some
	}
}

