using UnityEngine;
using System.Collections;
using UnityEditor;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Rename Game Object", description="Renames a game object.")]
[UTDefaultAction]
public class UTRenameGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object that should be renamed.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="The new name of the game object")]
	[UTInspectorHint(order=1, required=true)]
	public UTString newName;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object to rename.", this);
		}
		
		var theName = newName.EvaluateIn(context);
		if (string.IsNullOrEmpty(theName)) {
			throw new UTFailBuildException("You need to specify a new name for the game object.", this);
		}
		
		theGameObject.name = theName;
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Rename Game Object",  false, 550)]
	public static void AddAction ()
	{
	 	Create<UTRenameGameObjectAction> ();
	}


}
