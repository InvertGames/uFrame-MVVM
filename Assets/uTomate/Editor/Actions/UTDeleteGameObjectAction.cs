using UnityEngine;
using System.Collections;
using UnityEditor;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Delete Game Object", description="Deletes a game object. No undo. Use the dry-run feature is strongly suggested.")]
[UTDefaultAction]
public class UTDeleteGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object that should be deleted.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="If checked, the object will not be deleted.")]
	[UTInspectorHint(order=1)]
	public UTBool dryRun;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object to delete.", this);
		}
		
		if (dryRun.EvaluateIn(context)) {
			Debug.Log("Would delete game object " + theGameObject.name, theGameObject);
		}
		else {
			if (UTPreferences.DebugMode) {
				Debug.Log("Deleting game object " + theGameObject.name);
			}
			GameObject.DestroyImmediate(theGameObject);
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Delete Game Object",  false, 540)]
	public static void AddAction ()
	{
	 	Create<UTDeleteGameObjectAction> ();
	}


}
