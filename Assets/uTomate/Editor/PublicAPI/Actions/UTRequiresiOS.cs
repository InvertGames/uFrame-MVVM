//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;

/// <summary>
/// Attribute to mark actions which require Unity iOS or Unity iOS Pro. The inspector will display a warning when these
/// actions are being used in an environment where they are not supported.
/// </summary>
[AttributeUsageAttribute(AttributeTargets.Class)]
[Obsolete("Please use UTRequiresLicenseAttribute instead.")]
public class UTRequiresiOS : System.Attribute
{
	public bool iOSPro = false;
	
	public static bool RequiresiOS(Type type) {
		return type.GetCustomAttributes(typeof(UTRequiresiOS), true).Length > 0;
	}
	
	public static bool RequiresiOSPro(Type type) {
		var attrs = type.GetCustomAttributes(typeof(UTRequiresiOS), true);
		return attrs.Length > 0 && (attrs[0] as UTRequiresiOS).iOSPro;
	}

}

