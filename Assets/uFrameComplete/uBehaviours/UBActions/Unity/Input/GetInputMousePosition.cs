using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The current mouse position in pixel coordinates. (Read Only)")]
[UBCategory("Input")]
public class GetInputMousePosition : UBAction {

	[UBRequireVariable] [UBRequired] public UBVector3 _Result = new UBVector3();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, Input.mousePosition);
		}

	}

	public override string ToString(){
	return string.Format("Get mousePosition from {0}", "Input");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = Input.mousePosition");
	}

}