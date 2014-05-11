using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Global tinting color for all background elements rendered by the GUI.")]
[UBCategory("GUI")]
public class GetGUIBackgroundColor : UBAction {

	[UBRequireVariable] [UBRequired] public UBColor _Result = new UBColor();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.backgroundColor);
		}

	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = GUI.backgroundColor");
	}

}