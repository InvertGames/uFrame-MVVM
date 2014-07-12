// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class UTQuaternion : UTProperty<Quaternion>
{
	[SerializeField]
	private Quaternion val;
	
	public override Quaternion Value {
		get {
			return val;
		}
		set {
			val = value;
		}
	}
}
