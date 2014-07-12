//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEditor;
using System.Collections;

[UTActionInfo(actionCategory = "Scene Manipulation")]
[UTDoc(title="Save Scene", description="Saves the current scene.")]
[UTDefaultAction]
public class UTSaveSceneAction : UTAction
{
	
	[UTDoc(description="Save a copy of the scene?")]
	[UTInspectorHint(order=0)]
	public UTBool saveCopy;
	[UTDoc(description="File name where to save the copy. This is ignored when 'Save Copy' is false.")]
	[UTInspectorHint(order=1, displayAs=UTInspectorHint.DisplayAs.SaveFileSelect, caption="Select file name for copy.", extension="unity", required=true)]
	public UTString filename;
	
	public override IEnumerator Execute (UTContext context)
	{
		var isSaveCopy = saveCopy.EvaluateIn (context);
		var theFileName = "";
		if (isSaveCopy) {
			theFileName = filename.EvaluateIn (context);
			if (string.IsNullOrEmpty (theFileName)) {
				throw new UTFailBuildException ("You need to specify a file name when saving a copy of a scene.", this);
			}
			theFileName = UTFileUtils.FullPathToProjectPath (theFileName);

			UTFileUtils.EnsureParentFolderExists (UTFileUtils.CombineToPath (UTFileUtils.ProjectRoot, theFileName));
		}
		if (! EditorApplication.SaveScene (theFileName, isSaveCopy)) {
			throw new UTFailBuildException ("Saving the scene failed.", this);
		}
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Save Scene", false, 290)]
	public static void AddAction ()
	{
		Create<UTSaveSceneAction> ();
	}

}

