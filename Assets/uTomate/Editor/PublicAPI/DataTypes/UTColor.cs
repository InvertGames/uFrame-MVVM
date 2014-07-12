//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

/// <summary>
///  Wrapper around the <code>Color</code> data type.
/// </summary>
[Serializable]
public class UTColor : UTProperty<Color> {
	
	
	[SerializeField]
	private Color propertyValue = Color.blue;
	
	public override Color Value {
		get {
			return propertyValue;
		}
		set {
			propertyValue = value;
		}
	}
	
}

