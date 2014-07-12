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
/// Annotation for marking a script extension class. A script extension can register itself under a certain name
/// in the context. If the extension implements <see cref="UTIContextAware"/> then the context will be injected
/// into the script extension. Script extensions can use this to register additional variables to the context.
/// </summary>
[AttributeUsageAttribute(AttributeTargets.Class)]
public class UTScriptExtension : System.Attribute
{
	/// <summary>
	/// The variable name at which this extension will be bound in the context. If the name is null, then
	/// the script extension will be not registered insde the context.
	/// </summary>
	public string variable;
	
	/// <summary>
	/// The load order. Lower values will be loaded first. Values 0-1000 are reserved for uTomate, please use
	/// values larger than that.
	/// </summary>
	public int loadOrder;
	
	public UTScriptExtension() : this(null, 1001) {
	}
	
	public UTScriptExtension(int loadOrder) : this (null, loadOrder) {
	}
	
	public UTScriptExtension(string variable) : this(variable, 1001) {
	}
	
	public UTScriptExtension (string variable, int loadOrder)
	{
		this.variable = variable;
		this.loadOrder = loadOrder;
	}
}

