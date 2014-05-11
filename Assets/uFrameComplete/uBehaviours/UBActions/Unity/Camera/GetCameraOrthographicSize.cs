using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Camera's half-size when in orthographic mode.")]
[UBCategory("Camera")]
public class GetCameraOrthographicSize : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequireVariable] [UBRequired] public UBFloat _Result = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Camera.GetValueAs<Camera>(context).orthographicSize);
		}

	}

	public override string ToString(){
	return string.Format("Get orthographicSize from {0}", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Camera#.orthographicSize");
	}

}