using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Revert the aspect ratio to the screen's aspect ratio.")]
[UBCategory("Camera")]
public class ResetAspect : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	protected override void PerformExecute(IUBContext context){
		_Camera.GetValueAs<Camera>(context).ResetAspect();
	}

	public override string ToString(){
	return string.Format("Reset {0}'s Aspect ratio", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.ResetAspect()");
	}

}