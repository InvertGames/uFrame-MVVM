using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"How the camera clears the background.")]
[UBCategory("Camera")]
public class SetCameraClearFlags : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequired] public UBEnum _Value = new UBEnum(typeof(CameraClearFlags));
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Camera.GetValueAs<Camera>(context).clearFlags = ((CameraClearFlags)_Value.GetIntValue(context));
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s clearFlags to {1}", _Camera.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.clearFlags = #_Value# ");
	}

}