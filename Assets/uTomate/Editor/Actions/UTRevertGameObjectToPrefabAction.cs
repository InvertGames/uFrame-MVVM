using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using UnityEditorInternal;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Revert Game Object to Prefab", description="Reverts a game object to it's prefab.")]
[UTDefaultAction]
public class UTRevertGameObjectToPrefabAction : UTAction
{
	
	[UTDoc(description="The game object that should be reverted.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object to revert.", this);
		}
		
		PrefabUtility.RevertPrefabInstance(theGameObject);
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Revert Game Object to Prefab",  false, 510)]
	public static void AddAction ()
	{
		Create<UTRevertGameObjectToPrefabAction> ();
	}


}
