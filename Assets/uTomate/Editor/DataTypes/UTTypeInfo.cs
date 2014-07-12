// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEngine;

[Serializable]
public class UTTypeInfo
{
	
	[SerializeField]
	private string typeName;
	
	[NonSerialized]
	private Type theType;
	[NonSerialized]
	private bool lookupDone;
	
	public UTTypeInfo ()
	{
	}
	
	public UTTypeInfo (string typeName)
	{
		this.typeName = typeName;
	}
	
	public string TypeName {
		get {
			return typeName;
		}
	}
	
	public Type Type {
		get {
			if (!lookupDone) {
				lookupDone = true;
				theType = UTInternalCall.GetType(typeName);			
			}
			return theType;
		}
	}
}

