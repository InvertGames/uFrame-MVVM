// (C) 2013 Ancient Light Studios. All rights reserved.
using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class UTRect : UTProperty<Rect>
{
	[SerializeField]
	private Rect val;
	
	public override Rect Value {
		get {
			return val;
		}
		set {
			val = value;
		}
	}
}
