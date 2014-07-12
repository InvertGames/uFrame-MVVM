using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Select Game Object", description="Selects a game object in the currently open scene.")]
[UTDefaultAction]
public class UTSelectGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object that should be selected.")]
	[UTInspectorHint(required=true)]
	public UTGameObject gameObject;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn(context);
		if (theGameObject == null) {
			throw new UTFailBuildException("Game object is required", this);
		}
		Selection.activeObject = theGameObject;
		
		yield return "";
		
	}
	
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Select Game Object", false, 400)]
	public static void AddAction ()
	{
		Create<UTSelectGameObjectAction> ();
	}
	
	
}
