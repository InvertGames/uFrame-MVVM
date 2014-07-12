// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[UTPropertyRenderer(typeof(UTMember), typeof(UTMemberInfo))]
public class UTMemberPropertyRenderer : UTIPropertyRenderer
{
		
	public void Render (UTFieldWrapper fieldWrapper)
	{
		Type baseType = fieldWrapper.InspectorHint.baseType;
		var compatibleTypes = UTComponentScanner.FindCompatibleTypes (baseType); 	
		
		var val = (UTMemberInfo)fieldWrapper.Value;
		int currentIndex = -1;
		if (val != null) {
			currentIndex = Array.IndexOf (compatibleTypes.TypeNames, val.TypeName);
		} 
		EditorGUILayout.BeginVertical ();	
		int newIndex = -1;
		if (fieldWrapper.Label != null) {
			newIndex = EditorGUILayout.Popup (fieldWrapper.Label, currentIndex, compatibleTypes.NicifiedTypeNames);
		} else {
			newIndex = EditorGUILayout.Popup (currentIndex, compatibleTypes.NicifiedTypeNames);
		}
		
		if (currentIndex != newIndex) {
			if (newIndex == -1) {
				fieldWrapper.Value = null;
				val = null;
			} else {
				var type = UTInternalCall.GetType (compatibleTypes.TypeNames [newIndex]);
				var writableMembers = UTComponentScanner.FindPublicWritableMembersOf(type);
				string propertyPath = null;
				if (writableMembers.MemberInfos.Length > 0) {
					propertyPath = writableMembers.MemberInfos[0].Name;
				}
				val = new UTMemberInfo (type.FullName, propertyPath);
				fieldWrapper.Value = val;
			}
		}
		
		GUI.enabled = val != null && !string.IsNullOrEmpty(val.TypeName);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.PrefixLabel (" ");
		if (GUILayout.Button (val.FieldPath, EditorStyles.popup)) {
			var genericMenu = BuildMenu (val.Type, val.SetFieldPath);
			genericMenu.ShowAsContext ();
		}
		EditorGUILayout.EndHorizontal ();
		GUI.enabled = true;
		
		EditorGUILayout.EndVertical ();
	}
	
	private GenericMenu BuildMenu (Type type, GenericMenu.MenuFunction2 function)
	{
		var result = new GenericMenu ();
		BuildMenu (result, type, "", "", false, 0, function);
		return result;
	}
	
	private void BuildMenu (GenericMenu menu, Type type, string itemVisiblePath, string itemInternalPath, bool addSelf, int depth, GenericMenu.MenuFunction2 function)
	{
		if (addSelf) {
			menu.AddItem (new GUIContent (itemVisiblePath + "/" + type.Name), false, function, itemInternalPath);
		}
		var members = UTComponentScanner.FindPublicWritableMembersOf (type);
		foreach (var memberInfo in members.MemberInfos) {
			
			var newInternalPath = string.IsNullOrEmpty (itemInternalPath) ? memberInfo.Name : itemInternalPath + "." + memberInfo.Name;
			var newVisiblePath = string.IsNullOrEmpty (itemVisiblePath) ? memberInfo.Name : itemVisiblePath + "/" + memberInfo.Name;
			if (memberInfo.DeclaringType != typeof(Component)) {
					var memberInfoType = UTInternalCall.GetMemberType (memberInfo);
					if (UTInternalCall.HasMembers (memberInfoType) && depth < 2) {
						BuildMenu (menu, memberInfoType, newVisiblePath, newInternalPath, UTInternalCall.IsWritable (memberInfo), depth + 1, function);
					} else {
						menu.AddItem (new GUIContent (newVisiblePath), false, function, newInternalPath);
					}
			}
		}
	}
	
	
}

