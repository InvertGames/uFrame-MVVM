using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Tinting color for all text rendered by the GUI.")]
[UBCategory("GUI")]
public class GetGUIContentColor : UBAction {

	[UBRequireVariable] [UBRequired] public UBColor _Result = new UBColor();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.contentColor);
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = GUI.contentColor");
	}

}