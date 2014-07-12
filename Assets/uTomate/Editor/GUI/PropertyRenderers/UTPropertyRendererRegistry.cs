//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using UnityEngine;

/// <summary>
/// Registry providing the known property renderers to the UTInspectorRenderer.
/// </summary>
public class UTPropertyRendererRegistry
{
	private static Dictionary<Type, UTIPropertyRenderer> cache = null;
	
	/// <summary>
	/// Gets the property renderer for the given type.
	/// </summary>
	/// <returns>
	/// The renderer for type or null if no renderer exists.
	/// </returns>
	/// <param name='type'>
	/// Type.
	/// </param>
	public static UTIPropertyRenderer GetRendererForType (Type type)
	{
		if (cache == null) {
			BuildCache ();
		}
		
		if (cache.ContainsKey (type)) {
			return cache [type];		
		} else {
			if (type.IsGenericType) {
				var genericType = type.GetGenericTypeDefinition (); // get the generic type definition and try to lookup this
				if (cache.ContainsKey (genericType)) {
					cache [type] = cache [genericType]; // update cache so this can be found faster next time
					return cache [type];
				}
			}
			// try supertype
			var supertype = type.BaseType;
			if (supertype != null) {
				var result = GetRendererForType (supertype);
				if (result != null) {
					cache [type] = result; // update cache so this can be found faster next time
				}
			}
			return null;
		}
	}
	
	private static void BuildCache ()
	{
		cache = new Dictionary<Type, UTIPropertyRenderer> ();
		var renderers = UTComponentScanner.FindComponentsAnnotatedWith<UTIPropertyRenderer, UTPropertyRendererAttribute> ();
		
		foreach (var entry in renderers) {
			var rendererAttribute = entry.Value;
			var instance = entry.Key;
			var supportedTypes = rendererAttribute.supportedTypes;
			foreach (var supportedType in supportedTypes) {
				if (cache.ContainsKey (supportedType)) {
					Debug.LogWarning ("There already exists a property renderer for type '" + supportedType + "'");
				}
				cache [supportedType] = instance;
			}
		}
	}
	
}

