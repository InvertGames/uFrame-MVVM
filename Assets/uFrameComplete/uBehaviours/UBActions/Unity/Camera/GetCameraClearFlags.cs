using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"How the camera clears the background.")]
[UBCategory("Camera")]
public class GetCameraClearFlags : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	[UBRequireVariable] [UBRequired] public UBEnum _Result = new UBEnum(typeof(CameraClearFlags));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, _Camera.GetValueAs<Camera>(context).clearFlags);
		}

	}

	public override string ToString(){
	return string.Format("Get clearFlags from {0}", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = #_Camera#.clearFlags");
	}

}