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
using System.Collections;
using System.IO;

[UTActionInfo(actionCategory = "Files & Folders")]
[UTDoc(title="Delete files & folders", description="Deletes files and folders. No undo. It is recommend that you perform a dry run with this action to avoid deleting stuff that you need.")]
[UTDefaultAction]
public class UTDeleteFilesAction : UTAction
{
	[UTDoc(description="Do you want to delete a single file/folder or a set of files/folders?")]
	[UTInspectorHint(order=0)]
	public UTDeletionType deletionType;
	[UTDoc(description="File or folder to be deleted.")]
	[UTInspectorHint(order=1, required=true)]
	public UTString fileOrFolder;
	[UTDoc(title="Base Folder", description="The base folder where to search for files and directories to be deleted. If empty will use the project directory.")]
	[UTInspectorHint(displayAs=UTInspectorHint.DisplayAs.FolderSelect, order=2, caption="Select base folder for delete operation.")]
	public UTString baseDirectory;
	[UTDoc(description="The files and folders to delete.")]
	[UTInspectorHint(order=3)]
	public UTString[] includes;
	[UTDoc(description="The files and folders not to delete.")]
	[UTInspectorHint(order=4)]
	public UTString[] excludes;
	[UTDoc(description="When this is true, the files will not be deleted, just printed to the console.")]
	[UTInspectorHint(order=5)]
	public UTBool dryRun;
	
	public override IEnumerator Execute (UTContext context)
	{
		DeletionType theDeletionType = deletionType.EvaluateIn (context);
		var doDryRun = dryRun.EvaluateIn (context);
		
		if (theDeletionType == DeletionType.SingleFileOrFolder) {
			var theSingleFileOrFolder = fileOrFolder.EvaluateIn (context);
			if (string.IsNullOrEmpty (theSingleFileOrFolder)) {
				throw new UTFailBuildException ("You need to specify the name of the file or folder to be deleted.", this);
			}
			if (doDryRun) {
				Debug.Log ("[Dry Run] Would delete " + theSingleFileOrFolder);
			} else {
				FileAttributes attributes;
				try {
					attributes = File.GetAttributes (theSingleFileOrFolder);
				}
				catch(DirectoryNotFoundException) {
					if (UTPreferences.DebugMode) {
						Debug.Log("File " + theSingleFileOrFolder + " already deleted.");
					}
					yield break;
				}
				if ((attributes & FileAttributes.Directory) == FileAttributes.Directory) {
					if (UTPreferences.DebugMode) {
						Debug.Log ("Deleting directory " + theSingleFileOrFolder);
					}
					Directory.Delete (theSingleFileOrFolder, true);
				} else {
					if (UTPreferences.DebugMode) {
						Debug.Log ("Deleting file " + theSingleFileOrFolder);
					}
					File.Delete (theSingleFileOrFolder);
				}
			}
		} else {
		
			var theBaseDirectory = baseDirectory.EvaluateIn (context);
			if (string.IsNullOrEmpty (theBaseDirectory)) {
				theBaseDirectory = UTFileUtils.ProjectRoot;
			}
			if (!Directory.Exists (theBaseDirectory)) {
				Debug.Log ("Base directory " + theBaseDirectory + " does not exist. Skipping delete action.");
			} else {
				theBaseDirectory = UTFileUtils.NormalizeSlashes (theBaseDirectory);
		
				var theIncludes = EvaluateAll (includes, context);
				var theExcludes = EvaluateAll (excludes, context);
		
				var theStuff = UTFileUtils.CalculateFileset (theBaseDirectory, theIncludes, theExcludes, UTFileUtils.FileSelectionMode.FilesAndFolders);
		
		
				Debug.Log ("Deleting " + theStuff.Length + " files & directories.");
		
				for (int i = 0; i< theStuff.Length; i++) {
					context.LocalProgress = ((float)i) / ((float)theStuff.Length);
			
					var stuff = theStuff [i];
					var exists = File.Exists (stuff) || Directory.Exists (stuff);
					if (!exists) {
						Debug.LogWarning (stuff + " does no longer exist. Check if your fileset can be simplified.");
						// maybe was a subfolder of a directory that got deleted before.
						continue;
					}
					var attributes = File.GetAttributes (stuff);
					if ((attributes & FileAttributes.Directory) == FileAttributes.Directory) {
						if (doDryRun) {
							Debug.Log ("[Dry Run] Would delete directory " + stuff);
						} else {
							if (UTPreferences.DebugMode) {
								Debug.Log ("Deleting directory " + stuff);
							}
							Directory.Delete (stuff, true);
						}
					} else {
						if (doDryRun) {
							Debug.Log ("[Dry Run] Would delete file" + stuff);
						} else {
							if (UTPreferences.DebugMode) {
								Debug.Log ("Deleting file " + stuff);
							}
							File.Delete (stuff);
						}
					}
					yield return "";
				}
			
			}
		}
	}

	[MenuItem("Assets/Create/uTomate/Files + Folders/Delete Files", false, 260)]
	public static void AddAction ()
	{
		Create<UTDeleteFilesAction> ();
	}
	
	
	public enum DeletionType
	{
		FileSet,
		SingleFileOrFolder
	}
}

