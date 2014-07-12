using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Remove Component From Game Object", description="Removes a component from a game object.")]
[UTDefaultAction]
public class UTRemoveComponentFromGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object that should be modified.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="The component that should be removed.")]
	[UTInspectorHint(required=true, order=1, baseType=typeof(Component))]
	public UTType component;
	[UTDoc(description="Should all matching components be removed? If false, only the first matching component will be removed.")]
	[UTInspectorHint(order=2)]
	public UTBool removeAll;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object.", this);
		}
		
		UTTypeInfo theComponentInfo = component.EvaluateIn (context);
		if (theComponentInfo == null) {
			throw new UTFailBuildException ("You need to specify a component that should be removed.", this);			
		}
		
		Type theComponent = theComponentInfo.Type;
		if (theComponent == null) {
			throw new UTFailBuildException("There is no component of type " + theComponentInfo.TypeName + " in the current project. Did you delete it accidently?", this );
		}
		
		var doRemoveAll = removeAll.EvaluateIn (context);
		
		
		Component[] toRemove;
		if (doRemoveAll) {
			toRemove = theGameObject.GetComponents (theComponent);
		} else {
			toRemove = new Component[] {theGameObject.GetComponent (theComponent)};
		}
		
		foreach (var comp in toRemove) {
			if (comp != null) {
				if (UTPreferences.DebugMode) {
					Debug.Log ("Removing component " + comp.name + ".", this);
				}
				UObject.DestroyImmediate (comp);
			}
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Remove Component From Game Object",  false, 570)]
	public static void AddAction ()
	{
		var result = Create<UTRemoveComponentFromGameObjectAction> ();
		result.removeAll = new UTBool ();
		result.removeAll.Value = true;
		result.removeAll.UseExpression = false;
	}
	
}

