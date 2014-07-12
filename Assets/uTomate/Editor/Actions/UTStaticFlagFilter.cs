using System;
using UnityEngine;
using UnityEditor;

public class UTStaticFlagFilter : UTFilter
{
	
	private StaticEditorFlags[] staticFlags;
	
	public UTStaticFlagFilter (StaticEditorFlags[] staticFlags)
	{
		this.staticFlags = staticFlags;
	}
	
	public bool Accept (object o)
	{
		GameObject go = o as GameObject;
		if (go == null) {
			return false;
		}
		
		if (staticFlags == null || staticFlags.Length == 0) {
			return false;
		}
		
		foreach (var staticFlag in staticFlags) {
			var theCurrentStaticFlags = GameObjectUtility.GetStaticEditorFlags (go);
			if ((theCurrentStaticFlags & staticFlag) == staticFlag) {
				return true;
			}
		}
		return false;
	}
}

