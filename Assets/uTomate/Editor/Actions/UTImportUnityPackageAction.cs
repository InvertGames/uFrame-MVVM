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

[UTActionInfo(actionCategory = "Import & Export")]
[UTDoc(title="Import Unity Package", description="Imports a unity package.")]
[UTDefaultAction]
public class UTImportUnityPackageAction : UTAction
{
	
	[UTDoc(description="Path to the package to import")]
	[UTInspectorHint(required=true, displayAs=UTInspectorHint.DisplayAs.OpenFileSelect, order=1, caption="Select package to import.")]
	public UTString packageToImport;
	
	[UTDoc(description="Should a dialog be opened for importing?")]
	[UTInspectorHint(required=false, order=2)]
	public UTBool interactive;

	public override IEnumerator Execute (UTContext context)
	{
		var thePackageToImport = packageToImport.EvaluateIn (context);
		
		if (string.IsNullOrEmpty (thePackageToImport)) {
			throw new UTFailBuildException ("Package to import must be set", this);
		}
		
		FileInfo info = new FileInfo (thePackageToImport);
		if (!info.Exists) {
			// try to resolve relative to the project root
			info = new FileInfo (Application.dataPath + "\\" + thePackageToImport);
			if (!info.Exists) {
				throw new UTFailBuildException ("Unable to locate input package " + thePackageToImport, this);
			}
		}
		var doInteractive = interactive.EvaluateIn (context);
		
		AssetDatabase.ImportPackage (thePackageToImport, doInteractive);
		Debug.Log ("Package imported from " + thePackageToImport, this);
		yield return "";
	}

	[MenuItem("Assets/Create/uTomate/Import + Export/Import Unity Package", false, 270)]
	public static void AddAction ()
	{
		Create<UTImportUnityPackageAction> ();
	}
}

