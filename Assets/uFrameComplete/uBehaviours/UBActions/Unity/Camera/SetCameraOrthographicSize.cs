using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Camera's half-size when in orthographic mode.")]
[UBCategory("Camera")]
public class SetCameraOrthographicSize : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequired] public UBFloat _Value = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Camera.GetValueAs<Camera>(context).orthographicSize = _Value.GetValue(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s orthographicSize to {1}", _Camera.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.orthographicSize = #_Value# ");
	}

}