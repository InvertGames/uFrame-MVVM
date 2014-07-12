using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Add Component To Game Object", description="Adds a component to a game object.")]
[UTDefaultAction]
public class UTAddComponentToGameObjectAction : UTAction
{
	
	[UTDoc(description="The game object that should be modified.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="The component that should be added.")]
	[UTInspectorHint(required=true, order=1, baseType=typeof(Component))]
	public UTType component;
	[UTDoc(description="Should the component only be added, when it does not yet exist on the game object?")]
	[UTInspectorHint(order=2)]
	public UTBool onlyIfNotExists;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn (context);
		if (theGameObject == null) {
			throw new UTFailBuildException ("You need to specify a game object.", this);
		}
		
		UTTypeInfo theComponentInfo = component.EvaluateIn (context);
		if (theComponentInfo == null) {
			throw new UTFailBuildException ("You need to specify a component that should be added.", this);			
		}
		
		Type theComponent = theComponentInfo.Type;
		if ( theComponent == null) {
			throw new UTFailBuildException("There is no component of type " + theComponentInfo.TypeName + " in the current project. Did you delete it accidently?", this );
		}
		
		var doOnlyIfNotExists = onlyIfNotExists.EvaluateIn (context);
		var doAdd = true;
		if (doOnlyIfNotExists) {
			 
			if (theGameObject.GetComponent (theComponent) != null) {
				if (UTPreferences.DebugMode) {
					Debug.Log ("Component of type " + theComponent.Name + " already exists at game object " + theGameObject);
				}
				doAdd = false;
			}
		}
		
		if (doAdd) {
			theGameObject.AddComponent (theComponent);
		}
		
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Add Component To Game Object",  false, 560)]
	public static void AddAction ()
	{
		var result = Create<UTAddComponentToGameObjectAction> ();
		result.onlyIfNotExists = new UTBool ();
		result.onlyIfNotExists.Value = true;
		result.onlyIfNotExists.UseExpression = false;
	}
	
}

