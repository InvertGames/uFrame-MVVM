//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Wrapper around the int datatype.
/// </summary>
[Serializable]
public class UTInt : UTProperty<int> {
	
	
	[SerializeField]
	private int propertyValue = 0;
	
	public override int Value {
		get {
			return propertyValue;
		}
		set {
			propertyValue = value;
		}
	}
	
	/// <summary>
	/// Tries to cast the given object into an int. Only strings can be converted. The method will try to
	/// parse the string as int. If this fails, the cast will be delegated to the base method (which will fail the build).
	/// </summary>
	protected override int CustomCast (object val)
	{
		if (val is string) {
			int result = 0;
			if (int.TryParse((string)val, out result)) {
				LogConversion(val, result);
				return result;
			}
		}
		return base.CustomCast(val);
	}
	
}

