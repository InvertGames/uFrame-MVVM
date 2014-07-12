//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections;
using UnityEngine;

[Serializable]
[UTDoc(title="For Each File", description="Executes a set of actions for each file/folder in the current project that matches the given includes and excludes.")]
public class UTAutomationPlanForEachFileEntry : UTAutomationPlanEntryBase
{
	[UTDoc(description="The sub-tree that should be executed.")]
	[HideInInspector]
	public UTAutomationPlanEntry startOfSubtree;
	[UTDoc(description="Name of the property which holds the current file/folder.")]
	[UTInspectorHint(order=0, required=true)]
	public UTString filePropertyName;
	[UTDoc(description="Name of the property which holds the index of the current file/folder in the list. Index is zero-based. Leave empty if you don't need this.")]
	[UTInspectorHint(order=1)]
	public UTString indexPropertyName;
	[UTDoc(description="What kind of files should be selected for the file set?")]
	[UTInspectorHint(order=2)]
	public UTFileSelectionMode selectionMode;
	[UTDoc(description="The files/folders that should be included.")]
	[UTInspectorHint(order=3)]
	public UTString[] includes;
	[UTDoc(description="The files/folders that should be excluded.")]
	[UTInspectorHint(order=4)]
	public UTString[] excludes;
	
	public override string Label {
		get {
			return "For Each File";
		}
	}
	
	public override IEnumerator Execute (UTContext context)
	{
		if (startOfSubtree == null) {
			Debug.LogWarning ("No subtree specified.");
			yield break;
		}

		UTFileUtils.FileSelectionMode theMode = UTFileUtils.FileSelectionMode.Files;
		if (selectionMode != null) { // can happen when we migrate older automation plans which didn't have this setting.
			theMode = selectionMode.EvaluateIn(context);
		}

		var theFilePropertyName = filePropertyName.EvaluateIn (context);
		if (string.IsNullOrEmpty (theFilePropertyName)) {
			throw new UTFailBuildException ("You need to specify the property which holds the current file.", this);
		}
		
		var theIncludes = UTAction.EvaluateAll (includes, context);
		var theExcludes = UTAction.EvaluateAll (excludes, context);
		
		var files = UTFileUtils.CalculateFileset (theIncludes, theExcludes, theMode);
		
		var theIndexPropertyName = indexPropertyName.EvaluateIn (context);
		var indexPropertySet = !string.IsNullOrEmpty (theIndexPropertyName);
		
		var index = 0;
		foreach (var file in files) {
			context [theFilePropertyName] = file;
			if (indexPropertySet) {
				context [theIndexPropertyName] = index;
			}
			var enumerator = UTAutomationPlan.ExecutePath (startOfSubtree, context);
			do {
				yield return "";
			} while (enumerator.MoveNext());
			index++;
		}
	}
}

