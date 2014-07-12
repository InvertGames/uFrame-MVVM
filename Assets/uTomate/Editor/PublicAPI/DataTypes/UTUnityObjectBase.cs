//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UObject  = UnityEngine.Object;
using UnityEngine;

[Serializable]
public class UTUnityObjectBase<T> : UTProperty<T> where T:UObject
{
	[SerializeField]
	private T propertyValue;
	
	public override T Value {
		get {
			return propertyValue;
		}
		set {
			propertyValue = value;
		}
	}
}

