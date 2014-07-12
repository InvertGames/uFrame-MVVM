// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEditor;
using System.IO.Ports;
using UnityEngine;

[Serializable]
public class UTMemberInfo
{
	
	[SerializeField]
	private string typeName;
	[SerializeField]
	private string fieldPath;
	[NonSerialized]
	private Type theType;
	[NonSerialized]
	private bool lookupDone;
	
	public UTMemberInfo ()
	{
	}
	
	public UTMemberInfo (string typeName, string fieldPath)
	{
		this.typeName = typeName;
		this.fieldPath = fieldPath;
	}
	
	public Type Type {
		get {
			if (!lookupDone) {
				lookupDone = true;
				this.theType = UTInternalCall.GetType (typeName);
			}
			return theType;
		}
	}

	public string TypeName {
		get {
			return typeName;
		}
	}
	
	public string FieldPath {
		get {
			return fieldPath;
		}
	}
	
	public bool FullyDefined {
		get {
			return !string.IsNullOrEmpty(typeName) && !string.IsNullOrEmpty(fieldPath);	
		}
	}
	
	/// <summary>
	/// Kinda hacky callback for the inspector...
	/// </summary>
	public void SetFieldPath(object fieldPath) {
		this.fieldPath = (string) fieldPath;	
	}
}

