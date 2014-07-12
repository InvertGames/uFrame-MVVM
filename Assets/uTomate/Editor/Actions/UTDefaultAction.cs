//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Reflection;
using UnityEditor;

/// <summary>
/// Annotation that marks an action as a build-in action
/// </summary>
[AttributeUsageAttribute(AttributeTargets.Class)]
public class UTDefaultAction : System.Attribute
{
	
	public UTDefaultAction () {
	}
		
	/// <summary>
	/// Gets the UTDefaultAction attached to the given type.
	/// </summary>
	/// <returns>
	/// The UTDefaultAction or null, if there is no UTDefaultAction on the type.
	/// </returns>
	public static UTDefaultAction GetFor(Type type) {
		var annotations = type.GetCustomAttributes(typeof(UTDefaultAction), false);	
		if (annotations.Length == 1) {
			return (UTDefaultAction) annotations[0];
		}
		return null;
	}
}

