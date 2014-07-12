//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Base class for all runnable actions.
/// </summary>
[UTDoc(title="General")]
public abstract class UTAction : ScriptableObject{
	
	/// <summary>
	/// Execute this instance.
	/// </summary>
	/// <param name="context">The current build context.</param>
	public abstract IEnumerator Execute(UTContext context);
	
	/// <summary>
	/// The action version. This is used to facilitate later migration steps if actions change.
	/// </summary>
	public static string ActionVersion = "1.1";
	
	[HideInInspector]
	public string CreatedWithActionVersion;
	
	/// <summary>
	/// Creates a new UBuildAction of the given type.
	/// </summary>
	/// <typeparam name='T'>
	/// The type of action to create.
	/// </typeparam>
	public static T Create<T>() where T : UTAction {
		UTDoc doc = UTDoc.GetFor(typeof(T));
		var result = UTils.CreateAssetOfType<T>(doc.title);
		result.CreatedWithActionVersion = ActionVersion;
		return result;
	}
	
	/// <summary>
	/// Evaluates the given array of UTProperties in the given context and returns an array of the
	/// evaluated values.
	/// </summary>
	public static T[] EvaluateAll<T>(UTProperty<T>[] propertyArray, UTContext context) {
		if (propertyArray == null) {
			return new T[0];
		}
		var result = new T[propertyArray.Length];
		for(int i = 0; i < propertyArray.Length; i++) {
			result[i] = propertyArray[i].EvaluateIn(context);
		}
		return result;
	}
	
	public override string ToString ()
	{
		return name;
	}
}
