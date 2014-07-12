using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using UnityEditorInternal;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Tag Game Object", description="Tags a game object.")]
[UTDefaultAction]
public class UTTagGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object that should be tagged.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="The tag of the game object.")]
	[UTInspectorHint(order=1)]
	public UTTag tagName;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object to tag.", this);
		}
		
		var theTagName = tagName.EvaluateIn(context);
		if (string.IsNullOrEmpty(theTagName)) {
			theGameObject.tag = string.Empty;
		}
		else {
			if (Array.IndexOf(InternalEditorUtility.tags, theTagName ) == -1) {
				throw new UTFailBuildException("There is currently no tag '" + theTagName +
					"' defined. Please define it in the tag manager and then run this action again.", this);
			}
			theGameObject.tag = theTagName;
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Tag Game Object",  false, 530)]
	public static void AddAction ()
	{
	 	Create<UTTagGameObjectAction> ();
	}


}
