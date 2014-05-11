using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"The global skin to use.")]
[UBCategory("GUI")]
public class GetGUISkin : UBAction {

	[UBRequireVariable] [UBRequired] public UBObject _Result = new UBObject(typeof(GUISkin));
	protected override void PerformExecute(IUBContext context){
		if (_Result != null){
			_Result.SetValue( context, GUI.skin);
		}

	}

	public override string ToString(){
	return string.Format("Get skin from {0}", "GUI");
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_Result# = GUI.skin");
	}

}