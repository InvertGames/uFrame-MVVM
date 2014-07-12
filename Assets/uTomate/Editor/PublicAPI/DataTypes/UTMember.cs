//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Reflection;
using System.Text.RegularExpressions;

/// <summary>
/// Wrapper around the FieldInfo datatype. Can custom-cast strings into UTMemberInfos. Format is [TypeName]:[FieldPath].
/// </summary>
[Serializable]
public class UTMember : UTProperty<UTMemberInfo> {
	
	private static Regex theRegex = new Regex("([a-zA-Z0-9_\\.]):([a-zA-Z0-9_\\.])");
	
	[SerializeField]
	private UTMemberInfo info;
	
	public override UTMemberInfo Value {
		get {
			return info;
		}
		set {
			info = value;
		}
	}

	/// <summary>
	/// Tries to cast the given object into a UTMemberInfo. Only strings can be converted. Format is [TypeName]:[FieldPath]
	/// </summary>
	protected override UTMemberInfo CustomCast (object val)
	{
		// we can convert strings 
		if (val is string) {
			var match = theRegex.Match((string)val);
			if (match.Success) {
				var typeName = match.Groups[1].Value;
				var fieldPath = match.Groups[2].Value;
				var result = new UTMemberInfo(typeName, fieldPath);
				LogConversion(val, result);
				return result;
			}
		}
	
		
		// delegate to base method.
		return base.CustomCast(val);
	}
	
}

