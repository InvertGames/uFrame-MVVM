//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
/// Wrapper around the float data type.
/// </summary>
[Serializable]
public class UTFloat : UTProperty<float> {
	
	
	[SerializeField]
	private float propertyValue = 0f;
	
	public override float Value {
		get {
			return propertyValue;
		}
		set {
			propertyValue = value;
		}
	}
	
	/// <summary>
	/// Tries to cast the given object into a float. Only strings can be converted. The method will try to
	/// parse the string as float. If this fails, the cast will be delegated to the base method (which will fail the build).
	/// </summary>
	protected override float CustomCast (object val)
	{
		if (val is string) {
			float result = 0;
			if (float.TryParse((string)val, out result)) {
				LogConversion(val, result);
				return result;
			}
		}
		return base.CustomCast(val);
	}
	
}

