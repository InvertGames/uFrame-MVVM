using UnityEngine;
using System.Collections;
using UnityEditor;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Set Game Object Active Status", description="Changes the game object's active status.")]
[UTDefaultAction]
public class UTChangeActiveStatusOfGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object for which the status should be changed.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="Should the game object be active?")]
	[UTInspectorHint(order=1)]
	public UTBool active;
	
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object for which the status should be changed.", this);
		}
		
#if UNITY_3_5
		theGameObject.SetActiveRecursively(active.EvaluateIn(context));
#else
		theGameObject.SetActive(active.EvaluateIn (context));
#endif		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Set Game Object Active Status",  false, 590)]
	public static void AddAction ()
	{
	 	Create<UTChangeActiveStatusOfGameObjectAction> ();
	}


}
