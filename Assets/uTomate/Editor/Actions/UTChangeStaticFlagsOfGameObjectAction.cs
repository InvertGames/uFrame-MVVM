using UnityEngine;
using System.Collections;
using UnityEditor;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Set Static Flags of Game Object", description="Changes the game object's static flags.")]
[UTDefaultAction]
public class UTChangeStaticFlagsOfGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object for which the static flags should be changed.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="The static flags that should be applied to the game object.")]
	[UTInspectorHint(order=1, required=true, multiSelect=true)]
	public UTStaticEditorFlags staticFlags;
	[UTDoc(description="How should the static flags be applied?")]
	[UTInspectorHint(required=true, order=2)]
	public UTSetOperationType changeType;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object for which the static flags should be changed.", this);
		}
		
		var theStaticFlags = staticFlags.EvaluateIn (context);
		
		var theCurrentStaticFlags = GameObjectUtility.GetStaticEditorFlags (theGameObject);
		switch (changeType.EvaluateIn (context)) {
		case UTSetOperation.Add:
			theStaticFlags = theStaticFlags | theCurrentStaticFlags;
			break;
		case UTSetOperation.Replace:
			break;
		case UTSetOperation.Subtract:
			theStaticFlags = theCurrentStaticFlags & ~theStaticFlags;
			break;
		default:
			throw new UTFailBuildException ("Change type is unsupported.", this); // should never happen(tm)
		}
		GameObjectUtility.SetStaticEditorFlags (theGameObject, theStaticFlags);
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Set Static Flags of Game Object",  false, 600)]
	public static void AddAction ()
	{
		Create<UTChangeStaticFlagsOfGameObjectAction> ();
	}


}
