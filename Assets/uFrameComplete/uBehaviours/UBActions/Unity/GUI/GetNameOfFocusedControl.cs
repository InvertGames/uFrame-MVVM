using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Get the name of named control that has focus.")]
[UBCategory("GUI")]
public class GetNameOfFocusedControl : UBAction {

	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.GetNameOfFocusedControl());
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.GetNameOfFocusedControl()");
	}

}