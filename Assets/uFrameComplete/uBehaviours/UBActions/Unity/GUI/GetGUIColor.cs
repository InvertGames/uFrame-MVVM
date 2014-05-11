using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Global tinting color for the GUI.")]
[UBCategory("GUI")]
public class GetGUIColor : UBAction {

	[UBRequireVariable] [UBRequired] public UBColor _Result = new UBColor();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.color);
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = GUI.color");
	}

}