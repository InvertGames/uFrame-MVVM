//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Registry for script extensions.
/// </summary>
public class UTScriptExtensionRegistry
{
	private static List<KeyValuePair<object, UTScriptExtension>> cachedSortedList;
	
	/// <summary>
	/// Gets the script extensions ordered by their load order.
	/// </summary>
	public static List<KeyValuePair<object,UTScriptExtension>> GetScriptExtensions() {
		if (cachedSortedList == null ) {
			var cache = new Dictionary<object, UTScriptExtension>();
			var scannedComponents = UTComponentScanner.FindComponentsAnnotatedWith<object,UTScriptExtension>();
			List<string> variableNames = new List<string>();
			foreach(var item in scannedComponents) {
				var annotation = item.Value;
				var component = item.Key;
				if (annotation.variable != null && variableNames.Contains(annotation.variable)) {
					Debug.LogWarning(component.GetType().FullName + " is annotated as " + annotation.variable + " but this variable name is already in use.");
					continue;
				} 
				else {
					cache.Add(item.Key, item.Value);
				}
			}
			cachedSortedList = new List<KeyValuePair<object, UTScriptExtension>>();
			cachedSortedList.AddRange(cache);
			cachedSortedList.Sort( (item1, item2) => (item1.Value.loadOrder.CompareTo(item2.Value.loadOrder)) );
		}
		return cachedSortedList;
	}
}

