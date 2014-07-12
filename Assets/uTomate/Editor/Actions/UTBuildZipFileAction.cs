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
using Ionic.Zip;

[UTActionInfo(actionCategory = "Import & Export")]
[UTDoc(title="Build ZIP file", description="Build a ZIP file.")]
[UTDefaultAction]
public class UTBuildZipFileAction : UTAction
{
	
	[UTDoc(title="Source Folder", description="Source folder where the files should be collected from. If empty files will be collected from the project's assets.")]
	[UTInspectorHint(order=0, required=false, displayAs=UTInspectorHint.DisplayAs.FolderSelect, caption="Select source folder for zipping")]
	public UTString baseDirectory;
	[UTDoc(description="Files to include")]
	[UTInspectorHint(order=1)]
	public UTString[] includes;
	[UTDoc(description="Files to exclude")]
	[UTInspectorHint(order=2)]
	public UTString[] excludes;
	[UTDoc(description="Filename where to put the ZIP file. If it's relative it will be put in the assets folder of the current package.")]
	[UTInspectorHint(order=3, displayAs=UTInspectorHint.DisplayAs.SaveFileSelect, caption="Select output file.", required=true)]
	public UTString outputFileName;
	[UTDoc(title="Base folder in ZIP file", description="Base folder in the ZIP-File. Leave empty if the folder structure should be copied verbatim from the source files.")]
	[UTInspectorHint(order=4)]
	public UTString baseFolderInZIPFile;
	[UTDoc(description="Should the folder structure be flattened (e.g. all folders be removed from the ZIP file)?")]
	[UTInspectorHint(order=5)]
	public UTBool flattenStructure;
	[UTDoc(description="If ticked, this action will append the files to an existing ZIP file. Otherwise an existing ZIP file will be deleted by this action.")]
	[UTInspectorHint(order=6)]
	public UTBool appendToExistingFile;

	public override IEnumerator Execute (UTContext context)
	{
		var theBaseDirectory = baseDirectory.EvaluateIn (context);
		if (string.IsNullOrEmpty (theBaseDirectory)) {
			theBaseDirectory = UTFileUtils.ProjectAssets;
		}
		
		var theOutputFileName = outputFileName.EvaluateIn (context);
		
		if (string.IsNullOrEmpty (theOutputFileName)) {
			throw new UTFailBuildException ("Output file name must be set", this);
		}
		
		
		if (!theOutputFileName.EndsWith (".zip")) {
			Debug.LogWarning ("Output filename should end with .zip.", this);
		}

		if (!appendToExistingFile.EvaluateIn(context)) {
			if (File.Exists(theOutputFileName)) {
				if (UTPreferences.DebugMode) {
					Debug.Log ("Deleting existing ZIP file: " + theOutputFileName);
				}
				File.Delete (theOutputFileName);
			}
		}

		var theIncludes = EvaluateAll (includes, context); 
		var theExcludes = EvaluateAll (excludes, context);
		
		var fileList = UTFileUtils.CalculateFileset (theBaseDirectory, theIncludes, theExcludes, UTFileUtils.FileSelectionMode.Files);

		if (fileList.Length == 0) {
			throw new UTFailBuildException ("There is nothing to ZIP.", this);
		}
		
		Debug.Log ("Zipping " + fileList.Length + " files.", this);
		
		UTFileUtils.EnsureParentFolderExists (theOutputFileName);

		var doFlatten = flattenStructure.EvaluateIn(context);
		var theBaseFolderInZipFile = baseFolderInZIPFile.EvaluateIn(context);
		using (ZipFile zf = new ZipFile(theOutputFileName)) {
			foreach (var file in fileList) {
				if (UTPreferences.DebugMode) {
					Debug.Log ("Zipping: " + file, this);
				}			
				if (doFlatten) {
					zf.AddFile(file, theBaseFolderInZipFile);
				}
				else {
					var relativePath = UTFileUtils.StripBasePath(UTFileUtils.GetParentPath(file), theBaseDirectory);
					zf.AddFile(file, UTFileUtils.CombineToPath(theBaseFolderInZipFile, relativePath));
				}
				yield return "";	
			}
			zf.Save();
		}
		
		Debug.Log ("ZIP file created at " + theOutputFileName, this);
		yield return "";
	}

	[MenuItem("Assets/Create/uTomate/Import + Export/Build ZIP File", false, 300)]
	public static void AddAction ()
	{
		Create<UTBuildZipFileAction> ();
	}
}

