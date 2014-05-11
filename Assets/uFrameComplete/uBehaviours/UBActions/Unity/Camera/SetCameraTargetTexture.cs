using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Destination render texture (Unity Pro only).")]
[UBCategory("Camera")]
public class SetCameraTargetTexture : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequired] public UBObject _Value = new UBObject(typeof(RenderTexture));
	protected override void PerformExecute(IUBContext context){
		if (_Value != null){
			_Camera.GetValueAs<Camera>(context).targetTexture = _Value.GetValueAs<RenderTexture>(context);
		}

	}

	public override string ToString(){
	return string.Format("Set {0}'s targetTexture to {1}", _Camera.ToString(RootContainer), _Value.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.targetTexture = #_Value# ");
	}

}