// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class UTVector3 : UTProperty<Vector3>
{
	[SerializeField]
	private Vector3 val;
	
	public override Vector3 Value {
		get {
			return val;
		}
		set {
			val = value;
		}
	}

	protected override Vector3 CustomCast (object val)
	{
		if (val is Quaternion) {
			var result =  ((Quaternion)val).eulerAngles;
			LogConversion(val, result);
			return result;
		}
		return base.CustomCast(val);
	}
}
