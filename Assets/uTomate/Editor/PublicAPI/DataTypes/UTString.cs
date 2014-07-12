//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Wrapper around the string datatype.
/// </summary>
[Serializable]
public class UTString : UTProperty<string> {
	
	
	[SerializeField]
	private string propertyValue = "";
	
	public override string Value {
		get {
			return propertyValue;
		}
		set {
			propertyValue = value;
		}
	}
	
	/// <summary>
	/// Converts the given object into a string by calling it's <code>ToString()</code> method. If the object is null
	/// will return the string "null".
	/// </summary>
	protected override string CustomCast (object val)
	{
		var result = val == null ? "null" : val.ToString();
		LogConversion(val, result);
		return result;
	}
	
}

