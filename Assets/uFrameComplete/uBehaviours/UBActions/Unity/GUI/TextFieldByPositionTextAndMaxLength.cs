using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Make a single-line text field where the user can edit a string.")]
[UBCategory("GUI")]
public class TextFieldByPositionTextAndMaxLength : UBAction {

	
	[UBRequired] public UBRect _Position = new UBRect();
	
	[UBRequired] public UBString _Text = new UBString();
	
	[UBRequired] public UBInt _MaxLength = new UBInt();
	[UBRequireVariable] [UBRequired] public UBString _Result = new UBString();
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.TextField(_Position.GetValue(context),_Text.GetValue(context),_MaxLength.GetValue(context)));
		}

	}



	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.TextField(#_Position#, #_Text#, #_MaxLength#)");
	}

}