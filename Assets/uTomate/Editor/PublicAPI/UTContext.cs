//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.IO;

/// <summary>
/// The UTContext allows to share state between different UTActions. When instantiated, it will collect data from all
/// script extensions.
/// </summary>
public class UTContext
{
	public const string PlaceholderRegexp = "\\$(?<open>\\()?([a-zA-Z_][0-9a-zA-Z_:]*\\??)(?<-open>\\))?(?(open)(?!))";
	public const string ContextVarRegexp = "^[a-zA-Z_][0-9a-zA-Z_:]*\\??$";
	private Dictionary<string,object> properties = new Dictionary<string,object> ();

	/// <summary>
	/// Indexer which allows to store and read arbitrary properties in the build context.
	/// </summary>
	/// <param name='key'>
	/// The key under which the property should be stored.
	/// </param>
	public object this [string key] { 
		get {
			if (key == "me") {
				return Me;
			}
			
			var lenient = false;
			if (key.EndsWith ("?")) {
				key = key.Substring (0, key.Length - 1);
				lenient = true;
			}
			if (!properties.ContainsKey (key)) {
				if (lenient) {
					return null;
				} else {
					throw new UTFailBuildException ("$" + key + " is not a known property.", null);
				}
			}
			return properties [key];
		} 
		set {
			if (!IsValidPropertyName (key)) {
				throw new UTFailBuildException (key + " is not a valid property name", null);
			}
			properties [key] = value;
		} 
	}
	
	public UTAutomationPlanEntry CurrentEntry {
		get;
		set;
	}
	
	public bool CancelRequested {
		get;
		set;
	}
	
	public bool Failed {
		get;
		set;
	}
	
	public object Me {
		get;
		set;
	}
	
	
	/// <summary>
	/// Gets or sets the local progress. This is used by actions to set their progress.
	/// </summary>
	/// <value>
	/// The local progress. Values should be between 0 and 1. A value of -1 means that the action does not track it's progress.
	/// </value>
	public float LocalProgress {
		get;
		set;
	}
	
	
	/// <summary>
	/// Determines if the given string is a valid property name.
	/// </summary>
	/// <returns>
	/// <c>true</c> if the given string is a valid property name, otherwise <c>false</c>.
	/// </returns>
	public static bool IsValidPropertyName (string name)
	{
		return  name != "me" && Regex.IsMatch (name, ContextVarRegexp);
	}
	
	/// <summary>
	///  Ctor. Initializes the context with all known script extensions.
	/// </summary>
	public UTContext ()
	{
		// register script extension components
		var extensions = UTScriptExtensionRegistry.GetScriptExtensions ();
		foreach (var extension in extensions) {
			if (extension.Value.variable != null) {
				if (properties.ContainsKey (extension.Value.variable)) {
					Debug.LogWarning ("Script extension " + extension.Key.GetType ().FullName + 
						" would hide existing variable " + extension.Value.variable + ". It will not be registered.");
					continue;
				}
				if (UTPreferences.DebugMode) {
					Debug.Log ("Registering script extension " + extension.Value.variable + " of type " + extension.Key.GetType ().FullName);
				}
				this [extension.Value.variable] = extension.Key;
			}
			
			if (extension.Key is UTIContextAware) { // inject context if extension wants to know about it.
				((UTIContextAware)extension.Key).Context = this; 
			}
		}
	}
	
	/// <summary>
	/// Unsets the specified property.
	/// </summary>
	/// <param name='property'>
	/// the property to unset.
	/// </param>
	public void Unset (string property)
	{
		if (properties.ContainsKey (property)) {
			properties.Remove (property);
		}
	}
	
	/// <summary>
	/// Checks if the context contains the given property.
	/// </summary>
	public bool ContainsProperty(string name) {
		return properties.ContainsKey(name);
	}
	
	/// <summary>
	/// Evaluate the specified input and replaces all known placeholders. Placeholders can be written as
	/// $foo.bar or optionally $(foo.bar) in case the first notation is ambiguous.
	/// 
	/// You can write arbitrary JavaScript expressions like:
	/// 
	/// 2 + 2  (yields 4)
	/// '2' + '2' (yields '22')
	/// '2' == '2' (yields true)
	///  etc..
	/// 
	/// Please note that properties can have different types and casting/transformation is not performed
	/// automatically. E.g. If the property "foo" has the value "2" (as a string) $foo * 2 will yield "22" 
	/// because it is a string. If you want to have the numeric value use parseInt($foo) * 2 (will yield 4).
	///
	/// </summary>
	/// <param name='input'>
	/// The input string.
	/// </param> 
	public object Evaluate (string input)
	{	
		
		
		var replaced = Regex.Replace (input, PlaceholderRegexp, "context['$1']");
		try {
			// we call this via reflection because we cannot reference unityscript stuff directly
			return UTInternalCall.InvokeStatic ("UTEval", "Evaluate", replaced, this);
		} catch (Exception e) {
			throw new UTFailBuildException ("Unparseable expression: '" + input + "' " + e.Message, null); // no experiments.
		}
	
	}
	
	
}
