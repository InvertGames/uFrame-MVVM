using UnityEngine;
using System.Collections;
using UnityEditor;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Instantiate Prefab", description="Instantiates a prefab in the currently open scene.")]
[UTDefaultAction]
public class UTInstantiatePrefabAction : UTAction
{
	
	[UTDoc(description="The prefab that should be instanciated.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject prefab;
	
	[UTDoc(description="The name of the instanciated object. Leave empty to use the default name.")]
	[UTInspectorHint(order=1)]
	public UTString objectName;
	
	[UTDoc(description="Name of the property into which the instanciated object should be stored. Leave empty if you don't need the object for further actions.")]
	public UTString outputProperty;
	
	public override IEnumerator Execute (UTContext context)
	{
		var thePrefab = prefab.EvaluateIn (context);
		if (thePrefab == null) {
			throw new UTFailBuildException ("You need to specify a prefab to instantiate.", this);
		}
		
		var theObjectName = objectName.EvaluateIn(context);
		if (UTPreferences.DebugMode) {
			Debug.Log ("Instantiating prefab: " + thePrefab);
		}
		
		var theObject = (GameObject) PrefabUtility.InstantiatePrefab(thePrefab);
		
		if (!string.IsNullOrEmpty(theObjectName)) {
			theObject.name = theObjectName;
		}
		
		
		var theProperty = outputProperty.EvaluateIn (context);
		if (!string.IsNullOrEmpty (theProperty)) {
			context [theProperty] = theObject;
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Instantiate Prefab",  false, 500)]
	public static void AddAction ()
	{
		Create<UTInstantiatePrefabAction> ();
	}


}
