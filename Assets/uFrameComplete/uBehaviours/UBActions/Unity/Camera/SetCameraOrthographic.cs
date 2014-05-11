using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Is the camera orthographic (true) or perspective (false)?")]
[UBCategory("Camera")]
public class SetCameraOrthographic : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequired] public UBBool _Value = new UBBool();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Camera.GetValueAs<Camera>(context).orthographic = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s orthographic to {1}", _Camera.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.orthographic = #_Value# ");
	}

}