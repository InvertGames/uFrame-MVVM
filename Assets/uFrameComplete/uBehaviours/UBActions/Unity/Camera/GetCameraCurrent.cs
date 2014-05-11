using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The camera we are currently rendering with, for low-level render control only (Read Only).")]
[UBCategory("Camera")]
public class GetCameraCurrent : UBAction {

	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(Camera));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Camera.current);
		}

	}

	public override string ToString(){
	return string.Format("Get current from {0}", "Camera");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Camera.current");
	}

}