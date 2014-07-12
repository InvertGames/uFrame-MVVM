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

[UTActionInfo(actionCategory = "Files & Folders")]
[UTDoc(title="Make Folders", description="Creates the given folders.")]
[UTDefaultAction]
public class UTMkDirAction : UTAction
{
	[UTDoc(description="The folders that should be created.")]
	public UTString[] folders;
	
	public override IEnumerator Execute (UTContext context)
	{
		string [] realFolders = EvaluateAll(folders, context);
		
		foreach( var folder in realFolders) {
			Debug.Log("Creating folder " + folder, this);
			try {
				Directory.CreateDirectory(folder);	
			}
			catch(Exception e) {
				throw new UTFailBuildException("Creating folder failed. " + e.Message,this);
			}
			yield return "";
		}
	}
	
	[MenuItem("Assets/Create/uTomate/Files + Folders/Make Folders", false, 240)]
	public static void AddAction ()
	{
		Create<UTMkDirAction> ();
	}
	
}

