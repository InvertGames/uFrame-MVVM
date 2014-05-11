using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Render the camera manually.")]
[UBCategory("Camera")]
public class Render : UBAction {

	[UBRequired] public UBObject _Camera = new UBObject(typeof(Camera));
	protected override void PerformExecute(IUBContext context){
		_Camera.GetValueAs<Camera>(context).Render();
	}

	public override string ToString(){
	    return string.Format("Render {0}", _Camera.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Camera#.Render()");
	}

}