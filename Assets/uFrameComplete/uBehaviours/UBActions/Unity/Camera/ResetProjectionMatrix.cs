using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make the projection reflect normal camera's parameters.")]
[UBCategory("Camera")]
public class ResetProjectionMatrix : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	protected override void PerformExecute(IUBContext context){
		_Camera.GetValueAs<Camera>(context).ResetProjectionMatrix();
	}

	public override string ToString(){
	return string.Format("Reset {0}'s Projection Matrix", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.ResetProjectionMatrix()");
	}

}