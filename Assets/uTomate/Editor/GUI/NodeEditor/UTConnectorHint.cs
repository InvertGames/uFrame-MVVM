//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Reflection;
using UnityEditor;

/// <summary>
/// Attribute which allows to specify hints on where to place connectors to the node renderer. The hints are
/// evaluated in <see cref="UTNode"/>
/// </summary>
[AttributeUsageAttribute(AttributeTargets.Field)]
public class UTConnectorHint : System.Attribute
{
	public ConnectorLocation connectorLocation = ConnectorLocation.Right;

	/// <summary>
	/// Gets the UTConnectorHint attached to the given field.
	/// </summary>
	public static UTConnectorHint GetFor(FieldInfo field) {
		var hints = field.GetCustomAttributes(typeof(UTConnectorHint), true);		
		if (hints.Length > 0) {
			return (UTConnectorHint) hints[0];
		}
		return new UTConnectorHint();
	}
	
	public enum ConnectorLocation {
		Right,
		Bottom,
		Left
	}
}

