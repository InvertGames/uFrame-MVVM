using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Where on the screen is the camera rendered in pixel coordinates.")]
[UBCategory("Camera")]
public class SetCameraPixelRect : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequired] public UBRect _Value = new UBRect();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Camera.GetValueAs<Camera>(context).pixelRect = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s pixelRect to {1}", _Camera.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.pixelRect = #_Value# ");
	}

}