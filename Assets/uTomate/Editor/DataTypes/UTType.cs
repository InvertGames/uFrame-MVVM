//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//
using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Property for type fields
/// </summary>
[Serializable]
public class UTType : UTProperty<UTTypeInfo>
{
	
	[SerializeField]
	private UTTypeInfo propertyValue = null;
	
	public override UTTypeInfo Value {
		get {
			return propertyValue;
		}
		set {
			propertyValue = value;
		}
	}

	/// <summary>
	/// Tries to cast the given object into a UTTypeInfo. Only strings can be converted. 
	/// </summary>
	protected override UTTypeInfo CustomCast (object val)
	{
		// we can convert strings to types
		if (val is string) {
			return new UTTypeInfo ((string)val);
		}
		
		// delegate to base method.
		return base.CustomCast (val);
	}
	
	
}

