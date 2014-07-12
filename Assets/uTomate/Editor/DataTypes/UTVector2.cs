// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class UTVector2 : UTProperty<Vector2>
{
	[SerializeField]
	private Vector2 val;
	
	public override Vector2 Value {
		get {
			return val;
		}
		set {
			val = value;
		}
	}
}
