using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Makes this camera's settings match other camera.")]
[UBCategory("Camera")]
public class CopyFrom : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	
	[UBRequired] public UBObject _Other = new UBObject(typeof(Camera));
	protected override void PerformExecute(IUBContext context){
		_Camera.GetValueAs<Camera>(context).CopyFrom(_Other.GetValueAs<Camera>(context));
	}

	public override string ToString(){
	return string.Format("Transfer Camera settings from {0} to {1}", _Camera.ToString(RootContainer), _Other.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.CopyFrom(#_Other#)");
	}

}