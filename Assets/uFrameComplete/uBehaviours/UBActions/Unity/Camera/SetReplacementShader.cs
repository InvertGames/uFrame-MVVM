using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make the camera render with shader replacement.")]
[UBCategory("Camera")]
public class SetReplacementShader : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	
	[UBRequired] public UBObject _Shader = new UBObject(typeof(Shader));
	
	[UBRequired] public UBString _ReplacementTag = new UBString();
	protected override void PerformExecute(IUBContext context){
		_Camera.GetValueAs<Camera>(context).SetReplacementShader(_Shader.GetValueAs<Shader>(context),_ReplacementTag.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s SetReplacementShader w/ {1} and {2}", _Camera.ToString(RootContainer), _Shader.ToString(RootContainer), _ReplacementTag.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.SetReplacementShader(#_Shader#, #_ReplacementTag#)");
	}

}