//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//
using System;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

[CustomEditor(typeof(UTSetComponentPropertyAction))]
public class UTSetComponentPropertyActionEditor : UTInspectorBase
{
	public override UTVisibilityDecision IsVisible (System.Reflection.FieldInfo fieldInfo)
	{
		if (fieldInfo.Name == "gameObject" || fieldInfo.Name == "property") {
			return base.IsVisible (fieldInfo);
		}
		
		// only expression field if the type is not clear
		var self = (UTSetComponentPropertyAction) target;
		if (self.property.UseExpression || !self.property.Value.FullyDefined) {
			if (fieldInfo.Name == "objectPropertyValue") {
				return UTVisibilityDecision.Visible;
			}
			return UTVisibilityDecision.Invisible;
		}
		
		// only expression field if the property is not valid anymore
		var propertyPath = UTComponentScanner.FindPropertyPath(self.property.Value.Type, self.property.Value.FieldPath);
		if (propertyPath == null) {
			if (fieldInfo.Name == "objectPropertyValue") {
				return UTVisibilityDecision.Visible;
			}
			return UTVisibilityDecision.Invisible;
		}
		
		Type propertyType = UTInternalCall.GetMemberType(propertyPath[propertyPath.Length - 1]);
		string expectedPropertyFieldName = "objectPropertyValue";
		if (typeof(string).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "stringPropertyValue";
		} else if (typeof(bool).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "boolPropertyValue";
		} else if (typeof(int).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "intPropertyValue";
		} else if (typeof(float).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "floatPropertyValue";
		} else if (typeof(Texture).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "texturePropertyValue";
		} else if (typeof(Vector3).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "vector3PropertyValue";
		} else if (typeof(Vector2).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "vector2PropertyValue";
		} else if (typeof(Rect).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "rectPropertyValue";
		} else if (typeof(Quaternion).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "quaternionPropertyValue";
		} else if (typeof(Material).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "materialPropertyValue";
		} else if (typeof(Color).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "colorPropertyValue";
		} else if (typeof(GameObject).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "gameObjectPropertyValue";
		} else if (typeof(UObject).IsAssignableFrom(propertyType)) {
			expectedPropertyFieldName = "unityObjectPropertyValue";
		}
		
		if (fieldInfo.Name == expectedPropertyFieldName) {
			return UTVisibilityDecision.Visible;
		}
		
		return UTVisibilityDecision.Invisible;
	}
}


