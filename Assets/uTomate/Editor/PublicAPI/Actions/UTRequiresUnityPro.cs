//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Reflection;

/// <summary>
/// Attribute to mark actions which require unity pro. The inspector will display a warning when these
/// actions are being used in Unity Free.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
[Obsolete("Please use UTRequiresLicenseAttribute instead.")]
public class UTRequiresUnityPro : System.Attribute
{
	
	public static bool RequiresUnityPro(Type type) {
		return type.GetCustomAttributes(typeof(UTRequiresUnityPro), true).Length > 0;
	}
	
	public static bool RequiresUnityPro(FieldInfo fieldInfo) {
		return fieldInfo.GetCustomAttributes(typeof(UTRequiresUnityPro), true).Length > 0;
	}
}

