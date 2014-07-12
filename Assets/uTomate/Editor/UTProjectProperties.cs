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
/// These properties are project wide and can be committed to the asset server.
/// </summary>
[Serializable]
public class UTProjectProperties : ScriptableObject
{
	[SerializeField]
	private string[] propertyNames;
	[SerializeField]
	private UTString[] propertyValues;
	[SerializeField]
	private bool[] isMachineSpecific;
	[SerializeField]
	private bool[] isPrivate;
	
	public void OnEnable ()
	{
		if (propertyNames == null) {
			propertyNames = new string[0];
			EditorUtility.SetDirty (this);
		}
		if (propertyValues == null) {
			propertyValues = new UTString[0];
			EditorUtility.SetDirty (this);
		}
		
		if (isMachineSpecific == null) {
			isMachineSpecific = new bool[0];
			EditorUtility.SetDirty (this);
		}
		
		// properties were added in 1.3, this does the migration process.
		if (isMachineSpecific.Length != propertyNames.Length) {
			Array.Resize (ref isMachineSpecific, propertyNames.Length);
			EditorUtility.SetDirty (this);
		}
		
		if (isPrivate == null) {
			isPrivate = new bool[0];
			EditorUtility.SetDirty (this);
		}

		// properties were added in 1.3, this does the migration process.
		if (isPrivate.Length != propertyNames.Length) {
			Array.Resize (ref isPrivate, propertyNames.Length);
			EditorUtility.SetDirty (this);
		}
	}
	
	/// <summary>
	/// Applies the project properties to the given context.
	/// </summary>
	public void ApplyTo (UTContext context)
	{
		for (int i = 0; i < propertyNames.Length; i++) {
			var propName = propertyNames [i];
			var propValue = propertyValues [i];
			var propIsPrivate = isPrivate [i];
			object theRealValue = null;
			if (propValue.UseExpression) {
				theRealValue = context.Evaluate (propValue.Expression);
			} else {
				theRealValue = propValue.Value;
			}
			
			if (UTPreferences.DebugMode) {
				var valueOutput = theRealValue;
				if (propIsPrivate && !propValue.UseExpression && valueOutput != null) {
					valueOutput = new string ('*', valueOutput.ToString ().Length);
				}
				Type valueType = theRealValue != null ? theRealValue.GetType () : null;
				Debug.Log ("Setting property '" + propName + "' to " + 
				(valueType != null ? "[" + valueType.Name + "] " : "") + valueOutput, this);
			}
			
			context [propName] = theRealValue;
		}
	}
	
	public void AddProperty (string propertyName)
	{
		Array.Resize<string> (ref propertyNames, propertyNames.Length + 1);
		Array.Resize<UTString> (ref propertyValues, propertyValues.Length + 1);
		Array.Resize<bool> (ref isMachineSpecific, isMachineSpecific.Length + 1);
		Array.Resize<bool> (ref isPrivate, isPrivate.Length + 1);
		
		propertyNames [propertyNames.Length - 1] = propertyName;
		propertyValues [propertyValues.Length - 1] = new UTString ();
		isMachineSpecific [isMachineSpecific.Length - 1] = false;
		isPrivate [isPrivate.Length - 1] = false;
		
		EditorUtility.SetDirty (this);
	}
	
	public void DeleteProperty (UTConfigurableProperty property)
	{
		var prop = property as UTConfigurableProjectProperty;
		if (prop == null) {
			throw new ArgumentException ("Wrong property type.");
		}
		
		int idx = prop.index;
		List<string> namesList = new List<string> (propertyNames);
		namesList.RemoveAt (idx);
		propertyNames = namesList.ToArray ();
			
		List<UTString> valuesList = new List<UTString> (propertyValues);
		valuesList.RemoveAt (idx);
		propertyValues = valuesList.ToArray ();

		List<bool> isMachineSpecificList = new List<bool> (isMachineSpecific);
		isMachineSpecificList.RemoveAt (idx);
		isMachineSpecific = isMachineSpecificList.ToArray ();

		List<bool> isPrivateList = new List<bool> (isPrivate);
		isPrivateList.RemoveAt (idx);
		isPrivate = isPrivateList.ToArray ();
		EditorUtility.SetDirty (this);
	}
	
	public IEnumerable<UTConfigurableProperty> Properties ()
	{
		var index = 0;
		while (index < propertyNames.Length) {
			yield return new UTConfigurableProjectProperty(this, index);
			index++;
		}
	}
	
	public bool PropertyExists (string propertyName)
	{
		return Array.IndexOf (propertyNames, propertyName) != -1;
	}
	
	private class UTConfigurableProjectProperty : UTConfigurableProperty
	{
		private UTProjectProperties parent;
		internal int index;
		
		public UTConfigurableProjectProperty (UTProjectProperties parent, int index)
		{
			this.parent = parent;
			this.index = index;
		}
		
		public string Name {
			get {
				return parent.propertyNames [index];
			}
			set {
				parent.propertyNames [index] = value;
			}
		}
		
		public string Value {
			get {
				return parent.propertyValues [index].Value;
			}
			set {
				parent.propertyValues [index].Value = value;
			}
		}
		
		public string Expression {
			get {
				return parent.propertyValues [index].Expression;
			}
			set {
				parent.propertyValues [index].Expression = value;
			}
		}
		
		public bool UseExpression {
			get {
				return parent.propertyValues [index].UseExpression;
			}
			set {
				parent.propertyValues [index].UseExpression = value;
			}
		}
		
		public bool IsMachineSpecific {
			get {
				return parent.isMachineSpecific [index];
			}
			set {
				parent.isMachineSpecific [index] = value;
			}
		}
		
		public bool IsPrivate {
			get {
				return parent.isPrivate [index];
			}
			set {
				parent.isPrivate [index] = value;
			}
		}
		
		public bool SupportsPrivate {
			get {
				return IsMachineSpecific;
			}
		}
	}
}

