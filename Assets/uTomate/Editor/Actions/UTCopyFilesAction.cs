//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;


[UTActionInfo(actionCategory = "Files & Folders")]
[UTDoc(title="Copy Files", description="Copies a set of files to a target destination.")]
[UTDefaultAction]
public class UTCopyFilesAction : UTAction
{
	[UTDoc(title="Source Folder", description="Source folder where the files should be collected from. If empty, files will be collected from the project.")]
	[UTInspectorHint(order=0, required=false, displayAs=UTInspectorHint.DisplayAs.FolderSelect, caption="Select source folder for copy")]
	public UTString baseDirectory;
	
	[UTDoc(description="Files to include.")]
	[UTInspectorHint(order=1)]
	public UTString[] includes;
	
	[UTDoc(description="Files to exclude.")]
	[UTInspectorHint(order=2)]
	public UTString[] excludes;
	
	[UTDoc(title="Target Folder", description="Folder to copy the files to.")]
	[UTInspectorHint(displayAs=UTInspectorHint.DisplayAs.FolderSelect, caption="Select target folder", required=true, order=3)]
	public UTString targetDirectory;
	
	[UTDoc(description="Overwrite files that exist in the target directory.")]
	[UTInspectorHint(order=4)]
	public UTBool overwriteExisting;
	
	[UTDoc(description="Overwrite file only, if the source file is newer than the target file? " +
		"Only relevant if overwriteExisting is true, otherwise ignored.")]
	[UTInspectorHint(order=5)]
	public UTBool onlyIfNewer;
	
	[UTDoc(description="Should the folder structure be flattened (e.g. all files copied to the target folder)?")]
	[UTInspectorHint(order=6)]
	public UTBool flattenStructure;
	
	
	public override IEnumerator Execute (UTContext context)
	{
		var theBaseDirectory = baseDirectory.EvaluateIn(context);
		if (string.IsNullOrEmpty(theBaseDirectory)) {
			theBaseDirectory = UTFileUtils.ProjectRoot;
		}

		
		if (!Directory.Exists(theBaseDirectory)) {
			throw new UTFailBuildException("The base directory " + theBaseDirectory + " does not exist.", this);
		}
		
		theBaseDirectory = UTFileUtils.NormalizeSlashes(theBaseDirectory);
		
		var theTargetDirectory = targetDirectory.EvaluateIn(context);
		if (string.IsNullOrEmpty(theTargetDirectory)) {
			throw new UTFailBuildException("You must specify a target directory.", this);
		}
		theTargetDirectory = UTFileUtils.NormalizeSlashes(theTargetDirectory);
		
		var theIncludes = EvaluateAll(includes, context);
		var theExcludes = EvaluateAll(excludes, context);
		var doFlatten = flattenStructure.EvaluateIn(context);
		
		var theFiles = UTFileUtils.CalculateFileset(theBaseDirectory, theIncludes, theExcludes, UTFileUtils.FileSelectionMode.Files);
		var theCopies = UTFileUtils.Repath(theFiles, theBaseDirectory, theTargetDirectory, doFlatten);
		
		Debug.Log("Copying " + theFiles.Length + " files to " + 
			theTargetDirectory + (doFlatten ? " and flattening " : " and preserving ") + " the directory structure.", this);
		
		var doOverwrite = overwriteExisting.EvaluateIn(context);
		var doOnlyNewer = onlyIfNewer.EvaluateIn(context);
		
		for(int i = 0; i < theFiles.Length; i++) {
			context.LocalProgress = ((float)i) / ((float) theFiles.Length);
			FileInfo src = new FileInfo(theFiles[i]);
			FileInfo target = new FileInfo(theCopies[i]);
			if (!doOverwrite && target.Exists) {
				if (UTPreferences.DebugMode) {
					Debug.Log ("File " + theCopies[i] + " exists. Not overwriting it.");
				}
				continue;
			}
			if (doOverwrite && doOnlyNewer && src.LastWriteTime.CompareTo(target.LastWriteTime) <= 0) {
				if (UTPreferences.DebugMode) {
					Debug.Log ("File " + theFiles[i] + " is not newer than " + theCopies[i] + ". Not copying it.", this);
				}
				continue;
			} 
			if (UTPreferences.DebugMode) {
				Debug.Log("Copying " + theFiles[i] + " to " + theCopies[i], this);
			}
			UTFileUtils.EnsureParentFolderExists(theCopies[i]);
			src.CopyTo(theCopies[i], doOverwrite);
			yield return "";
		}
		
	}
	
	[MenuItem("Assets/Create/uTomate/Files + Folders/Copy Files", false, 230)]
	public static void AddAction() {
		Create<UTCopyFilesAction>();
	}

}
