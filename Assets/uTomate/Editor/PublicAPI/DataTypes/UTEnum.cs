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
/// Base class for UTProperties which wrap Enum types. Since Unity cannot serialize generic classes you need
/// to derive from this class instead of just using a UTEnum&lt;T&gt;
/// </summary>
[Serializable]
public abstract class UTEnum<T> : UTProperty<T> {
	
	
	[SerializeField]
	private T propertyValue = ((T[]) Enum.GetValues(typeof(T)))[0];
	
	public override T Value {
		get {
			return propertyValue;
		}
		set {
			propertyValue = value;
		}
	}
		
	/// <summary>
	/// Returns the type of the enum that this property wraps.
	/// </summary>
	public Type EnumType {
		get {
			return typeof(T);
		}
	}
	
	
	/// <summary>
	/// Tries to cast the given object into an enum member of the wrapped enum. Only strings can be
	/// converted. The string will be de-nicified and is subsequently matched against the enum members.
	/// If a matching enum member is found it will be returned. Otherwise the cast will be delegated to the base
	/// class (which will fail the build).
	/// </summary>
	protected override T CustomCast (object val)
	{
		// we can convert strings to enums of the name matches.
		if (val is string) {
			// de-nicify, in case someone wrote the nicified name.
			val = ((string)val).Replace(" ", "");
			
			var result = Enum.Parse(typeof(T), (string)val);
			if (result == null) {
				throw new UTFailBuildException("String " + val + " created by Expression " + Expression + " is not a member of enum " + typeof(T).Name, null);
			}
			LogConversion(val, result);
			return (T)result;
		}
		
		// delegate to base method.
		return base.CustomCast(val);
	}
	
	
}

