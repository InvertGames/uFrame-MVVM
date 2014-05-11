using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Remove shader replacement from camera.")]
[UBCategory("Camera")]
public class ResetReplacementShader : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	protected override void PerformExecute(IUBContext context){
		_Camera.GetValueAs<Camera>(context).ResetReplacementShader();
	}

	public override string ToString(){
        return string.Format("Reset {0}'s Replacement Shader", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.ResetReplacementShader()");
	}

}