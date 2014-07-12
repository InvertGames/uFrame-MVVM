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
[UTDoc(title="Unzip ZIP file", description="Unzip a ZIP file.")]
[UTDefaultAction]
public class UTUnzipFileAction : UTAction
{
	
	[UTDoc(description="Filename of the ZIP file to unzip.")]
	[UTInspectorHint(order=1, displayAs=UTInspectorHint.DisplayAs.OpenFileSelect, caption="Select input file.", required=true)]
	public UTString inputFileName;
	[UTDoc(description="Folder to which the file should be unzipped.")]
	[UTInspectorHint(order=2,  displayAs=UTInspectorHint.DisplayAs.FolderSelect, caption="Select output folder.", required=true)]
	public UTString outputFolder;
	[UTDoc(description="Should existing files be overwritten?")]
	[UTInspectorHint(order=3)]
	public UTBool overwriteExistingFiles;

	public override IEnumerator Execute (UTContext context)
	{
		var theInputFileName = inputFileName.EvaluateIn (context);
		if (!File.Exists (theInputFileName)) {
			throw new UTFailBuildException ("The input file " + theInputFileName + " does not exist.", this);
		}
		
		var theOutputFolder = outputFolder.EvaluateIn (context);
		
		if (string.IsNullOrEmpty (theOutputFolder)) {
			throw new UTFailBuildException ("You need to specify an output folder.", this);
		}
		
		if (File.Exists (theOutputFolder)) {
			throw new UTFailBuildException ("The output folder " + theOutputFolder + " is a file.", this);
		}
		
		if (!Directory.Exists (theOutputFolder)) {
			Directory.CreateDirectory (theOutputFolder);
		}
		
		var doOverwrite = overwriteExistingFiles.EvaluateIn (context);
		using (ZipFile zip = ZipFile.Read(theInputFileName)) {
			foreach (ZipEntry e in zip) {
				if (UTPreferences.DebugMode) {
					Debug.Log ("Extracting " + e.FileName, this);
				}
				e.Extract( theOutputFolder, doOverwrite ? ExtractExistingFileAction.OverwriteSilently : ExtractExistingFileAction.DoNotOverwrite);
				yield return "";
			}
		}
		
		Debug.Log ("ZIP at " + theInputFileName + " extracted to " + theOutputFolder, this);
		yield return "";
	}

	[MenuItem("Assets/Create/uTomate/Import + Export/Unzip ZIP File", false, 290)]
	public static void AddAction ()
	{
		Create<UTUnzipFileAction> ();
	}
}

