using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using UnityEditorInternal;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Set Layer of Game Object", description="Changes the layer of a game object.")]
[UTDefaultAction]
public class UTSetLayerOfGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object that should be tagged.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="The layer of the game object.")]
	[UTInspectorHint(order=1)]
	public UTLayer layer;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object to tag.", this);
		}
		
		var theLayer = layer.EvaluateIn (context);
		theGameObject.layer = theLayer;
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Set Layer of Game Object",  false, 580)]
	public static void AddAction ()
	{
		Create<UTSetLayerOfGameObjectAction> ();
	}


}
