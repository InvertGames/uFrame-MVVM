// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UTCompatibleTypesResult
{
	
	private string[] typeNames;
	private GUIContent[] nicifiedTypeNames;
	
	public UTCompatibleTypesResult (IEnumerable<Type> types)
	{
		SortedDictionary<string, string> sortedNames = new SortedDictionary<string, string> (); 
		
		foreach (var type in types) {
			sortedNames.Add (ObjectNames.NicifyVariableName (type.Name), type.FullName);
		}
		
		typeNames = new string[sortedNames.Count];
		nicifiedTypeNames = new GUIContent[sortedNames.Count];
		
		var i = 0;
		foreach (var key in sortedNames.Keys) {
			nicifiedTypeNames[i] = new GUIContent(key);
			typeNames[i] = sortedNames[key];
			i++;
		}
	}
	
	public string[] TypeNames {
		get {
			return typeNames;
		}
	}
	
	public GUIContent[] NicifiedTypeNames {
		get {
			return nicifiedTypeNames;
		}
	}
}

