//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// These properties editor wide that is they are usable on all projects. The properties are
/// persisted in the EditorPreferences.
/// </summary>
public class UTEditorProperties
{
	
	private const string PropSpace = "uTomate.editorProperties.";
	private static List<UTConfigurableEditorProperty> properties;
	private static bool isDirty = true;
	
	public static IEnumerable<UTConfigurableProperty> Properties() {
		if (isDirty) {
			RebuildCaches();
		}
		foreach(var prop in properties) {
			yield return prop;
		}
	}
	
	public static void AddProperty (string propertyName)
	{
		properties.Add(new UTConfigurableEditorProperty(propertyName,"","",false,false));
		PersistCaches();
	}
	
	public static void DeleteProperty (UTConfigurableProperty property)
	{
		var prop = property as UTConfigurableEditorProperty;
		if (prop  == null) {
			throw new ArgumentException("Illegal property type.");
		}
		
		properties.Remove(prop);
		PersistCaches();
	}

	public static bool PropertyExists (string propertyName)
	{
		if (isDirty) {
			RebuildCaches ();
		}
		return properties.FindIndex(p=>p.Name == propertyName) != -1;
	}
	
	
	/// <summary>
	/// Applies the editor properties to the given context.
	/// </summary>
	public static void ApplyTo (UTContext context)
	{
		if (isDirty) {
			RebuildCaches ();
		}
		
		foreach (var prop in properties) {
			bool propIsExpression = prop.UseExpression;
			bool propIsPrivate = prop.IsPrivate;
			
			object theRealValue = null;
			if (propIsExpression) {
				theRealValue = context.Evaluate (prop.Expression);
			} else {
				theRealValue = prop.Value;
			}
			
			if (UTPreferences.DebugMode) {
				var valueOutput = theRealValue;
				if (propIsPrivate && !propIsExpression && valueOutput != null) {
					valueOutput = new string ('*', valueOutput.ToString ().Length);
				}
				
				Type valueType = theRealValue != null ? theRealValue.GetType () : null;
				Debug.Log ("Setting property '" + prop.Name + "' to " + 
				(valueType != null ? "[" + valueType.Name + "] " : "") + valueOutput);
			}
			
			context [prop.Name] = theRealValue;
		}
	}
	
	private static void RebuildCaches ()
	{
		var propCount = PropCount;
		properties = new List<UTConfigurableEditorProperty>();
		
		for (int i = 0; i < propCount; i++) {
			
			var name = EditorPrefs.GetString (PropSpace + "name." + i);
			var propValue = EditorPrefs.GetString (PropSpace + "value." + i);
			var propExpression = EditorPrefs.GetString (PropSpace + "expression." + i);
			var isExpression =  EditorPrefs.GetBool (PropSpace + "isexpression." + i);
			var isPrivate = EditorPrefs.GetBool (PropSpace + "isprivate." + i);
			properties.Add (new UTConfigurableEditorProperty(name, propValue, propExpression, isExpression, isPrivate));
		}
		isDirty = false;
	}
	
	private static void PersistCaches ()
	{
		int oldCount = PropCount;
		int newCount = properties.Count;
		for (int i = 0; i < newCount; i++) {
			var prop = properties[i];
			EditorPrefs.SetString (PropSpace + "name." + i, prop.Name);
			EditorPrefs.SetString (PropSpace + "value." + i, prop.Value);
			EditorPrefs.SetString (PropSpace + "expression." + i, prop.Expression);
			EditorPrefs.SetBool (PropSpace + "isexpression." + i, prop.UseExpression);
			EditorPrefs.SetBool (PropSpace + "isprivate." + i, prop.IsPrivate);
		}
		
		// delete unused keys
		for (int i = newCount; i < oldCount; i++) {
			EditorPrefs.DeleteKey (PropSpace + "name." + i);
			EditorPrefs.DeleteKey (PropSpace + "value." + i);
			EditorPrefs.DeleteKey (PropSpace + "expression." + i);
			EditorPrefs.DeleteKey (PropSpace + "isexpression." + i);
			EditorPrefs.DeleteKey (PropSpace + "isprivate." + i);
		}
		
		PropCount = newCount;
	}
	
	private static int PropCount {
		get {
			return EditorPrefs.GetInt (PropSpace + "Count", 0);
		}
		set {
			EditorPrefs.SetInt (PropSpace + "Count", value);
		}
	}
	
	private class UTConfigurableEditorProperty : UTConfigurableProperty
	{
		private string name;
		private string propValue;
		private string expression;
		private bool useExpression;
		private bool isPrivate;
		
		public UTConfigurableEditorProperty (string name, string propValue, string expression, bool useExpression, bool isPrivate)
		{
			this.name = name;
			this.propValue = propValue;
			this.expression = expression;
			this.useExpression = useExpression;
			this.isPrivate = isPrivate;
		}
		
		public string Name {
			get {
				return name;
			}
			set {
				if (value != name) {
					name = value;
					UTEditorProperties.PersistCaches();
				}
			}
		}
		
		public string Value {
			get {
				return propValue;
			}
			set {
				if (value != propValue) {
					propValue = value;
					UTEditorProperties.PersistCaches();
				}
			}
		}
		
		public string Expression {
			get {
				return expression;
			}
			set {
				if (value != expression) {
					expression = value;
					UTEditorProperties.PersistCaches();
				}
			}
		}
		
		public bool UseExpression {
			get {
				return useExpression;
			}
			set {
				if (value != useExpression) {
					useExpression = value;
					UTEditorProperties.PersistCaches();
				}
			}
		}
		
		public bool IsMachineSpecific {
			get {
				return false;
			}
			set {
				// editor properties are by definition machine specific.
			}
		}
		
		public bool IsPrivate {
			get {
				return isPrivate;
			}
			set {
				if (value != isPrivate) {
					isPrivate = value;
					UTEditorProperties.PersistCaches();
				}
			}
		}
		
		public bool SupportsPrivate {
			get {
				return true;
			}
		}
	}
}

