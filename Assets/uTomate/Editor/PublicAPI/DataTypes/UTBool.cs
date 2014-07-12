//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Wrapper around the bool data type. 
/// </summary>
[Serializable]
public class UTBool : UTProperty<bool> {
	
	
	[SerializeField]
	private bool propertyValue = false;
	
	public override bool Value {
		get {
			return propertyValue;
		}
		set {
			propertyValue = value;
		}
	}
	
	/// <summary>
	/// Casts the given object to a bool. If the object is a string and the string's value is "true" in any letter 
	/// case, the result will be <code>true</code> otherwise the result will be false.
	/// </summary>
	protected override bool CustomCast (object val)
	{
		var result = false;
		if (val is Boolean) {
			return (bool) val;
		}
		if (val is string ) {
			result = ((string)val).Equals("true", StringComparison.OrdinalIgnoreCase);
		}
		
		LogConversion(val, result);
		return result;
	}
	
	
}

