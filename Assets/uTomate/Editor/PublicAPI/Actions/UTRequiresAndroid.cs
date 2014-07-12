//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;

/// <summary>
/// Attribute to mark actions which require Unity Android or Unity Android Pro. The inspector will display a warning when these
/// actions are being used in an environment where they are not supported.
/// </summary>
[AttributeUsageAttribute(AttributeTargets.Class)]
[Obsolete("Please use UTRequiresLicenseAttribute instead.")]
public class UTRequiresAndroid : System.Attribute
{
	public bool androidPro = false;
	
	public static bool RequiresAndroid(Type type) {
		return type.GetCustomAttributes(typeof(UTRequiresAndroid), true).Length > 0;
	}
	
	public static bool RequiresAndroidPro(Type type) {
		var attrs = type.GetCustomAttributes(typeof(UTRequiresAndroid), true);
		return attrs.Length > 0 && (attrs[0] as UTRequiresAndroid).androidPro;
	}

}

