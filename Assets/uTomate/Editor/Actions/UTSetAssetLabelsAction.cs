//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using System.Collections;
using UnityEditor;
using UObject = UnityEngine.Object;
using System.Collections.Generic;

[UTDoc(title="Set Asset Labels", description="Sets the asset labels of the specified assets.")]
[UTInspectorGroups(groups=new string[]{"Labels", "Assets"})]
[UTActionInfo(actionCategory = "General", sinceUTomateVersion = "1.2.0")]
[UTDefaultAction]
public class UTSetAssetLabelsAction : UTAction
{
	
	[UTDoc(description="The labels to be set. Put one label per entry. If you want to clear the asset labels, simply set no labels here.")]
	[UTInspectorHint(group="Labels", order=10)]
	public UTString[] labels;
	[UTDoc(description="If ticked, the labels of the assets will be amended with the ones specified in this action. Otherwise the labels of the asset will be replaced by the labels specified in this action.")]
	[UTInspectorHint(group="Labels", order=20)]
	public UTBool amend;
	//
	[UTDoc(description="The assets that should be included.")]
	[UTInspectorHint(group="Assets", order=10)]
	public UTString[] includes;
	[UTDoc(description="The assets that should be excluded.")]
	[UTInspectorHint(group="Assets", order=20)]
	public UTString[]excludes;
	
	public override IEnumerator Execute (UTContext context)
	{
		var files = UTFileUtils.CalculateFileset (EvaluateAll (includes, context), EvaluateAll (excludes, context));
		UTFileUtils.FullPathToProjectPath (files); // repath them to be relative to project root
		
		var doAmend = amend.EvaluateIn (context);
		var theLabels = EvaluateAll (labels, context);
		
		Debug.Log("The labels: " + string.Join(", " , theLabels));
		
		if (doAmend && theLabels.Length == 0) {
			Debug.LogWarning("Amend is set to true but no labels are specified. This will effectively change nothing.");
			yield break;
		}
		
		Debug.Log ("Updating the labels of " + files.Length + " assets.");
		
		foreach (var file in files) {
			if (file.EndsWith (".meta")) {
				Debug.LogWarning ("File set contains a meta file: " + file + ". Please exclude meta files from the fileset.", this);
				continue;
			}
			var asset = AssetDatabase.LoadAssetAtPath (file, typeof(UObject));
			string[] finalLabels = theLabels;
			
			if (doAmend) {
				var currentLabels = AssetDatabase.GetLabels (asset);
				var knownLabels = new HashSet<string> (currentLabels);
				foreach (var aLabel in theLabels) {
					if (!knownLabels.Contains (aLabel)) {
						knownLabels.Add (aLabel);
					}
				}
				finalLabels = new string[knownLabels.Count];
				knownLabels.CopyTo (finalLabels);
			}	
			if (UTPreferences.DebugMode) {
				Debug.Log ("Setting labels of " + file + " to [" + string.Join (", ", finalLabels) + "]", this);
			}
			
			if (finalLabels.Length == 0) {
				AssetDatabase.ClearLabels(asset);
			}
			else {
				AssetDatabase.SetLabels (asset, finalLabels);
			}
			EditorUtility.SetDirty(asset);
			yield return "";
		}
		
		AssetDatabase.SaveAssets(); // save asset changes.
		Debug.Log ("Assets successfully updated.");
		
	}

	[MenuItem("Assets/Create/uTomate/General/Set Asset Labels",  false, 370)]
	public static void AddAction ()
	{
		var action = Create<UTSetAssetLabelsAction> ();
		action.excludes = new UTString[1];
		action.excludes[0] = new UTString();
		// exclude meta files by default
		action.excludes [0].Value = "**/*.meta";
		action.excludes [0].UseExpression = false;
	}
	
}
