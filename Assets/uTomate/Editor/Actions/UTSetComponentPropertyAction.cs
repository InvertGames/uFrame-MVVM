using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

[UTActionInfo(actionCategory="Scene Manipulation", sinceUTomateVersion="1.3.0")]
[UTDoc(title="Set Component Property", description="Sets a property of a component of a game object.")]
[UTDefaultAction]
public class UTSetComponentPropertyAction : UTAction
{
	
	[UTDoc(description="The game object that holds the component.")]
	[UTInspectorHint(required=true, order=0)]
	public UTGameObject gameObject;
	[UTDoc(description="The property that should be set.")]
	[UTInspectorHint(required=true, order=1, baseType=typeof(Component))]
	public UTMember property;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTString stringPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTBool boolPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTInt intPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTFloat floatPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTTexture texturePropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTVector3 vector3PropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTVector2 vector2PropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTRect rectPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTGameObject gameObjectPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTUnityObject unityObjectPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTQuaternion quaternionPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTMaterial materialPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTColor colorPropertyValue;
	[UTDoc(title="Property Value", description="The new value of the component property.")]
	[UTInspectorHint(order=2)]
	public UTObject objectPropertyValue;
	
	public override IEnumerator Execute (UTContext context)
	{
		var theGameObject = gameObject.EvaluateIn(context);
		if (theGameObject == null) {
			throw new UTFailBuildException("You must specify the game object htat holds the component.", this);
		}
		
		var theProperty = property.EvaluateIn(context);
		if (theProperty == null || !theProperty.FullyDefined) {
			throw new UTFailBuildException("You must specify the component type and its property you want to change.", this);
		}
		
		var propertyPath = UTComponentScanner.FindPropertyPath(theProperty.Type, theProperty.FieldPath);
		if (propertyPath == null) {
			throw new UTFailBuildException("The component type or the property path is no longer valid.", this);
		}
		
		var theComponent = theGameObject.GetComponent(theProperty.Type);
		if (theComponent == null) {
			// nothing to do
			if (UTPreferences.DebugMode) {
				Debug.Log("Component " + theProperty.Type.Name + " not found at game object " + theGameObject, this);
			}
		} else {		
			Type propertyType = UTInternalCall.GetMemberType(propertyPath[propertyPath.Length - 1]);
			object propertyValue;
			if (typeof(string).IsAssignableFrom(propertyType)) {
				propertyValue = stringPropertyValue.EvaluateIn(context);
			} else if (typeof(bool).IsAssignableFrom(propertyType)) {
				propertyValue = boolPropertyValue.EvaluateIn(context);
			} else if (typeof(int).IsAssignableFrom(propertyType)) {
				propertyValue = intPropertyValue.EvaluateIn(context);
			} else if (typeof(float).IsAssignableFrom(propertyType)) {
				propertyValue = floatPropertyValue.EvaluateIn(context);
			} else if (typeof(Texture).IsAssignableFrom(propertyType)) {
				propertyValue = texturePropertyValue.EvaluateIn(context);
			} else if (typeof(Vector3).IsAssignableFrom(propertyType)) {
				propertyValue = vector3PropertyValue.EvaluateIn(context);
			} else if (typeof(Vector2).IsAssignableFrom(propertyType)) {
				propertyValue = vector2PropertyValue.EvaluateIn(context);
			} else if (typeof(Rect).IsAssignableFrom(propertyType)) {
				propertyValue = rectPropertyValue.EvaluateIn(context);
			} else if (typeof(Quaternion).IsAssignableFrom(propertyType)) {
				propertyValue = quaternionPropertyValue.EvaluateIn(context);
			} else if (typeof(Material).IsAssignableFrom(propertyType)) {
				propertyValue = materialPropertyValue.EvaluateIn(context);
			} else if (typeof(Color).IsAssignableFrom(propertyType)) {
				propertyValue = colorPropertyValue.EvaluateIn(context);
			} else if (typeof(GameObject).IsAssignableFrom(propertyType)) {
				propertyValue = gameObjectPropertyValue.EvaluateIn(context);
			} else if (typeof(UObject).IsAssignableFrom(propertyType)) {
				propertyValue = unityObjectPropertyValue.EvaluateIn(context);
			} else {
				propertyValue = objectPropertyValue.EvaluateIn(context);
			}

			// TODO: we need a lot more validation here. 
			// e.g. is the value assignable? 
			
			// Tested with Vector3 -> BoxCollider:center 
			// and float -> BoxCollider:center.y
			UTInternalCall.SetMemberValue(theComponent, propertyPath, propertyValue);
		}
		yield return "";
	}
	
	[MenuItem("Assets/Create/uTomate/Scene Manipulation/Set Component Property",  false, 580)]
	public static void AddAction ()
	{
		Create<UTSetComponentPropertyAction> ();
		
	}
	
}

