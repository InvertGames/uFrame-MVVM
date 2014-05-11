using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Set the name of the next control.")]
[UBCategory("GUI")]
public class SetNextControlName : UBAction {

	
	[UBRequired] public UBString _Name = new UBString();
	protected override void PerformExecute(IUBContext context){
		GUI.SetNextControlName(_Name.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s SetNextControlName w/ {1}", "GUI", _Name.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("GUI.SetNextControlName(#_Name#)");
	}

}