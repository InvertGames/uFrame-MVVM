using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;


[UBCategory("Camera")]
public class RenderDontRestore : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	protected override void PerformExecute(IUBContext context){
		_Camera.GetValueAs<Camera>(context).RenderDontRestore();
	}

	public override string ToString(){
	return string.Format("Render {0} & DontRestore", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.RenderDontRestore()");
	}

}