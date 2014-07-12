//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;

/// <summary>
/// Attribute providing action metadata for documentation purposes.
/// </summary>
[AttributeUsageAttribute(AttributeTargets.Class)]
public class UTActionInfoAttribute : System.Attribute
{
	public string minUTomateVersion = null;
	public string maxUTomateVersion = null;
	public string minUnityVersion = null;
	public string maxUnityVersion = null;
	public string actionCategory = "General";
	public string sinceUTomateVersion = "1.0.0";
	
	public static UTActionInfoAttribute GetFor(Type type) {
		var info = type.GetCustomAttributes(typeof(UTActionInfoAttribute), false);	
		if (info.Length == 0) {
			return new UTActionInfoAttribute();
		}
		return info[0] as UTActionInfoAttribute;
	}
}


