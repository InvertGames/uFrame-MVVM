using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The first enabled camera tagged ""MainCamera"" (Read Only).")]
[UBCategory("Camera")]
public class GetCameraMain : UBAction {

	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(Camera));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Camera.main);
		}

	}

	public override string ToString(){
	return string.Format("Get main from {0}", "Camera");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Camera.main");
	}

}