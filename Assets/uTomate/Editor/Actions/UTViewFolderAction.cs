//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections;

[UTActionInfo(actionCategory = "Files & Folders", sinceUTomateVersion="1.2.0")]
[UTDoc(title="View Folder ", description="Open a folder using Finder/Explorer")]
[UTDefaultAction]
/// <summary>
/// Action that opens a folder. This action is based on code contributed by Jean Fabre.
/// </summary>
public class UTViewFolderAction : UTAction
{
	[UTDoc(description="Folder To View")]
	[UTInspectorHint(displayAs=UTInspectorHint.DisplayAs.FolderSelect, caption="Select Folder", required=true)]
	public UTString folder;
	
	public override IEnumerator Execute (UTContext context)
	{
		string realFolder = folder.EvaluateIn (context);
		
		try 
		{
			System.Diagnostics.Process.Start(new FileInfo(realFolder).Directory.FullName);
		}
		catch(Exception e) 
		{
			throw new UTFailBuildException("Opening folder failed. " + e.Message,this);
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Files + Folders/View Folder", false, 250)]
	public static void AddAction ()
	{
		Create<UTViewFolderAction> ();
	}
	
}

