using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Destination render texture (Unity Pro only).")]
[UBCategory("Camera")]
public class GetCameraTargetTexture : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(RenderTexture));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Camera.GetValueAs<Camera>(context).targetTexture);
		}

	}

	public override string ToString(){
	return string.Format("Get targetTexture from {0}", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Camera#.targetTexture");
	}

}