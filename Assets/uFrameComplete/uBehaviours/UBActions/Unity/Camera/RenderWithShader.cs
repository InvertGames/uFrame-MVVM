using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Render the camera with shader replacement.")]
[UBCategory("Camera")]
public class RenderWithShader : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	
	[UBRequired] public UBObject _Shader = new UBObject(typeof(Shader));
	
	[UBRequired] public UBString _ReplacementTag = new UBString();
	protected override void PerformExecute(IUBContext context){
		_Camera.GetValueAs<Camera>(context).RenderWithShader(_Shader.GetValueAs<Shader>(context),_ReplacementTag.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Render {0} with {1} shader", _Camera.ToString(RootContainer), _Shader.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.RenderWithShader(#_Shader#, #_ReplacementTag#)");
	}

}