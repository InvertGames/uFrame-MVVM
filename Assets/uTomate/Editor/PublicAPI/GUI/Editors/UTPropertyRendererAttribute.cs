//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;

/// <summary>
/// Attribute used to mark property renderers.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class UTPropertyRendererAttribute : System.Attribute
{
	public Type[] supportedTypes;
	
	public UTPropertyRendererAttribute (params Type[] supportedTypes)
	{
		this.supportedTypes = supportedTypes;
	}
	
	public static UTPropertyRendererAttribute GetFor (Type type)
	{
		var attrs = type.GetCustomAttributes (typeof(UTPropertyRendererAttribute), false);	
		if (attrs.Length == 1) {
			return (UTPropertyRendererAttribute)attrs [0];
		}
		return null;
	}
}

