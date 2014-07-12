using System;
using UnityEngine;
using System.Collections.Generic;

public class UTComponentFilter : UTFilter
{
	
	private Type[] componentTypes;
	
	public UTComponentFilter (UTTypeInfo[] componentTypes)
	{
		if (componentTypes != null) {
			List<Type> finalTypes = new List<Type> ();
			foreach (var componentType in componentTypes) {
				var finalType = componentType.Type;
				if (finalType == null) {
					Debug.LogWarning("There is no component of type " + componentType.TypeName + " in the current project. Did you delete it accidently?");
				}
				else {
					finalTypes.Add(finalType);
				}
			}
			this.componentTypes = finalTypes.ToArray();
		}
	}
	
	public bool Accept (object o)
	{
		GameObject go = o as GameObject;
		if (go == null) {
			return false;
		}
		
		if (componentTypes == null || componentTypes.Length == 0) {
			return false;
		}
		
		foreach (var componentType in componentTypes) {
			if (componentType != null && go.GetComponent (componentType) != null) {
				return true;
			}
		}
		return false;
	}
}

